using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using Domain.Interfaces;
using MassTransit;

namespace Infrastructure.Consumers
{
    public class ProductoRegistadoConsumer : IConsumer<ProductoRegistradoEvent>
    {
        private readonly IProductoService _productoService;

        public ProductoRegistadoConsumer(IProductoService productoService)
        {
            _productoService = productoService;
        }
        public async Task Consume(ConsumeContext<ProductoRegistradoEvent> context)
        {
            await _productoService.RegistrarProductoMongoAsync(context.Message.producto, context.Message.idCategoria, context.Message.idUsuario);
        }
    }
}
