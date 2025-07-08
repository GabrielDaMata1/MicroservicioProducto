using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class ModificarProductoHandlerTest
    {
        private readonly Mock<IProductoService> _mockProductoService = new();
        private readonly Mock<IUsuarioService> _mockUsuarioService = new();
        private readonly Mock<IPublishEndpoint> _mockPublishEndpoint = new();

        private readonly Guid _productoId = Guid.NewGuid();
        private readonly Guid _usuarioId = Guid.NewGuid();
        private readonly ModificarProductoDTO _dto;

        public ModificarProductoHandlerTest()
        {
            _dto = new ModificarProductoDTO
            {
                Id = _productoId,
                NombreProducto = "Producto actualizado",
                DescripcionProducto = "Descripción actualizada",
                ImagenURLProducto = "https://img.com/actualizado.jpg",
                PrecioBaseProducto = 200,
                CategoriaProducto = "Electrónica",
                EstadoProducto = "Disponible"
            };
        }

        [Fact]
        public async Task Handle_DeberiaModificarProductoYPublicarEvento()
        {
            var command = new ModificarProductoCommand(_dto, "correo@ejemplo.com");
            var producto = ProductoFactory.CrearProductoConId(
                _dto.Id,
                _dto.NombreProducto,
                _dto.DescripcionProducto,
                _dto.ImagenURLProducto,
                _dto.PrecioBaseProducto,
                _dto.EstadoProducto
            );

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(command.correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerIdCategoriaMongo(_dto.CategoriaProducto))
                .ReturnsAsync(1);

            _mockProductoService.Setup(s => s.ModificarProductoPostgreSQL(It.Is<Producto>(p => p.Id == _dto.Id && p.NombreProducto.Nombre == _dto.NombreProducto &&
                        p.DescripcionProducto.descripcion == _dto.DescripcionProducto && p.ImagenURLProducto.url == _dto.ImagenURLProducto && p.PrecioBaseProducto.precio == _dto.PrecioBaseProducto &&
                        p.EstadoProducto.estadoProducto == _dto.EstadoProducto), _usuarioId, 1)).ReturnsAsync(HttpStatusCode.OK);


            var handler = new ModificarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.True(resultado);
            _mockPublishEndpoint.Verify(p => p.Publish(It.IsAny<ProductoModificadoEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_SiUsuarioNoExiste()
        {
            var command = new ModificarProductoCommand(_dto, "correo@ejemplo.com");

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(command.correo))
                .ReturnsAsync(Guid.Empty);

            var handler = new ModificarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);

            await Assert.ThrowsAsync<UsuarioNoEncontradoException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_SiModificacionFallaEnPostgreSQL()
        {
            var command = new ModificarProductoCommand(_dto, "correo@ejemplo.com");
            var producto = ProductoFactory.CrearProductoConId(
                _dto.Id,
                _dto.NombreProducto,
                _dto.DescripcionProducto,
                _dto.ImagenURLProducto,
                _dto.PrecioBaseProducto,
                _dto.EstadoProducto
            );

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(command.correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerIdCategoriaMongo(_dto.CategoriaProducto))
                .ReturnsAsync(1);

            _mockProductoService.Setup(s => s.ModificarProductoPostgreSQL(producto, _usuarioId, 1))
                .ReturnsAsync(HttpStatusCode.InternalServerError);

            var handler = new ModificarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);

            await Assert.ThrowsAsync<FalloAlModificarProductoException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcionGenerica_SiOcurreErrorInesperado()
        {
            var command = new ModificarProductoCommand(_dto, "correo@ejemplo.com");

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(command.correo))
                .ThrowsAsync(new Exception("Error inesperado"));

            var handler = new ModificarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);

            var ex = await Assert.ThrowsAsync<FalloAlModificarProductoException>(() => handler.Handle(command, CancellationToken.None));
            Assert.Contains("Ocurrió un error al modificar el producto", ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.Equal("Error inesperado", ex.InnerException.Message);
        }
    }

}
