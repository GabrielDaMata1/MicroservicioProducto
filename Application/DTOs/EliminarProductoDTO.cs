using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para eliminar un producto registrado previamente.
    /// </summary>
    public class EliminarProductoDTO
    {
        /// <summary>
        /// Atributo que corresponde al Id del producto a eliminar.
        /// </summary>
        public Guid idProducto { get; set; }
    }
}
