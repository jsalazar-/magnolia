using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFloor.ModernUI.App.Modelo
{
    public class Producto
    {
        public string idProducto { get; set; }
        public string nombre { get; set; }
        public string stock { get; set; }
        public string precio { get; set; }
        public string precioReal { get; set; }
        public int idCategoria { get; set; }
        public int total { get; set; }
        public DateTime fecha { get; set; }
        public Producto() { 

        }
        public Producto(DateTime fecha, string idProducto, string nombre, string precioReal, string stock,int total, int idCategoria)
        {
            this.fecha = fecha;
            this.idProducto = idProducto;
            this.nombre = nombre;
            this.precioReal = precioReal;
            this.stock = stock;
            this.total = total;
            this.idCategoria = idCategoria;
            


        }

        public Producto(string idProducto, string nombre, string stock, string precioReal, string precio, int idCategoria,DateTime fecha)
        {
            this.idProducto = idProducto;
            this.nombre = nombre;
            this.stock = stock;
            this.precioReal = precioReal;
            this.precio = precio;
            this.idCategoria = idCategoria;
            this.fecha = fecha;

        
        }
    }
}
