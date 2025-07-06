using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Application.DTOs
{
    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para mostrar los productos de un subastador.
    /// </summary>
    public class HistorialProductosDTO
    {
        /// <summary>
        /// Atributo que corresponde al Id del producto a mostrar.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Atributo que corresponde al nombre del producto a mostrar.
        /// </summary>
        public string NombreProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la descripcion del producto a mostrar.
        /// </summary>
        public string DescripcionProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la dirección url del producto a mostrar alojada en Firebase.
        /// </summary>
        public string ImagenURLProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al precio base del producto a mostrar.
        /// </summary>
        public decimal PrecioBaseProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde a la categoria del producto a mostrar.
        /// </summary>
        public string CategoriaProducto { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado del producto a mostrar (Disponible, Subastando, Subastado).
        /// </summary>
        public string EstadoProducto { get; set; }
        public HistorialProductosDTO (Guid id,string nombreProducto, string descripcionProducto, string imagenUrl, decimal  precioBaseProducto, string categoria, string estadoProducto)
        {
            Id = id;
            NombreProducto= nombreProducto;
            DescripcionProducto= descripcionProducto;
            ImagenURLProducto= imagenUrl;
            PrecioBaseProducto= precioBaseProducto;
            CategoriaProducto= categoria;
            EstadoProducto= estadoProducto;
        }
    }
}
