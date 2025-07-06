using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.PostgreSQL
{
    /// <summary>
    /// Clase model que se encarga de definir la estructura de la entidad "Producto" en la base de datos en PostgreSQL
    /// </summary>
    public class ProductoPostgreSQL
    {
        [Key]
        /// <summary>
        /// Atributo que corresponde al ID del producto en la base de datos en PostgreSQL 
        /// </summary>
        public Guid Id { get; set; }

        [Required]
        /// <summary>
        /// Atributo que corresponde al nombre del producto en la base de datos en PostgreSQL 
        /// </summary>
        public string Nombre { get; set; }
        [Required]
        /// <summary>
        /// Atributo que corresponde a la descripcion del producto en la base de datos en PostgreSQL 
        /// </summary>
        public string Descripcion { get; set; }

        [Required]
        /// <summary>
        /// Atributo que corresponde al precio base del producto en la base de datos en PostgreSQL 
        /// </summary>
        public decimal PrecioBase { get; set; }
        /// <summary>
        /// Atributo que corresponde al ID de la categoria del producto en la base de datos en PostgreSQL 
        /// </summary>
        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }
        public virtual CategoriaPostgreSQL Categoria { get; set; }
        /// <summary>
        /// Atributo que corresponde al URL de la imagen del producto en la base de datos en PostgreSQL 
        /// </summary>
        [Required]
        public string ImagenURL { get; set; }
        /// <summary>
        /// Atributo que corresponde al estado del producto en la base de datos en PostgreSQL 
        /// </summary>
        [Required]
        public string Estado { get; set; }
        /// <summary>
        /// Atributo que corresponde ID del subastador al que le pertenece el producto en la base de datos en PostgreSQL 
        /// </summary>
        [Required]
        public Guid IdUsuario { get; set; }
    }
}
