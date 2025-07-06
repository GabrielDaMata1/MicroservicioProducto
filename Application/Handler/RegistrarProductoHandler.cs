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
    /// Clase Handler que se encarga registrar el producto de un subastador en las bases de datos (PostgreSQL,MongoDB) .
    /// </summary>
    public class RegistrarProductoHandler : IRequestHandler<RegistrarProductoCommand, bool>
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

        public RegistrarProductoHandler(IProductoService productoService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService)
        {
            _publishEndpoint = publishEndpoint;
            _productoService = productoService;
            _usuarioService = usuarioService;
        }

        public async Task<bool> Handle(RegistrarProductoCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.ProductoDto.correo);
                var idCategoria = await _productoService.ObtenerIdCategoriaMongo(request.ProductoDto.categoria);
                if (idCategoria == null)
                    throw new FalloAlRegistrarProductoException("La categoria que indicó no es una categoria válida");

                var producto = ProductoFactory.CrearProducto(request.ProductoDto.nombreProducto, request.ProductoDto.descripcionProducto, request.ProductoDto.imagenURLProducto, request.ProductoDto.precioBase);
                var productoId = await _productoService.RegistrarProductoPostgreSQLAsync(producto, idCategoria, idUsuario);
                if (productoId == Guid.Empty)
                    throw new FalloAlRegistrarProductoException("Ha ocurrido un error al registrar el producto en la base de datos de PostgreSQL");

                await _publishEndpoint.Publish(new ProductoRegistradoEvent(producto, idCategoria, idUsuario));
                return true;
                
            }
            catch (FalloAlRegistrarProductoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new FalloAlRegistrarProductoException("Ha ocurrido un error al registrar el producto en la base de datos");
            }
        }
    }
}
