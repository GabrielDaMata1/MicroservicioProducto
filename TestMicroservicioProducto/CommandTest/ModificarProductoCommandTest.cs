using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;

namespace TestMicroservicioProducto.CommandTest
{
    public class ModificarProductoCommandTest
    {
        [Fact]
        public void Constructor_DeberiaAsignarPropiedadesCorrectamente()
        {
            var productoId = Guid.NewGuid();
            var dto = new ModificarProductoDTO
            {
                Id = productoId,
                NombreProducto = "Nuevo nombre",
                DescripcionProducto = "Nueva descripción",
                PrecioBaseProducto = 150.00m,
                CategoriaProducto = "Tecnología",
                ImagenURLProducto = "https://imagen.com/producto.jpg",
                EstadoProducto = "Disponible"
            };
            var correo = "subastador@ejemplo.com";

            var command = new ModificarProductoCommand(dto, correo);

            // Assert
            Assert.Equal(dto, command.ProductoDto);
            Assert.Equal(correo, command.correo);
        }


    }
}
