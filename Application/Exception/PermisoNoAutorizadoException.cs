using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al intentar hacer una operación a la cual un usuario no tiene acceso o permisos.
    /// </summary>
    public class PermisoNoAutorizadoException: System.Exception
    {
        public PermisoNoAutorizadoException() { }

        public PermisoNoAutorizadoException(string message) : base(message) { }

    }
}
