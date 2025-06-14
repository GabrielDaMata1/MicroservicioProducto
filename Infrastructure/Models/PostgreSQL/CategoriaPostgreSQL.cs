using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.PostgreSQL
{
    public class CategoriaPostgreSQL
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
    }
}
