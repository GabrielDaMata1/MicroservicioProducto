using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exception;
using Application.Exceptions;
using Domain.Events;
using Domain.Interfaces;
using Infrastructure.Consumers;
using MassTransit;
using Moq;

namespace TestMicroservicioProducto.ConsumersTest
{
    public class EliminarProductoConsumerTest
    {
        private readonly Mock<IProductoService> _mockProductoService = new();
        private readonly EliminarProductoConsumer _consumer;

        public EliminarProductoConsumerTest()
        {
            _consumer = new EliminarProductoConsumer(_mockProductoService.Object);
        }

        [Fact]
        public async Task Consume_DeberiaEliminarProducto_CuandoEventoEsValido()
        {
            var productoId = Guid.NewGuid();
            var evento = new ProductoEliminadoEvent(productoId);

            var contextMock = new Mock<ConsumeContext<ProductoEliminadoEvent>>();
            contextMock.Setup(c => c.Message).Returns(evento);

            _mockProductoService.Setup(s => s.EliminarProductoMongoAsync(productoId)).ReturnsAsync(HttpStatusCode.OK);

            await _consumer.Consume(contextMock.Object);

            _mockProductoService.Verify(s => s.EliminarProductoMongoAsync(productoId), Times.Once);
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlEliminarProductoException_SiServicioFalla()
        {
            var productoId = Guid.NewGuid();
            var evento = new ProductoEliminadoEvent(productoId);

            var contextMock = new Mock<ConsumeContext<ProductoEliminadoEvent>>();
            contextMock.Setup(c => c.Message).Returns(evento);

            var inner = new Exception("Error interno en Mongo");
            _mockProductoService
                .Setup(s => s.EliminarProductoMongoAsync(productoId))
                .ThrowsAsync(inner);

            var ex = await Assert.ThrowsAsync<FalloAlEliminarProductoException>(() =>
                _consumer.Consume(contextMock.Object));

            Assert.StartsWith("Error al intentar eliminar el producto de MongoDB", ex.Message);
            Assert.Equal(inner, ex.InnerException);
        }



    }
}
