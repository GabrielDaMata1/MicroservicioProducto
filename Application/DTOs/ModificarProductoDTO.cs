using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para modificar un producto de un subastador.
    /// </summary>
    public class ModificarProductoDTO
    {
        /// <summary>
        /// Atributo que corresponde al Id del producto a modificar.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre del producto a modificar.
        /// </summary>
        public string NombreProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripción del producto a modificar.
        /// </summary>
        public string DescripcionProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la direccion URL del producto a modificar alojada en Firebase.
        /// </summary>
        public string ImagenURLProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio base del producto a modificar.
        /// </summary>
        public decimal PrecioBaseProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la categoria del producto a modificar.
        /// </summary>
        public string CategoriaProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado del producto a modificar (Disponible, Subastando, Subastado).
        /// </summary>
        public string EstadoProducto { get; set; }
    }
}
