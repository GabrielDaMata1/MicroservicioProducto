using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services
{
    public class ProductoService : IProductoService

    {
        private readonly IProductoMongoRepository _productoMongoRepository;
        private readonly IProductoRepositoryPostgreSQL _productoPostgreSQLRepository;


        public ProductoService(IProductoMongoRepository productoMongoRepository, IProductoRepositoryPostgreSQL productoPostgreSQLRepository)

        {
            _productoMongoRepository = productoMongoRepository;
            _productoPostgreSQLRepository = productoPostgreSQLRepository;

        }
        public async Task<int> ObtenerIdCategoriaMongo(string nombre)
        {
            try
            {
                var resul = await _productoMongoRepository.ObtenerIdCategoria(nombre);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener el id de la categoria {ex.Message}", ex);
            }
        }

        public async Task<Guid> RegistrarProductoPostgreSQLAsync(Producto producto, int idCategoria, Guid IdUsuario)
        {
            try
            {
                var resul = await _productoPostgreSQLRepository.RegistrarProductoAsync(producto, idCategoria, IdUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar registrar los productos en PostgreSQL: {ex.Message}", ex);
            }
        }

        public async Task<HttpStatusCode> RegistrarProductoMongoAsync(Producto producto, int idCategoria, Guid IdUsuario)
        {
            try
            {
                var resul = await _productoMongoRepository.RegistrarProductoAsync(producto, idCategoria, IdUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar registrar los productos en MongoDB: {ex.Message}", ex);
            }
        }

        public async Task<List<Producto>> ObtenerProductosPorGuidMongoAsync(Guid idUsuario)
        {
            try
            {
                var resul = await _productoMongoRepository.ObtenerProductosPorGuidAsync(idUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener los productos en MongoDB: {ex.Message}", ex);
            }
        }

        public async Task<HttpStatusCode> ModificarProductoPostgreSQL(Producto productoModificar, Guid idUsuario, int idCategoria)
        {
            try
            {
                var resul = await _productoPostgreSQLRepository.ModificarProducto(productoModificar, idUsuario, idCategoria);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar modificar los productos en PostgreSQL: {ex.Message}", ex);
            }
        }

        public async Task<HttpStatusCode> ModificarProductoMongoAsync(Producto productoModificar, int idCategoria, Guid IdUsuario)
        {
            try
            {
                var resul = await _productoMongoRepository.ModificarProductoAsync(productoModificar, idCategoria, IdUsuario);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar modificar los productos en MongoDB: {ex.Message}", ex);
            }
        }


        public async Task<HttpStatusCode> EliminarProductoMongoAsync(Guid idProducto)
        {
            try
            {
                var resul = await _productoMongoRepository.EliminarProductoAsync(idProducto);
                return HttpStatusCode.OK;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar eliminar el producto de MongoDB {ex.Message}", ex);
            }
        }

        public async Task<HttpStatusCode> EliminarProductoPostgreSQLAsync(Guid idProducto)
        {
            try
            {
                var resul = await _productoPostgreSQLRepository.EliminarProductoAsync(idProducto);
                return HttpStatusCode.OK;
            }
            catch (System.Exception ex)
            {
                throw new PostgresRepositoryException($"Error al intentar eliminar el productos en PostgreSQL: {ex.Message}", ex);
            }
        }

        public async Task<Producto> ObtenerProductoPorIdMongo(Guid idProducto)
        {
            try
            {
                var resul = await _productoMongoRepository.ObtenerProductoPorId(idProducto);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener el producto de MongoDB {ex.Message}", ex);
            }
        }

        public async Task<Guid> ObtenerIdUsuarioPorProductoIdMongo(Guid idProducto)
        {
            try
            {
                var resul = await _productoMongoRepository.ObtenerIdUsuarioPorProductoId(idProducto);
                return resul;
            }
            catch (System.Exception ex)
            {
                throw new MongoRepositoryException($"Error al intentar obtener el producto de MongoDB {ex.Message}", ex);
            }
        }
    }
}
