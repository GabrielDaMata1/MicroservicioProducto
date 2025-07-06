using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{   /// <summary>
    /// Clase Command que se encarga de enviar la solicitud de querer modificar un producto registrado previamente.
    /// </summary>
    public class ModificarProductoCommand : IRequest<bool>
    {
        /// <summary>
        /// Atributo DTO que se encarga de recibir la información del producto a modificar.
        /// </summary>
        public ModificarProductoDTO ProductoDto;
        /// <summary>
        /// Atributo que corresponde al correo del subastador al que le pertenece el producto a modificar.
        /// </summary>
        public string correo;

        public ModificarProductoCommand(ModificarProductoDTO productoDto, string correo)
        {
            this.ProductoDto = productoDto;
            this.correo = correo;
        }
    }
}
