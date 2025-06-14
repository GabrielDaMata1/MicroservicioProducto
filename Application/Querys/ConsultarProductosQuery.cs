using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Handler;
using Domain.Entities;
using MediatR;

namespace Application.Querys
{
    public class ConsultarProductosQuery : IRequest<List<HistorialProductosDTO>>
    {
        public string correo { get; set; }

        public ConsultarProductosQuery(string correo)
        {
            this.correo = correo;
        }
    }
}
