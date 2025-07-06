using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.Models.MongoDB
{
    /// <summary>
    /// Clase model que se encarga de definir la estructura del documento "Categoria" en la base de datos en MongoDB
    /// </summary>
    public class CategoriaMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        /// <summary>
        /// Atributo que corresponde al ID de la categoria en la base de datos en MongoDB 
        /// </summary>
        public int Id { get; set; }

        [BsonElement("nombre")]
        /// <summary>
        /// Atributo que corresponde al nombre de la categoria en la base de datos en MongoDB 
        /// </summary>
        public string nombre { get; set; }
    }
}
