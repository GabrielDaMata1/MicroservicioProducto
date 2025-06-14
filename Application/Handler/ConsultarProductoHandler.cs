using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exception;
using Application.Querys;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Handler
{
    public class ConsultarProductoHandler : IRequestHandler<ConsultarProductoQuery, HistorialProductosDTO>
    {
        private readonly IProductoService _productoService;

        public ConsultarProductoHandler(IProductoService productoService)
        {
            _productoService = productoService;
        }
        public async Task<HistorialProductosDTO> Handle(ConsultarProductoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var producto = await _productoService.ObtenerProductoPorIdMongo(request.IdProducto);
                if (producto == null)
                {
                    throw new ProductoNoEncontradoException();
                }

                return new HistorialProductosDTO(producto.Id, producto.NombreProducto.Nombre, producto.DescripcionProducto.descripcion, producto.ImagenURLProducto.url, producto.PrecioBaseProducto.precio, producto.CategoriaProducto.categoria);
            }
            catch (ProductoNoEncontradoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw new FalloAlObtenerProductoException("Ocurrió un error al obtener el producto en la base de datos", ex);
            }


        }
    }
}
