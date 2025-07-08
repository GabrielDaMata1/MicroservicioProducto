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
    public class ConsultarProductosHandlerTest
    {
        private readonly Mock<IProductoService> _mockProductoService = new();
        private readonly Mock<IUsuarioService> _mockUsuarioService = new();
        private readonly Guid _usuarioId = Guid.NewGuid();
        private readonly string _correo = "subastador@ejemplo.com";

        [Fact]
        public async Task Handle_DeberiaRetornarListaDeProductosDTO_CuandoExistenProductos()
        {
            var productos = new List<Producto>
        {
            new Producto(
                Guid.NewGuid(),
                new NombreProductoVO("Producto 1"),
                new DescripcionProductoVO("Descripción 1"),
                new ImagenURLProductoVO("https://img.com/1.jpg"),
                new PrecioBaseProductoVO(100),
                new CategoriaProductoVO("Tecnología"),
                new EstadoProductoVO("Disponible")
            ),
            new Producto(
                Guid.NewGuid(),
                new NombreProductoVO("Producto 2"),
                new DescripcionProductoVO("Descripción 2"),
                new ImagenURLProductoVO("https://img.com/2.jpg"),
                new PrecioBaseProductoVO(200),
                new CategoriaProductoVO("Hogar"),
                new EstadoProductoVO("Subastado")
            )
        };

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(_correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerProductosPorGuidMongoAsync(_usuarioId))
                .ReturnsAsync(productos);

            var handler = new ConsultarProductosHandler(_mockProductoService.Object, _mockUsuarioService.Object);
            var query = new ConsultarProductosQuery (_correo);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Contains(resultado, p => p.NombreProducto == "Producto 1");
            Assert.Contains(resultado, p => p.EstadoProducto == "Subastado");
        }

        [Fact]
        public async Task Handle_DeberiaRetornarListaVacia_CuandoNoHayProductos()
        {
            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(_correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerProductosPorGuidMongoAsync(_usuarioId))
                .ReturnsAsync(new List<Producto>());

            var handler = new ConsultarProductosHandler(_mockProductoService.Object, _mockUsuarioService.Object);
            var query = new ConsultarProductosQuery (_correo);

            var resultado = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarFalloAlObtenerProductoException_CuandoOcurreError()
        {
            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(_correo))
                .ThrowsAsync(new Exception("Error inesperado"));

            var handler = new ConsultarProductosHandler(_mockProductoService.Object, _mockUsuarioService.Object);
            var query = new ConsultarProductosQuery (_correo);

            var ex = await Assert.ThrowsAsync<FalloAlObtenerProductoException>(() => handler.Handle(query, CancellationToken.None));
            Assert.Contains("Ocurrió un error al obtener los productos", ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.Equal("Error inesperado", ex.InnerException.Message);
        }

    }
}
