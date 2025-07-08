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
    public class ModificarProductoConsumerTest
    {
        private readonly Mock<IProductoService> _mockProductoService = new();
        private readonly ModificarProductoConsumer _consumer;

        public ModificarProductoConsumerTest()
        {
            _consumer = new ModificarProductoConsumer(_mockProductoService.Object);
        }

        [Fact]
        public async Task Consume_DeberiaModificarProducto_CuandoEventoEsValido()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"));

            var idCategoria = 1;
            var idUsuario = Guid.NewGuid();
            var evento = new ProductoModificadoEvent(producto, idCategoria, idUsuario);

            var contextMock = new Mock<ConsumeContext<ProductoModificadoEvent>>();
            contextMock.Setup(c => c.Message).Returns(evento);

            _mockProductoService
                .Setup(s => s.ModificarProductoMongoAsync(producto, idCategoria, idUsuario))
                .ReturnsAsync(HttpStatusCode.OK);

            await _consumer.Consume(contextMock.Object);

            // 
            _mockProductoService.Verify(s => s.ModificarProductoMongoAsync(producto, idCategoria, idUsuario), Times.Once);
        }

        [Fact]
        public async Task Consume_DeberiaLanzarFalloAlEliminarProductoException_SiServicioFalla()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"));
            var idCategoria = 1;
            var idUsuario = Guid.NewGuid();
            var evento = new ProductoModificadoEvent(producto, idCategoria, idUsuario);

            var contextMock = new Mock<ConsumeContext<ProductoModificadoEvent>>();
            contextMock.Setup(c => c.Message).Returns(evento);

            var inner = new Exception("Error interno en Mongo");
            _mockProductoService
                .Setup(s => s.ModificarProductoMongoAsync(producto, idCategoria, idUsuario))
                .ThrowsAsync(inner);

            var ex = await Assert.ThrowsAsync<FalloAlEliminarProductoException>(() =>
                _consumer.Consume(contextMock.Object));

            Assert.StartsWith("Error al intentar modificar el producto de MongoDB", ex.Message);
            Assert.Equal(inner, ex.InnerException);
        }

    }
}
