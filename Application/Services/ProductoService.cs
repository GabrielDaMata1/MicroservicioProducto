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
    /// <summary>
    /// Clase Service que se encarga de procesar todas las operaciones sobre un producto, incluyendo las operaciones con bases de datos (PostgreSQL, MongoDB).
    /// </summary>
    public class ProductoService : IProductoService

    {
        /// <summary>
        /// Atributo que corresponde al repositorio de productos en la base de datos en MongoDB.
        /// </summary>
        private readonly IProductoMongoRepository _productoMongoRepository;
        /// <summary>
        /// Atributo que corresponde al repositorio de productos en la base de datos en PostgreSQL.
        /// </summary>
        private readonly IProductoRepositoryPostgreSQL _productoPostgreSQLRepository;


        public ProductoService(IProductoMongoRepository productoMongoRepository, IProductoRepositoryPostgreSQL productoPostgreSQLRepository)

        {
            _productoMongoRepository = productoMongoRepository;
            _productoPostgreSQLRepository = productoPostgreSQLRepository;

        }

        /// <summary>
        /// Método que se encarga de obtener el ID de una categoria en MongoDB.
        /// </summary>
        /// <param name="nombre">Parametro que corresponde el nombre de la categoria a consultar</param>
        /// <returns>Retorna un valor entero que corresponde al ID de la categoria consultada.</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar el id de la categoria en la base de datos.
        /// </exception>
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

        /// <summary>
        /// Método que se encarga de registrar un producto en PostgreSQL.
        /// </summary>
        /// <param name="producto">Entidad que contiene los valores del producto a registrar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="IdUsuario">Parametro que corresponde al ID del subastador que registra el producto</param>
        /// <returns>Retorna el GUID del producto generado automaticamente al crear el objeto.</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar el producto en la base de datos en PostgreSQL.
        /// </exception>
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

        /// <summary>
        /// Método que se encarga de registrar un producto en MongoDB.
        /// </summary>
        /// <param name="producto">Entidad que contiene los valores del producto a registrar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="IdUsuario">Parametro que corresponde al ID del subastador que registra el producto</param>
        /// <returns>Retorna un estado HTTP exitoso si se registra el producto</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al registrar el producto en la base de datos en MongoDB.
        /// </exception>
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

        /// <summary>
        /// Método que se encarga de obtener los productos de un subastador en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parametro que corresponde al ID del subastador que consulta los producto</param>
        /// <returns>Retorna una lista de Productos con su detalle</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar los productos en la base de datos en MongoDB.
        /// </exception>
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

        /// <summary>
        /// Método que se encarga de modificar un producto de un subastador en PostgreSQL.
        /// </summary>
        /// <param name="productoModificar">Entidad que contiene los valores del producto a modificar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="idUsuario">Parametro que corresponde al ID del subastador que modificar el producto</param>
        /// <returns>Retorna un estado HTTP exitoso si se modifica el producto</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al modificar el productos en la base de datos en PostgreSQL.
        /// </exception>
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
        /// <summary>
        /// Método que se encarga de modificar un producto de un subastador en MongoDB.
        /// </summary>
        /// <param name="productoModificar">Entidad que contiene los valores del producto a modificar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="IdUsuario">Parametro que corresponde al ID del subastador que modificar el producto</param>
        /// <returns>Retorna un estado HTTP exitoso si se modifica el producto</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al modificar el productos en la base de datos en MongoDB.
        /// </exception>
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


        /// <summary>
        /// Método que se encarga de eliminar el producto de un subastador en MongoDB.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a eliminar</param>
        /// <returns>Retorna un estado HTTP exitoso si se elimina el producto</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al eliminar el producto en la base de datos en MongoDB.
        /// </exception>
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

        /// <summary>
        /// Método que se encarga de eliminar el producto de un subastador en PostgreSQL.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a eliminar</param>
        /// <returns>Retorna un estado HTTP exitoso si se elimina el producto</returns>
        /// <exception cref="PostgresRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al eliminar el producto en la base de datos en PostgreSQL.
        /// </exception>
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
        /// <summary>
        /// Método que se encarga de obtener el producto de un subastador en MongoDB.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar</param>
        /// <returns>Retorna el Producto con su detalle</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar el producto en la base de datos en MongoDB.
        /// </exception>

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

        /// <summary>
        /// Método que se encarga de obtener el ID del subastador a quien le pertence el producto en MongoDB.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar</param>
        /// <returns>Retorna el ID del subastador a quien le pertenece el producto</returns>
        /// <exception cref="MongoRepositoryException">
        /// Esta excepcion ocurre si sucede un problema al consultar el producto en la base de datos en MongoDB.
        /// </exception>
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
