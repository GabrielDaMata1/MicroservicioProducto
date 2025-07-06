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
    /// Clase consumer que se encarga de consumir el evente ProductoEliminadoEvent al ser publicado en la cola de RabbitMQ
    /// </summary>
    public class EliminarProductoConsumer : IConsumer<ProductoEliminadoEvent>
    {
        /// <summary>
        /// Atributo que corresponde a las operaciones posibles que se pueden realizar sobre un producto, el cual será inyectado por inversión de dependencias.
        /// </summary>
        private readonly IProductoService _productoService;

        public EliminarProductoConsumer(IProductoService productoService)
        {
            _productoService = productoService;
        }
        /// <summary>
        /// Método que se encarga de procesar la eliminación del producto en la base de datos de MongoBD.
        /// </summary>
        /// <param name="context">Parametro que contiene el ID del producto a eliminar.</param>
        public async Task Consume(ConsumeContext<ProductoEliminadoEvent> context)
        {
            //Se elimina el producto de la base de datos en MongoDB
            await _productoService.EliminarProductoMongoAsync(context.Message.idProducto);

        }
    }
}


