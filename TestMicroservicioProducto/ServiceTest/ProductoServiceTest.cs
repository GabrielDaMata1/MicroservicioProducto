using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Value_Object;
using Moq;

namespace TestMicroservicioProducto.ServiceTest
{
    public class ProductoServiceTest
    {
        private readonly Mock<IProductoMongoRepository> _mockMongoRepo = new();
        private readonly Mock<IProductoRepositoryPostgreSQL> _mockPostgresRepo = new();
        private readonly ProductoService _service;

        public ProductoServiceTest()
        {
            _service = new ProductoService(_mockMongoRepo.Object, _mockPostgresRepo.Object);
        }

        [Fact]
        public async Task ObtenerIdCategoriaMongo_DeberiaRetornarId()
        {
            _mockMongoRepo.Setup(r => r.ObtenerIdCategoria("Tecnología")).ReturnsAsync(1);

            var result = await _service.ObtenerIdCategoriaMongo("Tecnología");

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task ObtenerIdCategoriaMongo_DeberiaLanzarMongoRepositoryException()
        {
            _mockMongoRepo.Setup(r => r.ObtenerIdCategoria("Error")).ThrowsAsync(new Exception("DB error"));

            var ex = await Assert.ThrowsAsync<MongoRepositoryException>(() => _service.ObtenerIdCategoriaMongo("Error"));

            Assert.NotNull(ex.InnerException);
            Assert.Equal("DB error", ex.InnerException.Message);
        }



        [Fact]
        public async Task RegistrarProductoPostgreSQLAsync_DeberiaRetornarGuid()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new EstadoProductoVO("Disponible"));
            var expectedId = Guid.NewGuid();

            _mockPostgresRepo.Setup(r => r.RegistrarProductoAsync(producto, 1, Guid.Empty)).ReturnsAsync(expectedId);

            var result = await _service.RegistrarProductoPostgreSQLAsync(producto, 1, Guid.Empty);

            Assert.Equal(expectedId, result);
        }

