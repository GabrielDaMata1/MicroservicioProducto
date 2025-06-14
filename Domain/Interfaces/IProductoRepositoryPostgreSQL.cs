using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProductoRepositoryPostgreSQL
    {
            Task<Guid> RegistrarProductoAsync(Producto producto, int idCategoria, Guid IdUsuario);
            Task<HttpStatusCode> ModificarProducto(Producto productoModificar, Guid idUsuario, int idCategoria);
            Task<bool> EliminarProductoAsync(Guid id);
    }
}
