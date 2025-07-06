using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exception;
using Application.Querys;
using Domain.Interfaces;
using MassTransit;
using MediatR;

namespace Application.Handler
{
    /// <summary>
    /// Clase Handler que se encarga consultar el subastador a quien le pertenece un producto dado en la bases de datos en MongoDB.
    /// </summary>
    public class ConsultarIdUsuarioProductoQueryHandler : IRequestHandler<ConsultarIdUsuarioProductoQuery, Guid>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un producto, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IProductoService _productoService;

        public ConsultarIdUsuarioProductoQueryHandler( IProductoService productoService)
        {
            _productoService = productoService;
        }
        /// <summary>
        /// Metodo que se encarga de procesar la consulta para obtener el ID del usuario al que pertenece un producto específico.
        /// </summary>
        /// <param name="request">Parametro que contiene el ID del producto.</param>
        /// <returns>Retorna el ID del subastador a quien le pertenece el producto.</returns>
        /// <exception cref="FalloAlObtenerProductoException">
        /// Esta excepcion ocurre si no se pudo obtener el ID del usuario en el MicroservicioUsuarios o si ocurre un error inesperado.
        /// </exception>

        public async Task<Guid> Handle(ConsultarIdUsuarioProductoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Se obtiene el ID del subastador al que le pertenece el producto dado.
                var idUsuario = await _productoService.ObtenerIdUsuarioPorProductoIdMongo(request.IdProducto);

                //En caso de que el Id del subastador retornado por la consulta sea vacío, se lanza la excepción
                if (idUsuario == Guid.Empty)
                    throw new FalloAlObtenerProductoException("No se pudo obtener el ID del usuario al que le pertenece el producto en la base de datos ");

                return idUsuario;
            }
            catch (FalloAlObtenerProductoException)
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
