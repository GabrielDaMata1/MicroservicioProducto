using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    public class FalloAlEliminarProductoException: System.Exception
    {
        public FalloAlEliminarProductoException() : base("Ha ocurrido un error al eliminar el producto.") { }

        public FalloAlEliminarProductoException(string mensaje) : base(mensaje) { }

        public FalloAlEliminarProductoException(string mensaje, System.Exception innerException)
            : base(mensaje, innerException) { }
    }
}
