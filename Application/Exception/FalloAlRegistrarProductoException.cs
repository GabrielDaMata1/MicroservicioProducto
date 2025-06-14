using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    public class FalloAlRegistrarProductoException: System.Exception
    {
        public FalloAlRegistrarProductoException() : base("Ha ocurrido un error al obtener el producto.") { }

        public FalloAlRegistrarProductoException(string mensaje) : base(mensaje) { }

        public FalloAlRegistrarProductoException(string mensaje, System.Exception innerException)
            : base(mensaje, innerException) { }
    }
}
