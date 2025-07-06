using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    /// <summary>
    /// Clase Command que se encarga de enviar la solicitud de querer eliminaar un producto registrado previamente.
    /// </summary>
    public class EliminarProductoCommand : IRequest<bool>
    {
        /// <summary>
        /// Atributo DTO que se encarga de recibir la información del producto a eliminar.
        /// </summary>
        public EliminarProductoDTO ProductoDto;
        /// <summary>
        /// Atributo que corresponde al correo del subastador al que le pertenece el producto a eliminar.
        /// </summary>
        public string correo;

        public EliminarProductoCommand(EliminarProductoDTO productoDto, string correo)
        {
            this.ProductoDto = productoDto;
            this.correo = correo;
        }
    }
}
