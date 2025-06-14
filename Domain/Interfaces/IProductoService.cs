using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductoService
    {
        Task<HttpStatusCode> RegistrarProductoMongoAsync(Producto producto, int idCategoria, Guid IdUsuario);
        Task<Guid> RegistrarProductoPostgreSQLAsync(Producto producto, int idCategoria, Guid IdUsuario);
        Task<int> ObtenerIdCategoriaMongo(string nombre);
        Task<HttpStatusCode> ModificarProductoPostgreSQL(Producto productoModificar, Guid idUsuario, int idCategoria);
        Task<List<Producto>> ObtenerProductosPorGuidMongoAsync(Guid idUsuario);
        Task<HttpStatusCode> ModificarProductoMongoAsync(Producto producto, int idCategoria, Guid IdUsuario);

        Task<HttpStatusCode> EliminarProductoMongoAsync(Guid id);
        Task<HttpStatusCode> EliminarProductoPostgreSQLAsync(Guid id);

        Task<Producto> ObtenerProductoPorIdMongo(Guid idProducto);

        Task<Guid> ObtenerIdUsuarioPorProductoIdMongo(Guid idProducto);
    }
}
