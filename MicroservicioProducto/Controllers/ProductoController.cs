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
    /// <summary>
    /// Clase controller API encargada de procesar las solicitudes de inserción, eliminación, modificación y consulta,
    /// sobre los productos.
    /// </summary>

    [ApiController]
    [Route("api/Productos")]
    public class ProductoController: ControllerBase
    {
        /// <summary>
        /// Atributo que se encarga de enviar solicitudes (commands/queries) mediante el patrón mediador
        /// </summary>
        private readonly IMediator _mediator;

        public ProductoController(IMediator mediator)
        {
            _mediator = mediator;

        }
        /// <summary>
        /// Endpoint encargado de registrar un nuevo producto.
        /// </summary>
        /// <param name="productoDTO">Parametro de tipo DTO con los datos del producto a registrar.</param>
        /// <returns>Resultado de la operación con mensaje y estado dependiendo del resultado.</returns>

        [HttpPost("registroProducto")]
        public async Task<IActionResult> RegistrarProducto([FromBody] RegistrarProductoDTO productoDTO)
        {
            var resultado = await _mediator.Send(new RegistrarProductoCommand(productoDTO));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "El producto se registró exitosamente.", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "El producto no pudo ser registrado.", Exito = false });
        }
        /// <summary>
        /// Endpoint encargado de consultar los productos de un subastador.
        /// </summary>
        /// <param name="correo">Parametro que corresponde al correo del subastador.</param>
        /// <returns>Retorna una lista de objetos Producto con su detalle.</returns>

        [HttpPost("consultarProductos/{correo}")]
        public async Task<IActionResult> ConsultarProductos([FromRoute] string correo)
        {
            var resultado = await _mediator.Send(new ConsultarProductosQuery(correo));
            return Ok(resultado);
        }
        /// <summary>
        /// Endpoint encargado de modificar un producto.
        /// </summary>
        /// <param name="correo">Parametro que corresponde al correo del subastador.</param>
        /// <param name="productoModificarDto">Parametro de tipo DTO con los datos del producto a modificar.</param>
        /// <returns>Resultado de la operación con mensaje y estado dependiendo del resultado.</returns>
        [HttpPut("modificarProducto/{correo}")]
        public async Task<IActionResult> ModificarProducto([FromRoute] string correo, ModificarProductoDTO productoModificarDto)
        {
            var resultado = await _mediator.Send(new ModificarProductoCommand(productoModificarDto, correo));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "El producto se modificó exitosamente.", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "El producto no pudo ser modificado.", Exito = false });
        }
        /// <summary>
        /// Endpoint encargado de eliminar un producto de un subastador.
        /// </summary>
        /// <param name="correo">Parametro que corresponde al correo del subastador.</param>
        /// <param name="productEliminarDto">Parametro de tipo DTO con los datos del producto a eliminar.</param>
        /// <returns>Resultado de la operación con mensaje y estado dependiendo del resultado.</returns>

        [HttpDelete("eliminarProducto/{correo}")]
        public async Task<IActionResult> EliminarProducto([FromRoute] string correo, EliminarProductoDTO productEliminarDto)
        {
            var resultado = await _mediator.Send(new EliminarProductoCommand(productEliminarDto, correo));
            if (resultado)
            {
                return Ok(new ResultadoDTO { Mensaje = "El producto se eliminó exitosamente.", Exito = true });
            }

            return BadRequest(new ResultadoDTO { Mensaje = "El producto no pudo ser eliminado.", Exito = false });
        }
        /// <summary>
        /// Endpoint encargado de consultar un producto de un subastador.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar.</param>
        /// <returns>Retorna un objeto Producto con su detalle.</returns>

        [HttpGet("consultarProducto/{idProducto}")]
        public async Task<IActionResult> ConsultarProducto([FromRoute] Guid idProducto)
        {
            var resultado = await _mediator.Send(new ConsultarProductoQuery(idProducto));
            return Ok(resultado);
        }

        /// <summary>
        /// Endpoint encargado de consultar el ID del subastador a quien le pertenece el producto dado.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar.</param>
        /// <returns>Retorna el ID del subastador al que le pertenece el producto</returns>

        [HttpGet("consultarIdUsuarioProducto/{idProducto}")]
        public async Task<IActionResult> ConsultarIdUsuarioProducto([FromRoute] Guid idProducto)
        {
            var resultado = await _mediator.Send(new ConsultarIdUsuarioProductoQuery(idProducto));
            return Ok(resultado);
        }



    }
}
