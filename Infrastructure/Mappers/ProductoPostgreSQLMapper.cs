using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Models.PostgreSQL;

namespace Application.Mappers
{    
    /// <summary>
     /// Clase mapper que se encarga de mapear el objeto de tipo Entidad Producto (Dominio) a una entidad en la base de datos en PostgreSQL
     /// </summary>
    public static class ProductoPostgreSQLMapper
    {
        /// <summary>
        /// Método que se encarga de mapear un producto (Entidad) a un modelo en la base de datos en PostgreSQL.
        /// </summary>
        /// <param name="producto">Entidad que contiene los valores del producto a registrar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="idUsuario">Parametro que corresponde al ID del subastador que registra el producto</param>
        /// <returns>Retorna un objeto de tipo ProductoMongo, que corresponde al modelo de producto en la base de datos en PostgreSQL.</returns>
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
