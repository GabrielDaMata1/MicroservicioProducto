using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Domain.Entities;
using MediatR;

namespace Application.Querys
{
    public class ConsultarProductoQuery: IRequest<HistorialProductosDTO>
    {
        public Guid IdProducto { get; set; }

        public ConsultarProductoQuery(Guid idProducto)
        {
            IdProducto = idProducto;
        }
    }
}
