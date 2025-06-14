using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    public class ProductoNoEncontradoException : System.Exception
    {
        public ProductoNoEncontradoException() : base("Error, el producto proporcionado no se encontró") { }
    }
}
