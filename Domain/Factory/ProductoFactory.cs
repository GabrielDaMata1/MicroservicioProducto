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
    /// <summary>
    /// Clase Factory que se encarga de crear una instancia de la entidad Producto
    /// </summary>
    public static class ProductoFactory
    {
        /// <summary>
        /// Método que se encarga de crear una instancia de Producto.
        /// </summary>
        /// <param name="nombre">Parametro que corresponde al nombre del producto a crear</param>
        /// <param name="descripcion">Parametro que corresponde al correo del usuario a consultar</param>
        /// <param name="urlImagen">Parametro que corresponde al correo del usuario a consultar</param>
        /// <param name="precioBase">Parametro que corresponde al correo del usuario a consultar</param>
        /// <returns>Retorna un objeto de tipo Producto.</returns>
        public static Producto CrearProducto(string nombre, string descripcion, string urlImagen, decimal precioBase)
        {
            var nombreVO = new NombreProductoVO(nombre);
            var descripcionVO = new DescripcionProductoVO(descripcion);
            var urlImagenVO = new ImagenURLProductoVO(urlImagen);
            var precioBaseVO = new PrecioBaseProductoVO(precioBase);

            return new Producto(nombreVO, descripcionVO, urlImagenVO, precioBaseVO);
        }
        /// <summary>
        /// Método que se encarga de crear una instancia de Producto, incluyendo su estado y ID.
        /// </summary>
        /// <param name="id">Parametro que corresponde al id del producto a crear</param>
        /// <param name="nombre">Parametro que corresponde al nombre del producto a crear</param>
        /// <param name="descripcion">Parametro que corresponde a la descripcio del producto a crear</param>
        /// <param name="urlImagen">Parametro que corresponde al url de la imagen del producto a crear</param>
        /// <param name="precioBase">Parametro que corresponde al precio base del producto a crear</param>
        /// <param name="estado">Parametro que corresponde al estado del producto a crear</param>
        /// <returns>Retorna un objeto de tipo Producto.</returns>
        public static Producto CrearProductoConId(Guid id, string nombre, string descripcion, string urlImagen, decimal precioBase, string estado)
        {
            var nombreVO = new NombreProductoVO(nombre);
            var descripcionVO = new DescripcionProductoVO(descripcion);
            var urlImagenVO = new ImagenURLProductoVO(urlImagen);
            var precioBaseVO = new PrecioBaseProductoVO(precioBase);
            var estadoVO = new EstadoProductoVO(estado);

            return new Producto(id,nombreVO, descripcionVO, urlImagenVO, precioBaseVO,estadoVO);
        }
        /// <summary>
        /// Método que se encarga de crear una instancia de Producto, incluyendo su estado, ID y categoria.
        /// </summary>
        /// <param name="id">Parametro que corresponde al id del producto a crear</param>
        /// <param name="nombre">Parametro que corresponde al nombre del producto a crear</param>
        /// <param name="descripcion">Parametro que corresponde a la descripcio del producto a crear</param>
        /// <param name="urlImagen">Parametro que corresponde al url de la imagen del producto a crear</param>
        /// <param name="precioBase">Parametro que corresponde al precio base del producto a crear</param>
        /// <param name="categoria">Parametro que corresponde a la categoria del producto a crear</param>
        /// <param name="estado">Parametro que corresponde al estado del producto a crear</param>
        /// <returns>Retorna un objeto de tipo Producto.</returns>
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
