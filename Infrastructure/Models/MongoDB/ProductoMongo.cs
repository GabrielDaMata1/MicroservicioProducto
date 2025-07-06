using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Models.MongoDB
{
    /// <summary>
    /// Clase model que se encarga de definir la estructura del documento "Producto" en la base de datos en MongoDB
    /// </summary>
    public class ProductoMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        /// <summary>
        /// Atributo que corresponde al ID del producto en la base de datos en MongoDB 
        /// </summary>
        public Guid Id { get; set; }

        [BsonElement("Nombre")]
        /// <summary>
        /// Atributo que corresponde al nombre del producto en la base de datos en MongoDB 
        /// </summary>
        public string Nombre { get; set; }
        [BsonElement("Descripcion")]
        /// <summary>
        /// Atributo que corresponde a la descripcion del producto en la base de datos en MongoDB 
        /// </summary>
        public string Descripcion { get; set; }

        [BsonElement("CategoriaId")]
        /// <summary>
        /// Atributo que corresponde al ID de la categoria del producto en la base de datos en MongoDB 
        /// </summary>
        public int CategoriaId { get; set; }
        [BsonElement("PrecioBase")]
        /// <summary>
        /// Atributo que corresponde al precio base del producto en la base de datos en MongoDB 
        /// </summary>
        public decimal PrecioBase { get; set; }
        [BsonElement("ImagenURL")]
        /// <summary>
        /// Atributo que corresponde al URL de la imagen del producto en la base de datos en MongoDB 
        /// </summary>
        public string ImagenURL { get; set; }
        [BsonElement("Estado")]
        /// <summary>
        /// Atributo que corresponde al estado del producto en la base de datos en MongoDB 
        /// </summary>
        public string Estado { get; set; }

        [BsonElement("IdUsuario")]
        [BsonRepresentation(BsonType.String)]
        /// <summary>
        /// Atributo que corresponde ID del subastador al que le pertenece el producto en la base de datos en MongoDB 
        /// </summary>
        public Guid IdUsuario { get; set; }
    }
}
