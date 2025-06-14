using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    public class PermisoNoAutorizadoException: System.Exception
    {
        public PermisoNoAutorizadoException() { }

        public PermisoNoAutorizadoException(string message) : base(message) { }

    }
}
