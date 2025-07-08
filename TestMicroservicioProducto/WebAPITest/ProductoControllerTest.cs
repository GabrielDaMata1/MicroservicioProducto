using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Querys;
using MediatR;
using MicroservicioProducto.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestMicroservicioProducto.WebAPITest
{
    public class ProductoControllerTest
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly ProductoController _controller;

        public ProductoControllerTest()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new ProductoController(_mockMediator.Object);
        }

        [Fact]
        public async Task RegistrarProducto_DeberiaRetornarOk_SiRegistroExitoso()
        {
            var dto = new RegistrarProductoDTO();
            _mockMediator.Setup(m => m.Send(It.IsAny<RegistrarProductoCommand>(), default))
                         .ReturnsAsync(true);

            var result = await _controller.RegistrarProducto(dto) as OkObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResultadoDTO>(result.Value);
            Assert.True(response.Exito);
        }

        [Fact]
        public async Task RegistrarProducto_DeberiaRetornarBadRequest_SiFalla()
        {
            var dto = new RegistrarProductoDTO();
            _mockMediator.Setup(m => m.Send(It.IsAny<RegistrarProductoCommand>(), default))
                         .ReturnsAsync(false);

            var result = await _controller.RegistrarProducto(dto) as BadRequestObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResultadoDTO>(result.Value);
            Assert.False(response.Exito);
        }

        [Fact]
        public async Task ConsultarProductos_DeberiaRetornarOkConLista()
        {
            var correo = "usuario@test.com";
            var productos = new List<HistorialProductosDTO> {    {
                new HistorialProductosDTO(
                    Guid.NewGuid(),
                    "Producto de prueba",
                    "Descripción",
                    "https://img.com",
                    100,
                    "Tecnología",
                    "Disponible"
                )
            } };

            _mockMediator.Setup(m => m.Send(It.IsAny<ConsultarProductosQuery>(), default))
                         .ReturnsAsync(productos);

            var result = await _controller.ConsultarProductos(correo) as OkObjectResult;

            Assert.NotNull(result);
            Assert.IsType<List<HistorialProductosDTO>>(result.Value);
        }

        [Fact]
        public async Task ModificarProducto_DeberiaRetornarOk_SiModifica()
        {
            var dto = new ModificarProductoDTO();
            var correo = "usuario@test.com";

            _mockMediator.Setup(m => m.Send(It.IsAny<ModificarProductoCommand>(), default))
                         .ReturnsAsync(true);

            var result = await _controller.ModificarProducto(correo, dto) as OkObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResultadoDTO>(result.Value);
            Assert.True(response.Exito);
        }

        [Fact]
        public async Task ModificarProducto_DeberiaRetornarBadRequest_SiFalla()
        {
            var dto = new ModificarProductoDTO();
            var correo = "usuario@test.com";

            _mockMediator.Setup(m => m.Send(It.IsAny<ModificarProductoCommand>(), default))
                         .ReturnsAsync(false);

            var result = await _controller.ModificarProducto(correo, dto) as BadRequestObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResultadoDTO>(result.Value);
            Assert.False(response.Exito);
        }

        [Fact]
        public async Task EliminarProducto_DeberiaRetornarOk_SiElimina()
        {
            var dto = new EliminarProductoDTO();
            var correo = "usuario@test.com";

            _mockMediator.Setup(m => m.Send(It.IsAny<EliminarProductoCommand>(), default))
                         .ReturnsAsync(true);

            var result = await _controller.EliminarProducto(correo, dto) as OkObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResultadoDTO>(result.Value);
            Assert.True(response.Exito);
        }

        [Fact]
        public async Task EliminarProducto_DeberiaRetornarBadRequest_SiFalla()
        {
            var dto = new EliminarProductoDTO();
            var correo = "usuario@test.com";

            _mockMediator.Setup(m => m.Send(It.IsAny<EliminarProductoCommand>(), default))
                         .ReturnsAsync(false);

            var result = await _controller.EliminarProducto(correo, dto) as BadRequestObjectResult;

            Assert.NotNull(result);
            var response = Assert.IsType<ResultadoDTO>(result.Value);
            Assert.False(response.Exito);
        }

        [Fact]
        public async Task ConsultarProducto_DeberiaRetornarOkConProducto()
        {
            var id = Guid.NewGuid();
            var producto = new HistorialProductosDTO(
                    Guid.NewGuid(),
                    "Producto de prueba",
                    "Descripción",
                    "https://img.com",
                    100,
                    "Tecnología",
                    "Disponible"
                );
            

            _mockMediator.Setup(m => m.Send(It.IsAny<ConsultarProductoQuery>(), default))
                         .ReturnsAsync(producto);

            var result = await _controller.ConsultarProducto(id) as OkObjectResult;

            Assert.NotNull(result);
            Assert.IsType<HistorialProductosDTO>(result.Value);
        }

        [Fact]
        public async Task ConsultarIdUsuarioProducto_DeberiaRetornarOkConGuid()
        {
            var id = Guid.NewGuid();
            var idUsuario = Guid.NewGuid();

            _mockMediator.Setup(m => m.Send(It.IsAny<ConsultarIdUsuarioProductoQuery>(), default))
                         .ReturnsAsync(idUsuario);

            var result = await _controller.ConsultarIdUsuarioProducto(id) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(idUsuario, result.Value);
        }

    }
}
