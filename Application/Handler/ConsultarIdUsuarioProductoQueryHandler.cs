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
    public class ConsultarIdUsuarioProductoQueryHandler : IRequestHandler<ConsultarIdUsuarioProductoQuery, Guid>
    {
        private readonly IProductoService _productoService;

        public ConsultarIdUsuarioProductoQueryHandler( IProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<Guid> Handle(ConsultarIdUsuarioProductoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var idUsuario = await _productoService.ObtenerIdUsuarioPorProductoIdMongo(request.IdProducto);

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
