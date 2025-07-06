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
    /// Clase Command que se encarga de enviar la solicitud de querer registrar un producto nuevo.
    /// </summary>
    public class RegistrarProductoCommand:IRequest<bool>
    {
        /// <summary>
        /// Atributo DTO que se encarga de recibir la información del producto a registrar.
        /// </summary>
        public RegistrarProductoDTO ProductoDto;

        public RegistrarProductoCommand(RegistrarProductoDTO productoDto)
        {
            this.ProductoDto = productoDto;
        }
    }
}
