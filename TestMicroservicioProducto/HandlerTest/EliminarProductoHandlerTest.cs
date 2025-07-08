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
using Domain.Interfaces;
using Domain.Value_Object;
using MassTransit;
using Moq;

namespace TestMicroservicioProducto.HandlerTest
{
    public class EliminarProductoHandlerTest
    {
        private readonly Mock<IProductoService> _mockProductoService = new();
        private readonly Mock<IUsuarioService> _mockUsuarioService = new();
        private readonly Mock<IPublishEndpoint> _mockPublishEndpoint = new();

        private readonly Guid _productoId = Guid.NewGuid();
        private readonly Guid _usuarioId = Guid.NewGuid();
        private readonly EliminarProductoDTO _dto;

        public EliminarProductoHandlerTest()
        {
            _dto = new EliminarProductoDTO { idProducto = _productoId };
        }

        [Fact]
        public async Task Handle_DeberiaEliminarProductoYPublicarEvento()
        {
            var handler = new EliminarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);
            var command = new EliminarProductoCommand(_dto, "correo@ejemplo.com");

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(command.correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerProductoPorIdMongo(_productoId))
                .ReturnsAsync(new Producto(
                    id: _productoId,
                    nombreProducto: new NombreProductoVO("Producto de prueba"),
                    descripcionProducto: new DescripcionProductoVO("Descripción de prueba"),
                    imagenUrlProducto: new ImagenURLProductoVO("https://imagen.com/producto.jpg"),
                    precioBaseProducto: new PrecioBaseProductoVO(100.00m),
                    estadoProducto: new EstadoProductoVO("Disponible")
                ));


            _mockProductoService.Setup(s => s.ObtenerIdUsuarioPorProductoIdMongo(_productoId))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.EliminarProductoPostgreSQLAsync(_productoId))
                .ReturnsAsync(HttpStatusCode.OK);

            var resultado = await handler.Handle(command, CancellationToken.None);

            Assert.True(resultado);
            _mockPublishEndpoint.Verify(p => p.Publish(It.IsAny<ProductoEliminadoEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_SiProductoNoEstaDisponible()
        {
            var handler = new EliminarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);
            var command = new EliminarProductoCommand(_dto, "correo@ejemplo.com");

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(command.correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerProductoPorIdMongo(_productoId))
                .ReturnsAsync(new Producto(
                    id: _productoId,
                    nombreProducto: new NombreProductoVO("Producto de prueba"),
                    descripcionProducto: new DescripcionProductoVO("Descripción de prueba"),
                    imagenUrlProducto: new ImagenURLProductoVO("https://imagen.com/producto.jpg"),
                    precioBaseProducto: new PrecioBaseProductoVO(100.00m),
                    estadoProducto: new EstadoProductoVO("Disponible")
                ));


            await Assert.ThrowsAsync<PermisoNoAutorizadoException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_SiProductoNoPerteneceAlUsuario()
        {
            var handler = new EliminarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);
            var command = new EliminarProductoCommand(_dto, "correo@ejemplo.com");

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(command.correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerProductoPorIdMongo(_productoId))
                .ReturnsAsync(new Producto(
                    id: _productoId,
                    nombreProducto: new NombreProductoVO("Producto de prueba"),
                    descripcionProducto: new DescripcionProductoVO("Descripción de prueba"),
                    imagenUrlProducto: new ImagenURLProductoVO("https://imagen.com/producto.jpg"),
                    precioBaseProducto: new PrecioBaseProductoVO(100.00m),
                    estadoProducto: new EstadoProductoVO("Disponible")
                ));


            _mockProductoService.Setup(s => s.ObtenerIdUsuarioPorProductoIdMongo(_productoId))
                .ReturnsAsync(Guid.NewGuid());

            await Assert.ThrowsAsync<PermisoNoAutorizadoException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_DeberiaLanzarExcepcion_SiFallaEliminacionPostgreSQL()
        {
            var handler = new EliminarProductoHandler(_mockProductoService.Object, _mockPublishEndpoint.Object, _mockUsuarioService.Object);
            var command = new EliminarProductoCommand(_dto, "correo@ejemplo.com");

            _mockUsuarioService.Setup(s => s.ObtenerUsuarioPorIdAsync(command.correo))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.ObtenerProductoPorIdMongo(_productoId))
                .ReturnsAsync(new Producto(
                    id: _productoId,
                    nombreProducto: new NombreProductoVO("Producto de prueba"),
                    descripcionProducto: new DescripcionProductoVO("Descripción de prueba"),
                    imagenUrlProducto: new ImagenURLProductoVO("https://imagen.com/producto.jpg"),
                    precioBaseProducto: new PrecioBaseProductoVO(100.00m),
                    estadoProducto: new EstadoProductoVO("Disponible")
                ));


            _mockProductoService.Setup(s => s.ObtenerIdUsuarioPorProductoIdMongo(_productoId))
                .ReturnsAsync(_usuarioId);

            _mockProductoService.Setup(s => s.EliminarProductoPostgreSQLAsync(_productoId))
                .ReturnsAsync(HttpStatusCode.InternalServerError);

            await Assert.ThrowsAsync<FalloAlEliminarProductoException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
