using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al eliminar un producto en las bases de datos (PostgreSQL, MongoDB).
    /// </summary>
    public class FalloAlEliminarProductoException: System.Exception
    {
        public FalloAlEliminarProductoException() : base("Ha ocurrido un error al eliminar el producto.") { }

        public FalloAlEliminarProductoException(string mensaje) : base(mensaje) { }

        public FalloAlEliminarProductoException(string mensaje, System.Exception innerException)
            : base(mensaje, innerException) { }
    }
}
