using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    /// <summary>
    /// Clase value object que representa el estado de un producto.
    /// </summary>
    public class EstadoProductoVO
    {
        /// <summary>
        /// Atributo que corresponde al estado de un producto.
        /// </summary>
        public string estadoProducto { get; set; }

        public EstadoProductoVO(string estadoProducto)
        {
            this.estadoProducto=estadoProducto;
        }
    }
}
