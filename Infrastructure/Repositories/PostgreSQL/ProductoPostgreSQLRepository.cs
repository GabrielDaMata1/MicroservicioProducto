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
    public class ProductoPostgreSQLRepository: IProductoRepositoryPostgreSQL
    {
        private readonly SubastaDbContext _dbContext;

        public ProductoPostgreSQLRepository(SubastaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

         public async Task<Guid> RegistrarProductoAsync(Producto producto, int idCategoria, Guid IdUsuario)
         {
             var productoBD = producto.ToPostgres(idCategoria,IdUsuario);
             await _dbContext.Producto.AddAsync(productoBD);
             await _dbContext.SaveChangesAsync();
             return productoBD.Id;
         }


        public async Task<HttpStatusCode> ModificarProducto( Producto productoModificar, Guid idUsuario, int idCategoria)
        {
            Console.WriteLine(productoModificar.Id.GetType());
            Console.WriteLine(idUsuario.GetType());
            var producto = await _dbContext.Set<ProductoPostgreSQL>()
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(productoModificar.Id.ToString())); ;
            Console.WriteLine(productoModificar.Id.GetType());
            Console.WriteLine(idUsuario.GetType());


            if (producto == null)
                return HttpStatusCode.NotFound;

            producto.Id = productoModificar.Id;
            producto.Nombre = productoModificar.NombreProducto.Nombre ?? producto.Nombre;
            producto.Descripcion= productoModificar.DescripcionProducto.descripcion ?? producto.Descripcion;
            producto.ImagenURL = productoModificar.ImagenURLProducto.url ?? producto.ImagenURL;
            producto.PrecioBase = productoModificar.PrecioBaseProducto.precio;
            producto.CategoriaId = idCategoria;
            producto.IdUsuario=idUsuario;
            _dbContext.Set<ProductoPostgreSQL>().Update(producto);
            await _dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

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
