using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exception
{
    /// <summary>
    /// Clase Exception que se encarga de manejar los errores producidos al intentar obtener un producto en la bases de datos en MongoDB.
    /// </summary>
    public class ProductoVacioException: System.Exception
    {
    public ProductoVacioException() : base("Error, el producto proporcionado se encuentra vacío") { }
    }
}
