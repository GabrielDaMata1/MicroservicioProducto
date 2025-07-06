using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para registrar un producto de un subastador.
    /// </summary>
    public class RegistrarProductoDTO
    {
       /// <summary>
       /// Atributo que corresponde al nombre del producto a registrar.
       /// </summary>
       public string nombreProducto { get; set; }
       /// <summary>
       /// Atributo que corresponde a la descripción del producto a registrar.
       /// </summary>
       public string descripcionProducto { get; set; }
       /// <summary>
       /// Atributo que corresponde a la direccion URL del producto a registrar alojada en Firebase.
       /// </summary>
       public string imagenURLProducto { get; set; }
       /// <summary>
       /// Atributo que corresponde al precio base de un producto a registrar.
       /// </summary>
       public decimal precioBase { get; set; }
       /// <summary>
       /// Atributo que corresponde a la categoria del producto a registrar.
       /// </summary>
       public string categoria { get; set; }
       /// <summary>
       /// Atributo que corresponde al correo del subastador quien registra el producto.
       /// </summary>
       public string correo { get; set; }
    }
}
