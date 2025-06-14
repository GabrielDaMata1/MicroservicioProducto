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
    public class EliminarProductoHandler : IRequestHandler<EliminarProductoCommand, bool>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IProductoService _productoService;
        private readonly IUsuarioService _usuarioService;

        public EliminarProductoHandler(IProductoService productoService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService)
        {
            _publishEndpoint = publishEndpoint;
            _productoService = productoService;
            _usuarioService = usuarioService;
        }
        public async Task<bool> Handle(EliminarProductoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.correo);

                var productoUsuarioId = await _productoService.ObtenerIdUsuarioPorProductoIdMongo(request.ProductoDto.idProducto).ConfigureAwait(false);

                if (productoUsuarioId != idUsuario)
                    throw new PermisoNoAutorizadoException("Error: Este producto no le pertenece al usuario autenticado.");

                var productoEliminado = await _productoService.EliminarProductoPostgreSQLAsync(request.ProductoDto.idProducto).ConfigureAwait(false);

                if (productoEliminado != HttpStatusCode.OK)
                    throw new FalloAlModificarProductoException("Ha ocurrido un error al eliminar el producto en la base de datos de PostgreSQL");

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
                throw new FalloAlModificarProductoException("Ha ocurrido un error al eliminar el producto en la base de datos");
            }
        }
    }
}
