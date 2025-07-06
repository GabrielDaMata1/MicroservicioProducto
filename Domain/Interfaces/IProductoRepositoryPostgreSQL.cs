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
    /// Clase interface que define las operaciones que se pueden realizar sobre los productos almacenados en PostgreSQL.
    /// </summary>
    public interface IProductoRepositoryPostgreSQL
    {
        /// <summary>
        /// Método que se encarga de registrar un producto en PostgreSQL.
        /// </summary>
        /// <param name="producto">Entidad que contiene los valores del producto a registrar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="IdUsuario">Parametro que corresponde al ID del subastador que registra el producto</param>
        /// <returns>Retorna el GUID del producto generado automaticamente al crear el objeto.</returns>
        Task<Guid> RegistrarProductoAsync(Producto producto, int idCategoria, Guid IdUsuario);
        /// <summary>
        /// Método que se encarga de modificar un producto de un subastador en PostgreSQL.
        /// </summary>
        /// <param name="productoModificar">Entidad que contiene los valores del producto a modificar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="idUsuario">Parametro que corresponde al ID del subastador que modificar el producto</param>
        /// <returns>Retorna un estado HTTP exitoso si se modifica el producto</returns>
        Task<HttpStatusCode> ModificarProducto(Producto productoModificar, Guid idUsuario, int idCategoria);
        /// <summary>
        /// Método que se encarga de eliminar el producto de un subastador en PostgreSQL.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a eliminar</param>
        /// <returns>Retorna un valor booleano True si se elimina el producto</returns>
        Task<bool> EliminarProductoAsync(Guid id);
    }
}
