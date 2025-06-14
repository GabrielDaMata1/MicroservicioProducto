using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RegistrarProductoDTO
    { 
       public string nombreProducto { get; set; }
       public string descripcionProducto { get; set; }
       public string imagenURLProducto { get; set; }
       public decimal precioBase { get; set; }

       public string categoria { get; set; }

       public string correo { get; set; }
    }
}
