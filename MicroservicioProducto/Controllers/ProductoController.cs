using Application.Command;
using Application.DTOs;
using Application.Querys;
using Domain.Entities;
using Domain.Factory;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicioProducto.Controllers
{

    [ApiController]
    [Route("api/Productos")]
    public class ProductoController: ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProductoRepositoryPostgreSQL _repository;

        public ProductoController(IMediator mediator, IProductoRepositoryPostgreSQL repository)
        {
            _mediator = mediator;
            _repository=repository;

        }

        [HttpPost("registroProducto")]
        public async Task<IActionResult> RegistrarProducto([FromBody] RegistrarProductoDTO productoDTO)
        {
            var resultado = await _mediator.Send(new RegistrarProductoCommand(productoDTO));
            return Ok(resultado);
        }

        [HttpPost("consultarProductos/{correo}")]
        public async Task<IActionResult> ConsultarProductos([FromRoute] string correo)
        {
            var resultado = await _mediator.Send(new ConsultarProductosQuery(correo));
            return Ok(resultado);
        }

        [HttpPut("modificarProducto/{correo}")]
        public async Task<IActionResult> ModificarProducto([FromRoute] string correo, ModificarProductoDTO productoModificarDto)
        {
            var resultado = await _mediator.Send(new ModificarProductoCommand(productoModificarDto, correo));
            return Ok(resultado);
        }

        [HttpDelete("eliminarProducto/{correo}")]
        public async Task<IActionResult> EliminarProducto([FromRoute] string correo, EliminarProductoDTO productEliminarDto)
        {
            var resultado = await _mediator.Send(new EliminarProductoCommand(productEliminarDto, correo));
            return Ok(resultado);
        }


        [HttpGet("consultarProducto/{idProducto}")]
        public async Task<IActionResult> ConsultarProducto([FromRoute] Guid idProducto)
        {
            var resultado = await _mediator.Send(new ConsultarProductoQuery(idProducto));
            return Ok(resultado);
        }

        [HttpGet("consultarIdUsuarioProducto/{idProducto}")]
        public async Task<IActionResult> ConsultarIdUsuarioProducto([FromRoute] Guid idProducto)
        {
            var resultado = await _mediator.Send(new ConsultarIdUsuarioProductoQuery(idProducto));
            return Ok(resultado);
        }



    }
}
