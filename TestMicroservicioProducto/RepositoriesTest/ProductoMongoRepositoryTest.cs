using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Exception;
using Domain.Entities;
using Domain.Value_Object;
using Infrastructure.Models.MongoDB;
using Infrastructure.Repositories.MongoDB;
using MongoDB.Driver;
using Moq;

namespace TestMicroservicioProducto.RepositoriesTest
{
    public class ProductoMongoRepositoryTest
    {
        private readonly Mock<IMongoCollection<ProductoMongo>> _mockProductoCollection = new();
        private readonly Mock<IMongoCollection<CategoriaMongo>> _mockCategoriaCollection = new();
        private readonly Mock<IMongoDatabase> _mockDatabase = new();
        private readonly Mock<IMongoClient> _mockMongoClient = new();
        private readonly ProductoMongoRepository _repository;

        public ProductoMongoRepositoryTest()
        {
            _mockMongoClient.Setup(c => c.GetDatabase("MicroservicioProducto", null))
                .Returns(_mockDatabase.Object);

            _mockDatabase.Setup(d => d.GetCollection<ProductoMongo>("Producto", null))
                .Returns(_mockProductoCollection.Object);

            _mockDatabase.Setup(d => d.GetCollection<CategoriaMongo>("Categoria", null))
                .Returns(_mockCategoriaCollection.Object);

            _repository = new ProductoMongoRepository(_mockMongoClient.Object);
        }

        [Fact]
        public async Task RegistrarProductoAsync_DeberiaInsertarYRetornarOK()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"));

            _mockProductoCollection
                .Setup(c => c.InsertOneAsync(It.IsAny<ProductoMongo>(), null, default))
                .Returns(Task.CompletedTask);

            var result = await _repository.RegistrarProductoAsync(producto, 1, Guid.NewGuid());

            Assert.Equal(HttpStatusCode.OK, result);
            _mockProductoCollection.Verify(c => c.InsertOneAsync(It.IsAny<ProductoMongo>(), null, default), Times.Once);
        }

        [Fact]
        public async Task RegistrarProductoAsync_DeberiaLanzarExcepcion_SiProductoEsNull()
        {
            await Assert.ThrowsAsync<ProductoVacioException>(() =>
                _repository.RegistrarProductoAsync(null, 1, Guid.NewGuid()));
        }

        [Fact]
        public async Task EliminarProductoAsync_DeberiaRetornarTrue_SiSeElimina()
        {
            var id = Guid.NewGuid();
            var deleteResult = new DeleteResult.Acknowledged(1);

            _mockProductoCollection
                .Setup(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<ProductoMongo>>(), default))
                .ReturnsAsync(deleteResult);

            var result = await _repository.EliminarProductoAsync(id);

