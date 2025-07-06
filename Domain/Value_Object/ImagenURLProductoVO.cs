using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Value_Object
{
    /// <summary>
    /// Clase value object que representa el url de la imagen de un producto.
    /// </summary>
    public class ImagenURLProductoVO
    {
        /// <summary>
        /// Atributo que corresponde a la url de la imaagen de un producto.
        /// </summary>
        public string url { get; set; }
        public ImagenURLProductoVO(string url)
        {
            this.url = url;
        }
    }
}
