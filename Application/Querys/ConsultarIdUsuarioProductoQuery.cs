using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Querys
{
    /// <summary>
    /// Clase Query que se encarga de enviar la solicitud para consultar el ID del usuario al que le pertenece un producto en específico .
    /// </summary>
    public class ConsultarIdUsuarioProductoQuery : IRequest<Guid>
    {
        /// <summary>
        /// Atributo que corresponde al ID del producto del subastador a consultar.
        /// </summary>
         public Guid IdProducto { get; set; }

        public ConsultarIdUsuarioProductoQuery(Guid idProducto)
        {
        IdProducto = idProducto;
        }
    }
}
