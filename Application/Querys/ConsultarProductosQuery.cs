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
    /// <summary>
    /// Clase Query que se encarga de enviar la solicitud para consultar los productos de un subastador .
    /// </summary>
    public class ConsultarProductosQuery : IRequest<List<HistorialProductosDTO>>
    {
        /// <summary>
        /// Atributo que corresponde al correo del subastador al que le pertenecen los productos a consultar.
        /// </summary>
        public string correo { get; set; }

        public ConsultarProductosQuery(string correo)
        {
            this.correo = correo;
        }
    }
}
