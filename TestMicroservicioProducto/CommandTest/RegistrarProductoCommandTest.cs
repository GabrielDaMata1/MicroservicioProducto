using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;

namespace TestMicroservicioProducto.CommandTest
{
    public class RegistrarProductoCommandTest
    {
        [Fact]
        public void Constructor_DeberiaAsignarProductoDtoCorrectamente()
        {
            var dto = new RegistrarProductoDTO
            {
                nombreProducto = "Producto nuevo",
                descripcionProducto = "Descripción del producto",
                imagenURLProducto = "https://firebase.com/imagen.jpg",
                precioBase = 199.99m,
                categoria = "Tecnología",
                correo = "subastador@ejemplo.com"
            };


            var command = new RegistrarProductoCommand(dto);

            Assert.Equal(dto, command.ProductoDto);
        }

    }
}
