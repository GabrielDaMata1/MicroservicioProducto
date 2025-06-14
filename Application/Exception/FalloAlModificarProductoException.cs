using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    public class FalloAlModificarProductoException: System.Exception
    {
        public FalloAlModificarProductoException() : base("Ha ocurrido un error al modificar el producto.") { }

        public FalloAlModificarProductoException(string mensaje) : base(mensaje) { }

        public FalloAlModificarProductoException(string mensaje, System.Exception innerException)
            : base(mensaje, innerException) { }
    }
}
