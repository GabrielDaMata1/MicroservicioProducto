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
    public class ModificarProductoHandler : IRequestHandler<ModificarProductoCommand, bool>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IProductoService _productoService;
        private readonly IUsuarioService _usuarioService;


        public ModificarProductoHandler(IProductoService productoService, IPublishEndpoint publishEndpoint, IUsuarioService usuarioService)
        {
            _publishEndpoint = publishEndpoint;
            _productoService = productoService;
            _usuarioService = usuarioService;
        }

        public async Task<bool> Handle(ModificarProductoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.correo);

                var idCategoria = await _productoService.ObtenerIdCategoriaMongo(request.ProductoDto.CategoriaProducto);

                var producto = ProductoFactory.CrearProductoConId(request.ProductoDto.Id, request.ProductoDto.NombreProducto, request.ProductoDto.DescripcionProducto, request.ProductoDto.ImagenURLProducto, request.ProductoDto.PrecioBaseProducto);
                var productoId = await _productoService.ModificarProductoPostgreSQL(producto, idUsuario, idCategoria);

                if (productoId != HttpStatusCode.OK)
                    throw new FalloAlModificarProductoException("No se pudo modificar el producto en la base de datos de PostgreSQL");

                await _publishEndpoint.Publish(new ProductoModificadoEvent(producto, idCategoria, idUsuario));
                return true;
                
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
