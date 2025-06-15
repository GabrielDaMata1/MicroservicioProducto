using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models.MongoDB
{
    public class ProductoMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("Nombre")]
        public string Nombre { get; set; }
        [BsonElement("Descripcion")]
        public string Descripcion { get; set; }

        [BsonElement("CategoriaId")]
        public int CategoriaId { get; set; }
        [BsonElement("PrecioBase")]
        public decimal PrecioBase { get; set; }
        [BsonElement("ImagenURL")]
        public string ImagenURL { get; set; }
        [BsonElement("Estado")]
        public string Estado { get; set; }

        [BsonElement("IdUsuario")]
        [BsonRepresentation(BsonType.String)]
        public Guid IdUsuario { get; set; }
    }
}
