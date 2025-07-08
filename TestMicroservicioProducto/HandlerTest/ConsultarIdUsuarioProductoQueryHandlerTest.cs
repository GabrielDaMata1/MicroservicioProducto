using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Exception;
using Application.Handler;
using Application.Querys;
using Domain.Interfaces;
using Moq;

namespace TestMicroservicioProducto.HandlerTest
{
    public class ConsultarIdUsuarioProductoQueryHandlerTest
    {
        private readonly Mock<IProductoService> _mockProductoService = new();
        private readonly Guid _productoId = Guid.NewGuid();
        private readonly Guid _usuarioId = Guid.NewGuid();

        [Fact]
        public async Task Handle_DeberiaRetornarIdUsuarioCorrectamente()
        {
            var query = new ConsultarIdUsuarioProductoQuery(_productoId);
            var handler = new ConsultarIdUsuarioProductoQueryHandler(_mockProductoService.Object);

            _mockProductoService
                .Setup(s => s.ObtenerIdUsuarioPorProductoIdMongo(_productoId))
                .ReturnsAsync(_usuarioId);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(_usuarioId, resultado);
            _mockProductoService.Verify(s => s.ObtenerIdUsuarioPorProductoIdMongo(_productoId), Times.Once);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_SiIdUsuarioEsVacio()
        {
            var query = new ConsultarIdUsuarioProductoQuery(_productoId);
            var handler = new ConsultarIdUsuarioProductoQueryHandler(_mockProductoService.Object);

            _mockProductoService
                .Setup(s => s.ObtenerIdUsuarioPorProductoIdMongo(_productoId))
                .ReturnsAsync(Guid.Empty);

            var ex = await Assert.ThrowsAsync<FalloAlObtenerProductoException>(() => handler.Handle(query, CancellationToken.None));
            Assert.Contains("No se pudo obtener el ID del usuario", ex.Message);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_SiServicioLanzaError()
        {
            var query = new ConsultarIdUsuarioProductoQuery (_productoId);
            var handler = new ConsultarIdUsuarioProductoQueryHandler(_mockProductoService.Object);

            _mockProductoService
                .Setup(s => s.ObtenerIdUsuarioPorProductoIdMongo(_productoId))
                .ThrowsAsync(new Exception("Error inesperado"));

            var ex = await Assert.ThrowsAsync<FalloAlObtenerProductoException>(() => handler.Handle(query, CancellationToken.None));
            Assert.Contains("Ocurrió un error al obtener el producto", ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.Equal("Error inesperado", ex.InnerException.Message);
        }

    }
}
