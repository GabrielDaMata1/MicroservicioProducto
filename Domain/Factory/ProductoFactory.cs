using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Value_Object;
using MassTransit.NewIdProviders;

namespace Domain.Factory
{
    public static class ProductoFactory
    {
        public static Producto CrearProducto(string nombre, string descripcion, string urlImagen, decimal precioBase)
        {
            var nombreVO = new NombreProductoVO(nombre);
            var descripcionVO = new DescripcionProductoVO(descripcion);
            var urlImagenVO = new ImagenURLProductoVO(urlImagen);
            var precioBaseVO = new PrecioBaseProductoVO(precioBase);

            return new Producto(nombreVO, descripcionVO, urlImagenVO, precioBaseVO);
        }

        public static Producto CrearProductoConId(Guid id, string nombre, string descripcion, string urlImagen, decimal precioBase, string estado)
        {
            var nombreVO = new NombreProductoVO(nombre);
            var descripcionVO = new DescripcionProductoVO(descripcion);
            var urlImagenVO = new ImagenURLProductoVO(urlImagen);
            var precioBaseVO = new PrecioBaseProductoVO(precioBase);
            var estadoVO = new EstadoProductoVO(estado);

            return new Producto(id,nombreVO, descripcionVO, urlImagenVO, precioBaseVO,estadoVO);
        }

        public static Producto CrearProductoConIdYCategoria(Guid id, string nombre, string descripcion, string urlImagen, decimal precioBase, string categoria, string estado)
        {
            var nombreVO = new NombreProductoVO(nombre);
            var descripcionVO = new DescripcionProductoVO(descripcion);
            var urlImagenVO = new ImagenURLProductoVO(urlImagen);
            var precioBaseVO = new PrecioBaseProductoVO(precioBase);
            var categoriaVO = new CategoriaProductoVO(categoria);
            var estadoVO = new EstadoProductoVO(estado);

            return new Producto(id, nombreVO, descripcionVO, urlImagenVO, precioBaseVO, categoriaVO, estadoVO);
        }
    }
}
