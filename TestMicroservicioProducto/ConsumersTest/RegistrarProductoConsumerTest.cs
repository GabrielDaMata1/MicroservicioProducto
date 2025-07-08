using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exception;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Domain.Value_Object;
using Infrastructure.Consumers;
using MassTransit;
using Moq;

namespace TestMicroservicioProducto.ConsumersTest
{
    public class RegistrarProductoConsumerTest
    {
        private readonly Mock<IProductoService> _mockProductoService = new();
        private readonly ProductoRegistadoConsumer _consumer;

        public RegistrarProductoConsumerTest()
        {
            _consumer = new ProductoRegistadoConsumer(_mockProductoService.Object);
        }

        [Fact]
        public async Task Consume_DeberiaRegistrarProducto_CuandoEventoEsValido()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"));
            var idCategoria = 1;
            var idUsuario = Guid.NewGuid();
            var evento = new ProductoRegistradoEvent(producto, idCategoria, idUsuario);

            var contextMock = new Mock<ConsumeContext<ProductoRegistradoEvent>>();
            contextMock.Setup(c => c.Message).Returns(evento);

            _mockProductoService
                .Setup(s => s.RegistrarProductoMongoAsync(producto, idCategoria, idUsuario))
                .ReturnsAsync(HttpStatusCode.Created);

            await _consumer.Consume(contextMock.Object);

            _mockProductoService.Verify(s => s.RegistrarProductoMongoAsync(producto, idCategoria, idUsuario), Times.Once);
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlRegistrarProductoException_SiServicioFalla()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"));
            var idCategoria = 1;
            var idUsuario = Guid.NewGuid();
            var evento = new ProductoRegistradoEvent(producto, idCategoria, idUsuario);

            var contextMock = new Mock<ConsumeContext<ProductoRegistradoEvent>>();
            contextMock.Setup(c => c.Message).Returns(evento);

            var inner = new Exception("Error interno en Mongo");
            _mockProductoService
                .Setup(s => s.RegistrarProductoMongoAsync(producto, idCategoria, idUsuario))
                .ThrowsAsync(inner);

            var ex = await Assert.ThrowsAsync<FalloAlRegistrarProductoException>(() =>
                _consumer.Consume(contextMock.Object));

            Assert.StartsWith("Error al intentar registrar el producto de MongoDB", ex.Message);
            Assert.Equal(inner, ex.InnerException);
        }
    }
}
