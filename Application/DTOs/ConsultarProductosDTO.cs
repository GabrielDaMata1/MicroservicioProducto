using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{

    /// <summary>
    /// Clase DTO que se encarga de encapsular la información necesaria para consultar un producto registrado previamente.
    /// </summary>
    public class ConsultarProductosDTO
    {
        /// <summary>
        /// Atributo que corresponde al correo del subastador al que le pertenecen los productos consultar.
        /// </summary>
        public string correo;
    }
}
