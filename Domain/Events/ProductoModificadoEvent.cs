using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Events
{
    /// <summary>
    /// Clase Event que es consumida por un consumidor para modificar un producto en la base de datos en MongoDB
    /// </summary>
    public record ProductoModificadoEvent(Producto producto, int idCategoria, Guid idUsuario);
}
