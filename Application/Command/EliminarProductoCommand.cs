using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using MediatR;

namespace Application.Command
{
    public class EliminarProductoCommand : IRequest<bool>
    {
        public EliminarProductoDTO ProductoDto;
        public string correo;

        public EliminarProductoCommand(EliminarProductoDTO productoDto, string correo)
        {
            this.ProductoDto = productoDto;
            this.correo = correo;
        }
    }
}
