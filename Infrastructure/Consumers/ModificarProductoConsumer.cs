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
    public class ModificarProductoConsumer : IConsumer<ProductoModificadoEvent>
    {
        private readonly IProductoService _productoService;

        public ModificarProductoConsumer(IProductoService productoService)
        {
            _productoService = productoService;
        }
        public async Task Consume(ConsumeContext<ProductoModificadoEvent> context)
        {
            await _productoService.ModificarProductoMongoAsync(context.Message.producto, context.Message.idCategoria, context.Message.idUsuario);
        }
    }
}
