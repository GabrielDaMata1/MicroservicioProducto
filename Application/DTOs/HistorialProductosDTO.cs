using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Value_Object;

namespace Application.DTOs
{
    public class HistorialProductosDTO
    {
        public Guid Id { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public string ImagenURLProducto { get; set; }

        public decimal PrecioBaseProducto { get; set; }

        public string CategoriaProducto { get; set; }
        public HistorialProductosDTO (Guid id,string nombreProducto, string descripcionProducto, string imagenUrl, decimal  precioBaseProducto, string categoria)
        {
            Id = id;
            NombreProducto= nombreProducto;
            DescripcionProducto= descripcionProducto;
            ImagenURLProducto= imagenUrl;
            PrecioBaseProducto= precioBaseProducto;
            CategoriaProducto= categoria;
        }
    }
}
