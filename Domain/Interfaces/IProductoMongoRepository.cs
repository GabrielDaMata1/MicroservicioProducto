using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductoMongoRepository
    {
        Task<HttpStatusCode> RegistrarProductoAsync(Producto producto, int idCategoria, Guid IdUsuario);
        Task<int> ObtenerIdCategoria(string nombre);

        Task<List<Producto>> ObtenerProductosPorGuidAsync(Guid idUsuario);
        Task<HttpStatusCode> ModificarProductoAsync(Producto producto, int idCategoria, Guid IdUsuario);

        Task<bool> EliminarProductoAsync(Guid idProducto);

        Task<Producto> ObtenerProductoPorId(Guid idProducto);

        Task<Guid> ObtenerIdUsuarioPorProductoId(Guid idProducto);
    }

}
