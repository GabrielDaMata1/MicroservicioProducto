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
    /// <summary>
    /// Clase Query que se encarga de enviar la solicitud para consultar el un producto específico .
    /// </summary>
    public class ConsultarProductoQuery: IRequest<HistorialProductosDTO>
    {
        /// <summary>
        /// Atributo que corresponde al ID del producto a consultar.
        /// </summary>
        public Guid IdProducto { get; set; }

        public ConsultarProductoQuery(Guid idProducto)
        {
            IdProducto = idProducto;
        }
    }
}
