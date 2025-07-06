using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Mappers;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Models.PostgreSQL;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.PostgreSQL
{
    /// <summary>
    /// Clase repository que implementa las operaciones que se pueden realizar sobre los productos almacenados en PostgreSQL.
    /// </summary>
    public class ProductoPostgreSQLRepository: IProductoRepositoryPostgreSQL
    {
        /// <summary>
        /// Atributo que corresponde al contexto de la base de datos del Microservicio Producto en PostgreSQL.
        /// </summary>
        private readonly SubastaDbContext _dbContext;

        public ProductoPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Método que se encarga de registrar un producto en PostgreSQL.
        /// </summary>
        /// <param name="producto">Entidad que contiene los valores del producto a registrar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="IdUsuario">Parametro que corresponde al ID del subastador que registra el producto</param>
        /// <returns>Retorna el GUID del producto generado automaticamente al crear el objeto.</returns>
        public async Task<Guid> RegistrarProductoAsync(Producto producto, int idCategoria, Guid IdUsuario)
         {
             var productoBD = producto.ToPostgres(idCategoria,IdUsuario);
             await _dbContext.Producto.AddAsync(productoBD);
             await _dbContext.SaveChangesAsync();
             return productoBD.Id;
         }

        /// <summary>
        /// Método que se encarga de modificar un producto de un subastador en PostgreSQL.
        /// </summary>
        /// <param name="productoModificar">Entidad que contiene los valores del producto a modificar</param>
        /// <param name="idCategoria">Parametro que corresponde al ID de la categoria del producto</param>
        /// <param name="idUsuario">Parametro que corresponde al ID del subastador que modificar el producto</param>
        /// <returns>Retorna un estado HTTP exitoso si se modifica el producto</returns>
        public async Task<HttpStatusCode> ModificarProducto( Producto productoModificar, Guid idUsuario, int idCategoria)
        {
            var producto = await _dbContext.Set<ProductoPostgreSQL>()
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(productoModificar.Id.ToString())); ;



            if (producto == null)
                return HttpStatusCode.NotFound;

            producto.Id = productoModificar.Id;
            producto.Nombre = productoModificar.NombreProducto.Nombre ?? producto.Nombre;
            producto.Descripcion= productoModificar.DescripcionProducto.descripcion ?? producto.Descripcion;
            producto.ImagenURL = productoModificar.ImagenURLProducto.url ?? producto.ImagenURL;
            producto.PrecioBase = productoModificar.PrecioBaseProducto.precio;
            producto.CategoriaId = idCategoria;
            producto.IdUsuario=idUsuario;
            producto.Estado = productoModificar.EstadoProducto.estadoProducto;
            _dbContext.Set<ProductoPostgreSQL>().Update(producto);
            await _dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Método que se encarga de eliminar el producto de un subastador en PostgreSQL.
        /// </summary>
        /// <param name="idProducto">Parametro que corresponde al ID del producto a eliminar</param>
        /// <returns>Retorna un valor booleano True si se elimina el producto</returns>
        public async Task<bool> EliminarProductoAsync(Guid idProducto)
        {
            var producto = await _dbContext.Set<ProductoPostgreSQL>()
                .FirstOrDefaultAsync(p => p.Id == idProducto);

            if (producto == null)
                return false;

            _dbContext.Set<ProductoPostgreSQL>().Remove(producto);
            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
