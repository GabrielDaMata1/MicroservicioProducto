using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.PostgreSQL
{
    /// <summary>
    /// Clase model que se encarga de definir la estructura de la entidad "Categoria" en la base de datos en PostgreSQL
    /// </summary>
    public class CategoriaPostgreSQL
    {
        [Key]
        /// <summary>
        /// Atributo que corresponde al ID de la categoria en la base de datos en PostgreSQL 
        /// </summary>
        public int Id { get; set; }

        [Required]
        /// <summary>
        /// Atributo que corresponde al nombre de la categoria en la base de datos en PostgreSQL 
        /// </summary>
        public string Nombre { get; set; }
    }
}
