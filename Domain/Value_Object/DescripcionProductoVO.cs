using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    /// <summary>
    /// Clase value object que representa la descripcion de un producto.
    /// </summary>
    public class DescripcionProductoVO
    {
        /// <summary>
        /// Atributo que corresponde a la descripcion de un producto.
        /// </summary>
        public string descripcion { get; set; }

        public DescripcionProductoVO(string descripcion)
        {
            this.descripcion = descripcion;
        }
    }
}