        [Fact]
        public async Task RegistrarProductoPostgreSQLAsync_DeberiaLanzarPostgresRepositoryException()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new EstadoProductoVO("Disponible"));

            _mockPostgresRepo.Setup(r => r.RegistrarProductoAsync(producto, 1, Guid.Empty)).ThrowsAsync(new Exception("DB error"));

            var ex = await Assert.ThrowsAsync<PostgresRepositoryException>(() => _service.RegistrarProductoPostgreSQLAsync(producto, 1, Guid.Empty));

            Assert.NotNull(ex.InnerException);
            Assert.Equal("DB error", ex.InnerException.Message);
        }


        [Fact]
        public async Task RegistrarProductoMongoAsync_DeberiaRetornarHttpStatusCode()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new EstadoProductoVO("Disponible"));

            _mockMongoRepo.Setup(r => r.RegistrarProductoAsync(producto, 1, Guid.Empty)).ReturnsAsync(HttpStatusCode.OK);

            var result = await _service.RegistrarProductoMongoAsync(producto, 1, Guid.Empty);

            Assert.Equal(HttpStatusCode.OK, result);
        }

        [Fact]
        public async Task RegistrarProductoMongoAsync_DeberiaLanzarMongoRepositoryException()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new EstadoProductoVO("Disponible"));

            _mockMongoRepo.Setup(r => r.RegistrarProductoAsync(producto, 1, Guid.Empty)).ThrowsAsync(new Exception("Mongo error"));

            var ex = await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.RegistrarProductoMongoAsync(producto, 1, Guid.Empty));

            Assert.NotNull(ex.InnerException);
            Assert.Equal("Mongo error", ex.InnerException.Message);
        }


        [Fact]
        public async Task ObtenerProductosPorGuidMongoAsync_DeberiaRetornarLista()
        {
            var productos = new List<Producto>
        {
            new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"))
        };

            _mockMongoRepo.Setup(r => r.ObtenerProductosPorGuidAsync(Guid.Empty)).ReturnsAsync(productos);

            var result = await _service.ObtenerProductosPorGuidMongoAsync(Guid.Empty);

            Assert.Single(result);
        }

        [Fact]
        public async Task ObtenerProductosPorGuidMongoAsync_DeberiaLanzarMongoRepositoryException()
        {
            _mockMongoRepo.Setup(r => r.ObtenerProductosPorGuidAsync(Guid.Empty)).ThrowsAsync(new Exception("Mongo error"));

            var ex = await Assert.ThrowsAsync<MongoRepositoryException>(() => _service.ObtenerProductosPorGuidMongoAsync(Guid.Empty));

            Assert.NotNull(ex.InnerException);
            Assert.Equal("Mongo error", ex.InnerException.Message);
        }


        [Fact]
        public async Task ModificarProductoPostgreSQL_DeberiaRetornarHttpStatusCode()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new EstadoProductoVO("Disponible"));

            _mockPostgresRepo.Setup(r => r.ModificarProducto(producto, Guid.Empty, 1)).ReturnsAsync(HttpStatusCode.OK);

            var result = await _service.ModificarProductoPostgreSQL(producto, Guid.Empty, 1);

            Assert.Equal(HttpStatusCode.OK, result);
        }

        [Fact]
        public async Task ModificarProductoPostgreSQL_DeberiaLanzarPostgresRepositoryException()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new EstadoProductoVO("Disponible"));

            _mockPostgresRepo.Setup(r => r.ModificarProducto(producto, Guid.Empty, 1)).ThrowsAsync(new Exception("Postgres error"));

            var ex = await Assert.ThrowsAsync<PostgresRepositoryException>(() => _service.ModificarProductoPostgreSQL(producto, Guid.Empty, 1));

            Assert.NotNull(ex.InnerException);
            Assert.Equal("Postgres error", ex.InnerException.Message);
        }
        [Fact]
        public async Task ModificarProductoMongoAsync_DeberiaRetornarHttpStatusCode()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new EstadoProductoVO("Disponible"));
            _mockMongoRepo.Setup(r => r.ModificarProductoAsync(producto, 1, Guid.Empty)).ReturnsAsync(HttpStatusCode.OK);

            var result = await _service.ModificarProductoMongoAsync(producto, 1, Guid.Empty);

            Assert.Equal(HttpStatusCode.OK, result);
        }

        [Fact]
        public async Task ModificarProductoMongoAsync_DeberiaLanzarMongoRepositoryException()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new EstadoProductoVO("Disponible"));
            _mockMongoRepo.Setup(r => r.ModificarProductoAsync(producto, 1, Guid.Empty)).ThrowsAsync(new Exception("Mongo error"));

            var ex = await Assert.ThrowsAsync<MongoRepositoryException>(() =>
                _service.ModificarProductoMongoAsync(producto, 1, Guid.Empty));

            Assert.NotNull(ex.InnerException);
            Assert.Equal("Mongo error", ex.InnerException.Message);
        }

        [Fact]
        public async Task EliminarProductoMongoAsync_DeberiaRetornarHttpStatusCode()
        {
            var idProducto = Guid.NewGuid();
            _mockMongoRepo.Setup(r => r.EliminarProductoAsync(idProducto)).ReturnsAsync(true);

            var result = await _service.EliminarProductoMongoAsync(idProducto);

            Assert.Equal(HttpStatusCode.OK, result);
        }

        [Fact]
        public async Task EliminarProductoMongoAsync_DeberiaLanzarMongoRepositoryException()
        {
            var idProducto = Guid.NewGuid();
            _mockMongoRepo.Setup(r => r.EliminarProductoAsync(idProducto)).ThrowsAsync(new Exception("Mongo error"));

            var ex = await Assert.ThrowsAsync<MongoRepositoryException>(() => _service.EliminarProductoMongoAsync(idProducto));

            Assert.NotNull(ex.InnerException);
            Assert.Equal("Mongo error", ex.InnerException.Message);
        }

        [Fact]
        public async Task EliminarProductoPostgreSQLAsync_DeberiaRetornarHttpStatusCode()
        {
            var idProducto = Guid.NewGuid();
            _mockPostgresRepo.Setup(r => r.EliminarProductoAsync(idProducto)).ReturnsAsync(true);

            var result = await _service.EliminarProductoPostgreSQLAsync(idProducto);

            Assert.Equal(HttpStatusCode.OK, result);
        }

        [Fact]
        public async Task EliminarProductoPostgreSQLAsync_DeberiaLanzarPostgresRepositoryException()
        {
            var idProducto = Guid.NewGuid();
            _mockPostgresRepo.Setup(r => r.EliminarProductoAsync(idProducto)).ThrowsAsync(new Exception("Postgres error"));

            var ex = await Assert.ThrowsAsync<PostgresRepositoryException>(() => _service.EliminarProductoPostgreSQLAsync(idProducto));

            Assert.NotNull(ex.InnerException);
            Assert.Equal("Postgres error", ex.InnerException.Message);
        }

        [Fact]
        public async Task ObtenerProductoPorIdMongo_DeberiaRetornarProducto()
        {
            var idProducto = Guid.NewGuid();
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new EstadoProductoVO("Disponible"));
            _mockMongoRepo.Setup(r => r.ObtenerProductoPorId(idProducto))
                          .ReturnsAsync(producto);

            var result = await _service.ObtenerProductoPorIdMongo(idProducto);

            Assert.Equal(producto, result);
        }

        [Fact]
        public async Task ObtenerProductoPorIdMongo_DeberiaLanzarMongoRepositoryException()
        {
            var idProducto = Guid.NewGuid();
            _mockMongoRepo.Setup(r => r.ObtenerProductoPorId(idProducto)).ThrowsAsync(new Exception("Mongo error"));

            var ex = await Assert.ThrowsAsync<MongoRepositoryException>(() => _service.ObtenerProductoPorIdMongo(idProducto));

            Assert.NotNull(ex.InnerException);
            Assert.Equal("Mongo error", ex.InnerException.Message);
        }

        [Fact]
        public async Task ObtenerIdUsuarioPorProductoIdMongo_DeberiaRetornarGuid()
        {
            var idProducto = Guid.NewGuid();
            var idUsuario = Guid.NewGuid();
            _mockMongoRepo.Setup(r => r.ObtenerIdUsuarioPorProductoId(idProducto)).ReturnsAsync(idUsuario);

            var result = await _service.ObtenerIdUsuarioPorProductoIdMongo(idProducto);

            Assert.Equal(idUsuario, result);
        }

        [Fact]
        public async Task ObtenerIdUsuarioPorProductoIdMongo_DeberiaLanzarMongoRepositoryException()
        {
            var idProducto = Guid.NewGuid();
            _mockMongoRepo.Setup(r => r.ObtenerIdUsuarioPorProductoId(idProducto)).ThrowsAsync(new Exception("Mongo error"));

            var ex = await Assert.ThrowsAsync<MongoRepositoryException>(() => _service.ObtenerIdUsuarioPorProductoIdMongo(idProducto));

            Assert.NotNull(ex.InnerException);
            Assert.Equal("Mongo error", ex.InnerException.Message);
        }


    }
}
