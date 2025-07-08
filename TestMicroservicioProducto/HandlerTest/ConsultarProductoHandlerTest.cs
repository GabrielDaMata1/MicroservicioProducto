using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exception;
using Application.Handler;
using Application.Querys;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Value_Object;
using Moq;

namespace TestMicroservicioProducto.HandlerTest
{
    public class ConsultarProductoHandlerTest
    {
        private readonly Mock<IProductoService> _mockProductoService = new();
        private readonly Guid _productoId = Guid.NewGuid();

        [Fact]
        public async Task Handle_DeberiaRetornarDTO_CuandoProductoExiste()
        {
            var producto = new Producto(
                _productoId,
                new NombreProductoVO("Producto Test"),
                new DescripcionProductoVO("Descripción"),
                new ImagenURLProductoVO("https://img.com"),
                new PrecioBaseProductoVO(100),
                new CategoriaProductoVO("Tecnología"),
                new EstadoProductoVO("Disponible")
            );

            _mockProductoService
                .Setup(s => s.ObtenerProductoPorIdMongo(_productoId))
                .ReturnsAsync(producto);

            var handler = new ConsultarProductoHandler(_mockProductoService.Object);
            var query = new ConsultarProductoQuery(_productoId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(producto.Id, resultado.Id);
            Assert.Equal("Producto Test", resultado.NombreProducto);
            Assert.Equal("Disponible", resultado.EstadoProducto);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarProductoNoEncontradoException_CuandoProductoEsNull()
        {
            _mockProductoService
                .Setup(s => s.ObtenerProductoPorIdMongo(_productoId))
                .ReturnsAsync((Producto)null);

            var handler = new ConsultarProductoHandler(_mockProductoService.Object);
            var query = new ConsultarProductoQuery (_productoId);

            await Assert.ThrowsAsync<ProductoNoEncontradoException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarFalloAlObtenerProductoException_CuandoOcurreError()
        {
            _mockProductoService
                .Setup(s => s.ObtenerProductoPorIdMongo(_productoId))
                .ThrowsAsync(new Exception("Error inesperado"));

            var handler = new ConsultarProductoHandler(_mockProductoService.Object);
            var query = new ConsultarProductoQuery (_productoId);

            var ex = await Assert.ThrowsAsync<FalloAlObtenerProductoException>(() => handler.Handle(query, CancellationToken.None));
            Assert.Contains("Ocurrió un error al obtener el producto", ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.Equal("Error inesperado", ex.InnerException.Message);
        }

    }
}