            Assert.True(result);
        }

        [Fact]
        public async Task ObtenerIdCategoria_CategoriaExistente_RetornaId()
        {
            var mockCursor = new Mock<IAsyncCursor<CategoriaMongo>>();
            var mockCategoria = new CategoriaMongo { Id = 1, nombre = "Categoria1" };

            mockCursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);

            mockCursor.Setup(_ => _.Current).Returns(new List<CategoriaMongo> { mockCategoria }); _mockCategoriaCollection.Setup(c => c.FindAsync(
                    It.IsAny<FilterDefinition<CategoriaMongo>>(), It.IsAny<FindOptions<CategoriaMongo, CategoriaMongo>>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockCursor.Object);
            var result = await _repository.ObtenerIdCategoria("Categoria1");
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task ModificarProductoAsync_DeberiaReemplazarYRetornarOK()
        {
            var producto = new Producto(Guid.NewGuid(), new NombreProductoVO("P"), new DescripcionProductoVO("D"), new ImagenURLProductoVO("url"), new PrecioBaseProductoVO(100), new CategoriaProductoVO("Cat"), new EstadoProductoVO("Disponible"));
            var idCategoria = 1;
            var idUsuario = Guid.NewGuid();

            _mockProductoCollection.Setup(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<ProductoMongo>>(), It.IsAny<ProductoMongo>(), 
                    It.IsAny<ReplaceOptions>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ReplaceOneResult.Acknowledged(1, 1, true));

            var result = await _repository.ModificarProductoAsync(producto, idCategoria, idUsuario);

            _mockProductoCollection.Verify(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<ProductoMongo>>(), It.IsAny<ProductoMongo>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ObtenerProductosPorGuidAsync_DeberiaRetornarListaDeProductos_CuandoExistenProductos()
        {
            var idUsuario = Guid.NewGuid();
            var productoMongo = new ProductoMongo
            {
                Id = Guid.NewGuid(),
                Nombre = "Producto 1",
                Descripcion = "Descripción 1",
                ImagenURL = "url1",
                PrecioBase = 100,
                CategoriaId = 1,
                Estado = "Disponible",
                IdUsuario = idUsuario
            };
            var categoriaMongo = new CategoriaMongo
            {
                Id = 1,
                nombre = "Categoria 1"
            };
            var mockCursorProductos = new Mock<IAsyncCursor<ProductoMongo>>();
            mockCursorProductos.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
            mockCursorProductos.Setup(_ => _.Current).Returns(new List<ProductoMongo> { productoMongo }); 
            _mockProductoCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<ProductoMongo>>(), It.IsAny<FindOptions<ProductoMongo, ProductoMongo>>(), 
                    It.IsAny<CancellationToken>())).ReturnsAsync(mockCursorProductos.Object);
            var mockCursorCategoria = new Mock<IAsyncCursor<CategoriaMongo>>();
            mockCursorCategoria.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
            mockCursorCategoria.Setup(_ => _.Current).Returns(new List<CategoriaMongo> { categoriaMongo });
            _mockCategoriaCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<CategoriaMongo>>(), It.IsAny<FindOptions<CategoriaMongo, CategoriaMongo>>(), 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursorCategoria.Object);

            var result = await _repository.ObtenerProductosPorGuidAsync(idUsuario);
            Assert.Single(result);
            Assert.Equal(productoMongo.Id, result[0].Id);
            Assert.Equal(productoMongo.Nombre, result[0].NombreProducto.Nombre);
            Assert.Equal(categoriaMongo.nombre, result[0].CategoriaProducto.categoria);

        }

        [Fact]
        public async Task ObtenerProductoPorId_DeberiaRetornarProducto_CuandoElProductoExiste()
        {
            var idProducto = Guid.NewGuid();
            var productoMongo = new ProductoMongo
            {
                Id = idProducto,
                Nombre = "Producto 1",
                Descripcion = "Descripción 1",
                ImagenURL = "url1",
                PrecioBase = 100,
                CategoriaId = 1,
                Estado = "Disponible"
            };
            var categoriaMongo = new CategoriaMongo
            {
                Id = 1,
                nombre = "Categoria 1"
            };
            var mockCursorProductos = new Mock<IAsyncCursor<ProductoMongo>>();
            mockCursorProductos.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
            mockCursorProductos.Setup(_ => _.Current).Returns(new List<ProductoMongo> { productoMongo });

            _mockProductoCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<ProductoMongo>>(), It.IsAny<FindOptions<ProductoMongo, ProductoMongo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursorProductos.Object);

            var mockCursorCategoria = new Mock<IAsyncCursor<CategoriaMongo>>();
            mockCursorCategoria.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
            mockCursorCategoria.Setup(_ => _.Current).Returns(new List<CategoriaMongo> { categoriaMongo });

            _mockCategoriaCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<CategoriaMongo>>(), It.IsAny<FindOptions<CategoriaMongo, CategoriaMongo>>(),
                    It.IsAny<CancellationToken>())).ReturnsAsync(mockCursorCategoria.Object);
            var result = await _repository.ObtenerProductoPorId(idProducto);
            Assert.NotNull(result);
            Assert.Equal(productoMongo.Id, result.Id);
            Assert.Equal(productoMongo.Nombre, result.NombreProducto.Nombre);
            Assert.Equal(productoMongo.Descripcion, result.DescripcionProducto.descripcion);
            Assert.Equal(productoMongo.ImagenURL, result.ImagenURLProducto.url);
            Assert.Equal(productoMongo.PrecioBase, result.PrecioBaseProducto.precio);
            Assert.Equal(categoriaMongo.nombre, result.CategoriaProducto.categoria);
        }

        [Fact]
        public async Task ObtenerIdUsuarioPorProductoId_DeberiaRetornarIdUsuario_CuandoElProductoExiste()
        {
            var idProducto = Guid.NewGuid();
            var idUsuario = Guid.NewGuid();
            var productoMongo = new ProductoMongo
            {
                Id = idProducto,
                IdUsuario = idUsuario,
                Nombre = "Producto 1",
                Descripcion = "Descripción 1",
                ImagenURL = "url1",
                PrecioBase = 100,
                CategoriaId = 1,
                Estado = "Disponible"
            };

            var mockCursorProductos = new Mock<IAsyncCursor<ProductoMongo>>();
            mockCursorProductos.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true).ReturnsAsync(false);
            mockCursorProductos.Setup(_ => _.Current).Returns(new List<ProductoMongo> { productoMongo });

            _mockProductoCollection.Setup(c => c.FindAsync(It.IsAny<FilterDefinition<ProductoMongo>>(), It.IsAny<FindOptions<ProductoMongo, ProductoMongo>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockCursorProductos.Object);
            var result = await _repository.ObtenerIdUsuarioPorProductoId(idProducto);
            Assert.Equal(idUsuario, result);
        }
    }
}