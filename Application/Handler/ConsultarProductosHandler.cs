using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Exception;
using Application.Querys;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    public class ConsultarProductosHandler : IRequestHandler<ConsultarProductosQuery, List<HistorialProductosDTO>>
    {
        private readonly IProductoService _productoService;

        public ConsultarProductosHandler(IProductoService productoService)
        {
            _productoService = productoService;
        }
        public async Task<List<HistorialProductosDTO>> Handle(ConsultarProductosQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var usuarioService = new UsuarioService(new HttpClient());
                var idUsuario = await usuarioService.ObtenerUsuarioPorIdAsync(request.correo);

                var listaProductos = await _productoService.ObtenerProductosPorGuidMongoAsync(idUsuario);
                if (listaProductos == null || !listaProductos.Any())
                {
                    return new List<HistorialProductosDTO>();
                }
                return listaProductos.Select(h => new HistorialProductosDTO(h.Id, h.NombreProducto.Nombre, h.DescripcionProducto.descripcion, h.ImagenURLProducto.url, h.PrecioBaseProducto.precio, h.CategoriaProducto.categoria)).ToList();
            }
            catch (System.Exception ex)
            {
                throw new FalloAlObtenerProductoException("Ocurrió un error al obtener los productos en la base de datos", ex);
            }

        }
    }
}
