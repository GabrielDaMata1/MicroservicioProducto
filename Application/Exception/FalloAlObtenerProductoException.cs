using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    public class FalloAlObtenerProductoException : System.Exception
    {
        public FalloAlObtenerProductoException() : base("Ha ocurrido un error al obtener el producto.") { }

        public FalloAlObtenerProductoException(string mensaje) : base(mensaje) { }

        public FalloAlObtenerProductoException(string mensaje, System.Exception innerException)
            : base(mensaje, innerException) { }
    }

}
