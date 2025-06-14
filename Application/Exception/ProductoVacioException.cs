using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    public class ProductoVacioException: System.Exception
    {
    public ProductoVacioException() : base("Error, el producto proporcionado se encuentra vacío") { }
    }
}
