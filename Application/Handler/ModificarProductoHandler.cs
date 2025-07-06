using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exception;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using MassTransit.Initializers.Variables;
using MediatR;

namespace Application.Handler
{
    /// <summary>
    /// Clase Handler que se encarga modificar el producto de un subastador en las bases de datos (PostgreSQL,MongoDB) .
    /// </summary>
    public class ModificarProductoHandler : IRequestHandler<ModificarProductoCommand, bool>
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


        public ModificarProductoHandler(IProductoService productoService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService)
        {
            _publishEndpoint = publishEndpoint;
            _productoService = productoService;
            _usuarioService = usuarioService;
        }


        /// <summary>
        /// Método que se encarga de procesar la modificación de un producto específico.
        /// </summary>
        /// <param name="request">Parametro que contiene und DTO con el ID del producto a modificar y el correo del subastador 
        /// al que le pertenece el producto.</param>
        /// <returns>Retorna un valor booleano True si las operaciones fueron exitosas.</returns>
        /// <exception cref="UsuarioNoEncontradoException">
        /// Esta excepcion ocurre si no se pudo obtener el ID del usuario en el Microservicio Usuarios.
        /// </exception>
        /// <exception cref="FalloAlModificarProductoException">
        /// Esta excepcion ocurre si ocurre un error al modificar el producto en las bases de datos o si ocurre un error inesperado.
        /// </exception>
        public async Task<bool> Handle(ModificarProductoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Se obtiene el ID del subastador al que le pertenece el producto dado.
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.correo);

                //En caso de que el Id del subastador retornado por la consulta sea vacío, se lanza la excepción
                if (idUsuario == Guid.Empty || idUsuario == null)
                    throw new UsuarioNoEncontradoException();

                //Se obtiene el id de la categoria correspondiente al producto en la base de datos en MongoDB
                var idCategoria = await _productoService.ObtenerIdCategoriaMongo(request.ProductoDto.CategoriaProducto);

                //Se crea el producto a modificar en la base de datos en MongoDB
                var producto = ProductoFactory.CrearProductoConId(request.ProductoDto.Id, request.ProductoDto.NombreProducto, request.ProductoDto.DescripcionProducto, request.ProductoDto.ImagenURLProducto, request.ProductoDto.PrecioBaseProducto, request.ProductoDto.EstadoProducto);

                // Se modifica el producto en la base de datos de PostgreSQL.
                var productoId = await _productoService.ModificarProductoPostgreSQL(producto, idUsuario, idCategoria);

                // En caso de que la operación para modificar el producto no sea exitosa, se lanza una excepción.
                if (productoId != HttpStatusCode.OK)
                    throw new FalloAlModificarProductoException("No se pudo modificar el producto en la base de datos de PostgreSQL");

                //Se publica el mensaje en la cola de RabbitMQ para sincronizar la base de datos de MongoDB con PostgreSQL
                await _publishEndpoint.Publish(new ProductoModificadoEvent(producto, idCategoria, idUsuario));
                return true;
                
            }
            catch (UsuarioNoEncontradoException)
            {
                throw;
            }
            catch (FalloAlModificarProductoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new FalloAlModificarProductoException("Ocurrió un error al modificar el producto en la base de datos", ex);
            }










        }
    }
}
