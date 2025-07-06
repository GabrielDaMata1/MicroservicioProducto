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
    /// <summary>
    /// Clase consumer que se encarga de consumir el evente ProductoRegistradoEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class ProductoRegistadoConsumer : IConsumer<ProductoRegistradoEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un producto, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IProductoService _productoService;

        public ProductoRegistadoConsumer(IProductoService productoService)
        {
            _productoService = productoService;
        }
        /// <summary>
        /// Método que se encarga de procesar el registro del producto en la base de datos de MongoBD.
        /// </summary>
        /// <param name="context">Parametro que contiene el ID de la categoria del producto, el ID del subastador del producto y un objeto Producto con sus datos.</param>
        public async Task Consume(ConsumeContext<ProductoRegistradoEvent> context)
        {
            //Se registra el producto de la base de datos en MongoDB
            await _productoService.RegistrarProductoMongoAsync(context.Message.producto, context.Message.idCategoria, context.Message.idUsuario);
        }
    }
}
