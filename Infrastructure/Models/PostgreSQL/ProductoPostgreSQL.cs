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
    public class ProductoPostgreSQL
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }

        [Required]
        public decimal PrecioBase { get; set; }
        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }
        public virtual CategoriaPostgreSQL Categoria { get; set; }

        [Required]
        public string ImagenURL { get; set; }
        [Required]
        public string Estado { get; set; }

        [Required]
        public Guid IdUsuario { get; set; }
    }
}
