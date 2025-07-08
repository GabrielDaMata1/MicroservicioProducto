using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Mappers;
using Domain.Entities;
using Domain.Value_Object;
using Infrastructure.Models.PostgreSQL;
using Infrastructure.Persistance;
using Infrastructure.Repositories.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace TestMicroservicioProducto.RepositoriesTest
{
    public class ProductoPostgreSQLRepositoryTest
    {
        private readonly SubastaDbContext _dbContext;
        private readonly ProductoPostgreSQLRepository _repository;

        public ProductoPostgreSQLRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<SubastaDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new SubastaDbContext(options);
            _repository = new ProductoPostgreSQLRepository(_dbContext);
        }

        [Fact]
        public async Task RegistrarProductoAsync_DeberiaAgregarYRetornarId()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"));
            var id = await _repository.RegistrarProductoAsync(producto, 1, Guid.NewGuid());

            var productoEnDb = await _dbContext.Producto.FindAsync(id);
            Assert.NotNull(productoEnDb);
            Assert.Equal(id, productoEnDb.Id);
        }

        [Fact]
        public async Task ModificarProducto_DeberiaActualizarYRetornarOK()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"));
            var idUsuario = Guid.NewGuid();
            var id = await _repository.RegistrarProductoAsync(producto, 1, idUsuario);

            var modificado = new Producto(
                id,
                new NombreProductoVO("Nuevo nombre"),
                new DescripcionProductoVO("Nueva descripción"),
                new ImagenURLProductoVO("https://nuevo.com"),
                new PrecioBaseProductoVO(999),
                new EstadoProductoVO("Modificado")
            );

            var result = await _repository.ModificarProducto(modificado, idUsuario, 2);

            var actualizado = await _dbContext.Producto.FindAsync(id);
            Assert.Equal(HttpStatusCode.OK, result);
            Assert.Equal("Nuevo nombre", actualizado.Nombre);
            Assert.Equal(2, actualizado.CategoriaId);
        }

        [Fact]
        public async Task ModificarProducto_DeberiaRetornarNotFound_SiNoExiste()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"));
            var result = await _repository.ModificarProducto(producto, Guid.NewGuid(), 1);
            Assert.Equal(HttpStatusCode.NotFound, result);
        }

        [Fact]
        public async Task EliminarProductoAsync_DeberiaEliminarYRetornarTrue()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"));
            var id = await _repository.RegistrarProductoAsync(producto, 1, Guid.NewGuid());

            var result = await _repository.EliminarProductoAsync(id);

            var eliminado = await _dbContext.Producto.FindAsync(id);
            Assert.True(result);
            Assert.Null(eliminado);
        }

        [Fact]
        public async Task EliminarProductoAsync_DeberiaRetornarFalse_SiNoExiste()
        {
            var result = await _repository.EliminarProductoAsync(Guid.NewGuid());
            Assert.False(result);
        }


    }
}
