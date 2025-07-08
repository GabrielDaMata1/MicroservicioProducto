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
    /// <summary>
    /// Clase Handler que se encarga consultar todos los productos de un subastador en la bases de datos en MongoDB.
    /// </summary>
    public class ConsultarProductosHandler : IRequestHandler<ConsultarProductosQuery, List<HistorialProductosDTO>>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un producto, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IProductoService _productoService;
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un usuario en el Microservicio Usuarios, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IUsuarioService _usuarioService;

        public ConsultarProductosHandler(IProductoService productoService, IUsuarioService usuarioService)
        {
            _productoService = productoService;
            _usuarioService = usuarioService;
        }
        /// <summary>
        /// Metodo que se encarga de procesar la consulta para obtener todos los productos de un subastador.
        /// </summary>
        /// <param name="request">Parametro que contiene el correo del subastador.</param>
        /// <returns>Retorna una lista de DTOs con la información de los productos solicitados.</returns>
        /// <exception cref="FalloAlObtenerProductoException">
        /// Esta excepcion ocurre si ocurre un error inesperado.
        /// </exception>
        public async Task<List<HistorialProductosDTO>> Handle(ConsultarProductosQuery request, CancellationToken cancellationToken)
        {

            try
            {
                // Se obtiene el ID del subastador por medio de su correo en la base de datos en MongoDB
                var idUsuario = await _usuarioService.ObtenerUsuarioPorIdAsync(request.correo);

                // Se obtiene la lista de productos pertenecientes al subastador a través de la base de datos en MongoDB
                var listaProductos = await _productoService.ObtenerProductosPorGuidMongoAsync(idUsuario);
                if (listaProductos == null || !listaProductos.Any())
                {
                    return new List<HistorialProductosDTO>();
                }
                return listaProductos.Select(h => new HistorialProductosDTO(h.Id, h.NombreProducto.Nombre, h.DescripcionProducto.descripcion, h.ImagenURLProducto.url, h.PrecioBaseProducto.precio, h.CategoriaProducto.categoria, h.EstadoProducto.estadoProducto)).ToList();
            }
            catch (System.Exception ex)
            {
                throw new FalloAlObtenerProductoException("Ocurrió un error al obtener los productos en la base de datos", ex);
            }

        }
    }
}
