using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.Exception;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    /// <summary>
    /// Clase Handler que se encarga eliminar el producto de un subastador en las bases de datos (PostgreSQL,MongoDB) .
    /// </summary>
    public class EliminarProductoHandler : IRequestHandler<EliminarProductoCommand, bool>
    {
        /// <summary>
        /// Atributo que corresponde a las publicación de mensajes a la cola de RabbitMQ.
        /// </summary>
        private readonly IPublishEndpoint _publishEndpoint;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un producto, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IProductoService _productoService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un usuario, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IUsuarioService _usuarioService;

        public EliminarProductoHandler(IProductoService productoService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService)
        {
            _publishEndpoint = publishEndpoint;
            _productoService = productoService;
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Método que se encarga de procesar la eliminación de un producto específico.
        /// </summary>
        /// <param name="request">Parametro que contiene und DTO con el ID del producto a eliminar y el correo del subastador 
        /// al que le pertenece el producto.</param>
        /// <returns>Retorna un valor booleano True si las operaciones fueron exitosas.</returns>
        /// <exception cref="PermisoNoAutorizadoException">
        /// Esta excepcion ocurre si el estado del producto no se encuentra en disponible.
        /// También puede ocurrir esta excepción si el producto no le pertenece al usuario autenticado
        /// </exception>
        /// <exception cref="FalloAlEliminarProductoException">
        /// Esta excepcion ocurre si ocurre un error al elminar el producto en la base de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<bool> Handle(EliminarProductoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Se obtiene el ID del subastador por medio de su correo en la base de datos en MongoDB.
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.correo);

                // Se obtiene el producto a eliminar en la base de datos en MongoDB.
                var producto = await _productoService.ObtenerProductoPorIdMongo(request.ProductoDto.idProducto);

                // En caso de que el estado del producto no sea "Disponible", se lanza una excepción.
                if (producto.EstadoProducto.estadoProducto.Equals("Subastando") || producto.EstadoProducto.estadoProducto.Equals("Subastado")) 
                    throw new PermisoNoAutorizadoException("Error: Este producto se encuentra subastando o ya fue subastado.");

                // Se obtiene el ID del subastador del producto a eliminar en la base de datos en MongoDB.
                var productoUsuarioId = await _productoService.ObtenerIdUsuarioPorProductoIdMongo(request.ProductoDto.idProducto).ConfigureAwait(false);
                
                // En caso de que el ID del subastador del producto no se corresponda con el ID del subastador autenticado, se lanza una excepción.
                if (productoUsuarioId != idUsuario)
                    throw new PermisoNoAutorizadoException("Error: Este producto no le pertenece al usuario autenticado.");
                
                // Se elimina el producto en la base de datos de PostgreSQL.
                var productoEliminado = await _productoService.EliminarProductoPostgreSQLAsync(request.ProductoDto.idProducto).ConfigureAwait(false);

                // En caso de que la operación para eliminar el producto no sea exitosa, se lanza una excepción.
                if (productoEliminado != HttpStatusCode.OK)
                    throw new FalloAlEliminarProductoException("Ha ocurrido un error al eliminar el producto en la base de datos de PostgreSQL");

                //Se publica el mensaje en la cola de RabbitMQ para sincronizar la base de datos de MongoDB con PostgreSQL
                await _publishEndpoint.Publish(new ProductoEliminadoEvent(request.ProductoDto.idProducto)).ConfigureAwait(false);

                return true;
            }
            catch (PermisoNoAutorizadoException)
            {
                throw;
            }
            catch (FalloAlModificarProductoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new FalloAlEliminarProductoException("Ha ocurrido un error al eliminar el producto en la base de datos");
            }
        }
    }
}
