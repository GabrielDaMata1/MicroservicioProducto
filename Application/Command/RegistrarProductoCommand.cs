using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    public class RegistrarProductoCommand:IRequest<bool>
    {
        public RegistrarProductoDTO ProductoDto;

        public RegistrarProductoCommand(RegistrarProductoDTO productoDto)
        {
            this.ProductoDto = productoDto;
        }
    }
}
