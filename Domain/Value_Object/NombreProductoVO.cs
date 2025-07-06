using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization;

namespace Domain.Value_Object
{
    /// <summary>
    /// Clase value object que representa el nombre de un producto.
    /// </summary>
    public class NombreProductoVO
    {
        /// <summary>
        /// Atributo que corresponde al nombre de un producto.
        /// </summary>
        public string Nombre { get; set; }

        [JsonConstructor]
        public NombreProductoVO(string nombre)
        {
            Nombre = nombre;
        }
    }
}