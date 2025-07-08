using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Command;
using Application.DTOs;
using Application.Exception;
using Application.Handler;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using Domain.Value_Object;
using Infrastructure.Models.MongoDB;
using MassTransit;
using Moq;

namespace TestMicroservicioProducto.CommandTest
{
    public class EliminarProductoCommandTest
    {
        [Fact]
        public void Constructor_DeberiaAsignarPropiedadesCorrectamente()
        {
            var productoId = Guid.NewGuid();
            var dto = new EliminarProductoDTO
            {
                idProducto = productoId
            };
            var correo = "subastador@ejemplo.com";

            var command = new EliminarProductoCommand(dto, correo);

            Assert.Equal(dto, command.ProductoDto);
            Assert.Equal(correo, command.correo);
        }
    }
}
