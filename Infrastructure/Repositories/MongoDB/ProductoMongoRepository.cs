using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exception;
using Application.Exceptions;
using Domain.Entities;
using Domain.Factory;
using Domain.Interfaces;
using Domain.Value_Object;
using Infrastructure.Mappers;
using Infrastructure.Models.MongoDB;
using MongoDB.Driver;

namespace Infrastructure.Repositories.MongoDB
{
    /// <summary>
    /// Clase repository que implementa las operaciones que se pueden realizar sobre los productos almacenados en MongoDB.
    /// </summary>
    public class ProductoMongoRepository : IProductoMongoRepository
    {
        /// <summary>
        /// Atributo que corresponde a la colección de productos en la base de datos en MongoDB.
        /// </summary>
        private readonly IMongoCollection<ProductoMongo> _productoCollection;
        /// <summary>
        /// Atributo que corresponde a la colección de categoria en la base de datos en MongoDB.
        /// </summary>
        private readonly IMongoCollection<CategoriaMongo> _categoriaCollection;


        public ProductoMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("MicroservicioProducto");
            _productoCollection = database.GetCollection<ProductoMongo>("Producto");
            _categoriaCollection= database.GetCollection<CategoriaMongo>("Categoria");
        }
        /// <summary>
        /// Método que se encarga de registrar un producto en MongoDB.
        /// </summary>
        /// <param name="producto">Entidad que contiene los valores del producto a registrar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="IdUsuario">Parametro que corresponde al ID del subastador que registra el producto</param>
        /// <returns>Retorna un estado HTTP exitoso si se registra el producto</returns>
        public async Task<HttpStatusCode> RegistrarProductoAsync(Producto producto, int idCategoria, Guid IdUsuario)
        {
            if (producto == null)
                throw new ProductoVacioException();
            try
            {

                _productoCollection.InsertOneAsync(producto.ToMongo(idCategoria, IdUsuario));
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar el producto en MongoDB: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Método que se encarga de modificar un producto de un subastador en MongoDB.
        /// </summary>
        /// <param name="producto">Entidad que contiene los valores del producto a modificar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="IdUsuario">Parametro que corresponde al ID del subastador que modificar el producto</param>
        /// <returns>Retorna un estado HTTP exitoso si se modifica el producto</returns>
        public async Task<HttpStatusCode> ModificarProductoAsync(Producto producto, int idCategoria, Guid IdUsuario)
        {
            var resultado = await _productoCollection.ReplaceOneAsync(
                u => u.Id == producto.Id, producto.ToMongo(idCategoria, IdUsuario));
            return HttpStatusCode.OK;
        }
        /// <summary>
        /// Método que se encarga de obtener el ID de una categoria en MongoDB.
        /// </summary>
        /// <param name="nombre">Parametro que corresponde el nombre de la categoria a consultar</param>
        /// <returns>Retorna un valor entero que corresponde al ID de la categoria consultada.</returns>
        public async Task<int> ObtenerIdCategoria(string nombre)
        {
            var categoria = await _categoriaCollection.Find(r => r.nombre== nombre).FirstOrDefaultAsync();

            return categoria.Id;
        }
        /// <summary>
        /// Método que se encarga de obtener los productos de un subastador en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parametro que corresponde al ID del subastador que consulta los producto</param>
        /// <returns>Retorna una lista de Productos con su detalle</returns>
        public async Task<List<Producto>> ObtenerProductosPorGuidAsync(Guid idUsuario)
        {
            var filtro = Builders<ProductoMongo>.Filter.Eq(h => h.IdUsuario, idUsuario);
            var productosMongo = await _productoCollection.Find(filtro).ToListAsync();

            var productosMap = new List<Producto>();

            foreach (var p in productosMongo)
            {
                var filtroCategoria = Builders<CategoriaMongo>.Filter.Eq(c => c.Id, p.CategoriaId);
                var categoria = await _categoriaCollection.Find(filtroCategoria).FirstOrDefaultAsync();

                var categoriaVO = categoria != null
                    ? new CategoriaProductoVO(categoria.nombre)
                    : new CategoriaProductoVO("Categoría desconocida");

                var producto = new Producto(
                    p.Id,
                    new NombreProductoVO(p.Nombre),
                    new DescripcionProductoVO(p.Descripcion),
                    new ImagenURLProductoVO(p.ImagenURL),
                    new PrecioBaseProductoVO(p.PrecioBase),
                    categoriaVO,
                    new EstadoProductoVO(p.Estado)
                );

                productosMap.Add(producto);
            }

            return productosMap;
        }
        /// <summary>
        /// Método que se encarga de eliminar el producto de un subastador en MongoDB.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a eliminar</param>
        /// <returns>Retorna un estado HTTP exitoso si se elimina el producto</returns>
        public async Task<bool> EliminarProductoAsync(Guid idProducto)
        {
            var filtro = Builders<ProductoMongo>.Filter.Eq(p => p.Id, idProducto);
            var resultado = await _productoCollection.DeleteOneAsync(filtro);

            return resultado.DeletedCount > 0;
        }

        /// <summary>
        /// Método que se encarga de obtener el producto de un subastador en MongoDB.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar</param>
        /// <returns>Retorna el Producto con su detalle</returns>
        public async Task<Producto> ObtenerProductoPorId(Guid idProducto)
        {
            var productoMongo = await _productoCollection.Find(r => r.Id == idProducto).FirstOrDefaultAsync();
            var filtroCategoria = Builders<CategoriaMongo>.Filter.Eq(c => c.Id, productoMongo.CategoriaId);
            var categoria = await _categoriaCollection.Find(filtroCategoria).FirstOrDefaultAsync();
            var productoEntidad = ProductoFactory.CrearProductoConIdYCategoria(productoMongo.Id, productoMongo.Nombre,
                productoMongo.Descripcion, productoMongo.ImagenURL, productoMongo.PrecioBase,categoria.nombre,productoMongo.Estado);

            return productoEntidad;
        }
        /// <summary>
        /// Método que se encarga de obtener el ID del subastador a quien le pertence el producto en MongoDB.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar</param>
        /// <returns>Retorna el ID del subastador a quien le pertenece el producto</returns>
        public async Task<Guid> ObtenerIdUsuarioPorProductoId(Guid idProducto)
        {
            var productoMongo = await _productoCollection.Find(r => r.Id == idProducto).FirstOrDefaultAsync();
            return productoMongo.IdUsuario;
        }

    }
}
