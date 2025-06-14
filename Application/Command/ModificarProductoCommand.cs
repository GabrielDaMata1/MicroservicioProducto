using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    public class ModificarProductoCommand : IRequest<bool>
    {
        public ModificarProductoDTO ProductoDto;
        public string correo;

        public ModificarProductoCommand(ModificarProductoDTO productoDto, string correo)
        {
            this.ProductoDto = productoDto;
            this.correo = correo;
        }
    }
}
