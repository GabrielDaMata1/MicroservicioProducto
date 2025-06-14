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
    public class EliminarProductoConsumer : IConsumer<ProductoEliminadoEvent>
    {
        private readonly IProductoService _productoService;

        public EliminarProductoConsumer(IProductoService productoService)
        {
            _productoService = productoService;
        }
        public async Task Consume(ConsumeContext<ProductoEliminadoEvent> context)
        {
            await _productoService.EliminarProductoMongoAsync(context.Message.idProducto);

        }
    }
}


