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
    public class ProductoMongoRepository : IProductoMongoRepository
    {
        private readonly IMongoCollection<ProductoMongo> _productoCollection;
        private readonly IMongoCollection<CategoriaMongo> _categoriaCollection;


        public ProductoMongoRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("MicroservicioProducto");
            _productoCollection = database.GetCollection<ProductoMongo>("Producto");
            _categoriaCollection= database.GetCollection<CategoriaMongo>("Categoria");
        }

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

        public async Task<HttpStatusCode> ModificarProductoAsync(Producto producto, int idCategoria, Guid IdUsuario)
        {
            var resultado = await _productoCollection.ReplaceOneAsync(
                u => u.Id == producto.Id, producto.ToMongo(idCategoria, IdUsuario));
            return HttpStatusCode.OK;
        }

        public async Task<int> ObtenerIdCategoria(string nombre)
        {
            var categoria = await _categoriaCollection.Find(r => r.nombre== nombre).FirstOrDefaultAsync();

            return categoria.Id;
        }

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
        public async Task<bool> EliminarProductoAsync(Guid idProducto)
        {
            var filtro = Builders<ProductoMongo>.Filter.Eq(p => p.Id, idProducto);
            var resultado = await _productoCollection.DeleteOneAsync(filtro);

            return resultado.DeletedCount > 0;
        }


        public async Task<Producto> ObtenerProductoPorId(Guid idProducto)
        {
            var productoMongo = await _productoCollection.Find(r => r.Id == idProducto).FirstOrDefaultAsync();
            var filtroCategoria = Builders<CategoriaMongo>.Filter.Eq(c => c.Id, productoMongo.CategoriaId);
            var categoria = await _categoriaCollection.Find(filtroCategoria).FirstOrDefaultAsync();
            var productoEntidad = ProductoFactory.CrearProductoConIdYCategoria(productoMongo.Id, productoMongo.Nombre,
                productoMongo.Descripcion, productoMongo.ImagenURL, productoMongo.PrecioBase,categoria.nombre,productoMongo.Estado);

            return productoEntidad;
        }

        public async Task<Guid> ObtenerIdUsuarioPorProductoId(Guid idProducto)
        {
            var productoMongo = await _productoCollection.Find(r => r.Id == idProducto).FirstOrDefaultAsync();
            return productoMongo.IdUsuario;
        }

    }
}
