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
    /// <summary>
    /// Clase Handler que se encarga consultar un producto en la bases de datos en MongoDB.
    /// </summary>
    public class ConsultarProductoHandler : IRequestHandler<ConsultarProductoQuery, HistorialProductosDTO>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un producto, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IProductoService _productoService;

        public ConsultarProductoHandler(IProductoService productoService)
        {
            _productoService = productoService;
        }
        /// <summary>
        /// Método que se encarga de procesar la consulta para obtener un producto específico.
        /// </summary>
        /// <param name="request">Parametro que contiene el ID del producto.</param>
        /// <returns>Retorna un DTO con la información del producto solicitado.</returns>
        /// <exception cref="ProductoNoEncontradoException">
        /// Esta excepcion ocurre si no se pudo obtener el producto desde la base de datos de MongoBD.
        /// </exception>
        /// <exception cref="FalloAlObtenerProductoException">
        /// Esta excepcion ocurre si ocurre un error inesperado.
        /// </exception>
        public async Task<HistorialProductosDTO> Handle(ConsultarProductoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Se obtiene el producto desde la base de datos de mongo cuyo ID fue dado en la solicitud al endpoint.
                var producto = await _productoService.ObtenerProductoPorIdMongo(request.IdProducto);
                //En caso de que el producto retornado por la consulta sea vacío, se lanza la excepción
                if (producto == null)
                {
                    throw new ProductoNoEncontradoException();
                }
                return new HistorialProductosDTO(producto.Id, producto.NombreProducto.Nombre, producto.DescripcionProducto.descripcion, producto.ImagenURLProducto.url, producto.PrecioBaseProducto.precio, producto.CategoriaProducto.categoria, producto.EstadoProducto.estadoProducto);
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
