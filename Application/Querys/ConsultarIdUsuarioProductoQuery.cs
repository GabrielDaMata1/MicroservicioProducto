using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Querys
{
    public class ConsultarIdUsuarioProductoQuery : IRequest<Guid>
    {
    public Guid IdProducto { get; set; }

    public ConsultarIdUsuarioProductoQuery(Guid idProducto)
    {
        IdProducto = idProducto;
    }
}
}
