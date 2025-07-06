using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al registrar un producto en las bases de datos (PostgreSQL, MongoDB).
    /// </summary>
    public class FalloAlRegistrarProductoException: System.Exception
    {
        public FalloAlRegistrarProductoException() : base("Ha ocurrido un error al obtener el producto.") { }

        public FalloAlRegistrarProductoException(string mensaje) : base(mensaje) { }

        public FalloAlRegistrarProductoException(string mensaje, System.Exception innerException)
            : base(mensaje, innerException) { }
    }
}
