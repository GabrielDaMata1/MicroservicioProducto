using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Models.PostgreSQL;

namespace Application.Mappers
{
    public static class ProductoPostgreSQLMapper
    {
        public static ProductoPostgreSQL ToPostgres(this Producto producto, int idCategoria,Guid idUsuario)
        {
            return new ProductoPostgreSQL
            {
                Id = producto.Id,
                Nombre = producto.NombreProducto.Nombre,
                Descripcion = producto.DescripcionProducto.descripcion,
                ImagenURL = producto.ImagenURLProducto.url,
                PrecioBase = producto.PrecioBaseProducto.precio,
                CategoriaId = idCategoria,
                IdUsuario = idUsuario,
                Estado = producto.EstadoProducto.estadoProducto
            };
        }

    }
}
