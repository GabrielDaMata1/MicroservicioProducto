using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    /// <summary>
    /// Clase value object que representa el precio base de un producto.
    /// </summary>
    public class PrecioBaseProductoVO
    {
        /// <summary>
        /// Atributo que corresponde al precio base de un producto.
        /// </summary>
        public decimal precio { get; set; }

        public PrecioBaseProductoVO(decimal precio)
        {
            this.precio = precio;
        }
    }
}
