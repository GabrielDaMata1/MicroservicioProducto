using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exception;
using Application.Handler;
using Domain.Entities;
using Domain.Events;
using Domain.Factory;
using Domain.Interfaces;
using MassTransit;
using Moq;

namespace TestMicroservicioProducto.HandlerTest
{
    public class RegistrarProductoHandlerTest
    {
        private readonly Mock<IProductoService> _mockProductoService = new();
        private readonly Mock<IUsuarioService> _mockUsuarioService = new();
        private readonly Mock<IPublishEndpoint> _mockPublishEndpoint = new();

        private readonly Guid _usuarioId = Guid.NewGuid();
        private readonly Guid _productoId = Guid.NewGuid();
        private readonly int _categoriaId = 1;

        private readonly RegistrarProductoDTO _dto = new()
        {
            nombreProducto = "Producto nuevo",
            descripcionProducto = "Descripción del producto",
            imagenURLProducto = "https://img.com/producto.jpg",
            precioBase = 150.00m,
            categoria = "Tecnología",
            correo = "usuario@ejemplo.com"
        };

        [Fact]
        public async Task Handle_DeberiaRegistrarProductoYPublicarEvento()
        {
            var command = new RegistrarProductoCommand(_dto);
            var producto = ProductoFactory.CrearProducto(
                _dto.nombreProducto,
                _dto.descripcionProducto,
                _dto.imagenURLProducto,
                _dto.precioBase
            );

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(_dto.correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerIdCategoriaMongo(_dto.categoria))
                .ReturnsAsync(_categoriaId);

            _mockProductoService.Setup(s => s.RegistrarProductoPostgreSQLAsync(It.IsAny<Producto>(), _categoriaId, _usuarioId))
                .ReturnsAsync(_productoId);

            var handler = new RegistrarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.True(resultado);
            _mockPublishEndpoint.Verify(p => p.Publish(It.IsAny<ProductoRegistradoEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_SiCategoriaEsInvalida()
        {
            var command = new RegistrarProductoCommand(_dto);

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(_dto.correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerIdCategoriaMongo(_dto.categoria))
                .ReturnsAsync(null);

            var handler = new RegistrarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);

            await Assert.ThrowsAsync<FalloAlRegistrarProductoException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_SiProductoNoSeRegistra()
        {
            var command = new RegistrarProductoCommand(_dto);

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(_dto.correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerIdCategoriaMongo(_dto.categoria))
                .ReturnsAsync(_categoriaId);

            _mockProductoService.Setup(s => s.RegistrarProductoPostgreSQLAsync(It.IsAny<Producto>(), _categoriaId, _usuarioId))
                .ReturnsAsync(Guid.Empty);

            var handler = new RegistrarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);

            await Assert.ThrowsAsync<FalloAlRegistrarProductoException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcionGenerica_SiOcurreErrorInesperado()
        {
            var command = new RegistrarProductoCommand(_dto);

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(_dto.correo))
                .ThrowsAsync(new Exception("Error inesperado"));

            var handler = new RegistrarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);

            var ex = await Assert.ThrowsAsync<FalloAlRegistrarProductoException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Contains("Ha ocurrido un error al registrar el producto", ex.Message);
        }

    }
}
