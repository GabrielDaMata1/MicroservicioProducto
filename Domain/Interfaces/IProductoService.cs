using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// Clase interface que define las operaciones que se pueden realizar sobre los productos almacenados en ambas bases de datos (PostgreSQL, MongoDB).
    /// </summary>
    public interface IProductoService
    {
        /// <summary>
        /// Método que se encarga de registrar un producto en MongoDB.
        /// </summary>
        /// <param name="producto">Entidad que contiene los valores del producto a registrar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="IdUsuario">Parametro que corresponde al ID del subastador que registra el producto</param>
        /// <returns>Retorna un estado HTTP exitoso si se registra el producto</returns>
        Task<HttpStatusCode> RegistrarProductoMongoAsync(Producto producto, int idCategoria, Guid IdUsuario);
        /// <summary>
        /// Método que se encarga de registrar un producto en PostgreSQL.
        /// </summary>
        /// <param name="producto">Entidad que contiene los valores del producto a registrar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="IdUsuario">Parametro que corresponde al ID del subastador que registra el producto</param>
        /// <returns>Retorna el GUID del producto generado automaticamente al crear el objeto.</returns>
        Task<Guid> RegistrarProductoPostgreSQLAsync(Producto producto, int idCategoria, Guid IdUsuario);
        /// <summary>
        /// Método que se encarga de obtener el ID de una categoria en MongoDB.
        /// </summary>
        /// <param name="nombre">Parametro que corresponde el nombre de la categoria a consultar</param>
        /// <returns>Retorna un valor entero que corresponde al ID de la categoria consultada.</returns>
        Task<int> ObtenerIdCategoriaMongo(string nombre);
        /// <summary>
        /// Método que se encarga de modificar un producto de un subastador en PostgreSQL.
        /// </summary>
        /// <param name="productoModificar">Entidad que contiene los valores del producto a modificar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="idUsuario">Parametro que corresponde al ID del subastador que modificar el producto</param>
        /// <returns>Retorna un estado HTTP exitoso si se modifica el producto</returns>
        Task<HttpStatusCode> ModificarProductoPostgreSQL(Producto productoModificar, Guid idUsuario, int idCategoria);
        /// <summary>
        /// Método que se encarga de obtener los productos de un subastador en MongoDB.
        /// </summary>
        /// <param name="idUsuario">Parametro que corresponde al ID del subastador que consulta los producto</param>
        /// <returns>Retorna una lista de Productos con su detalle</returns>
        Task<List<Producto>> ObtenerProductosPorGuidMongoAsync(Guid idUsuario);
        /// <summary>
        /// Método que se encarga de modificar un producto de un subastador en MongoDB.
        /// </summary>
        /// <param name="productoModificar">Entidad que contiene los valores del producto a modificar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="IdUsuario">Parametro que corresponde al ID del subastador que modificar el producto</param>
        /// <returns>Retorna un estado HTTP exitoso si se modifica el producto</returns>
        Task<HttpStatusCode> ModificarProductoMongoAsync(Producto producto, int idCategoria, Guid IdUsuario);
        /// <summary>
        /// Método que se encarga de eliminar el producto de un subastador en MongoDB.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a eliminar</param>
        /// <returns>Retorna un estado HTTP exitoso si se elimina el producto</returns>
        Task<HttpStatusCode> EliminarProductoMongoAsync(Guid id);
        /// <summary>
        /// Método que se encarga de eliminar el producto de un subastador en PostgreSQL.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a eliminar</param>
        /// <returns>Retorna un valor booleano True si se elimina el producto</returns>
        Task<HttpStatusCode> EliminarProductoPostgreSQLAsync(Guid id);
        /// <summary>
        /// Método que se encarga de obtener el producto de un subastador en MongoDB.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar</param>
        /// <returns>Retorna el Producto con su detalle</returns>
        Task<Producto> ObtenerProductoPorIdMongo(Guid idProducto);
        /// <summary>
        /// Método que se encarga de obtener el ID del subastador a quien le pertence el producto en MongoDB.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a consultar</param>
        /// <returns>Retorna el ID del subastador a quien le pertenece el producto</returns>
        Task<Guid> ObtenerIdUsuarioPorProductoIdMongo(Guid idProducto);
    }
}
