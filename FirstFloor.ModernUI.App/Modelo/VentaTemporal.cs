using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFloor.ModernUI.App.Modelo
{
    public class VentaTemporal
    {
        public Int64 idVenta { get; set; }
         public string idProducto { get; set; }
        public string nombre { get; set; }
        public string precio { get; set; }
        public string cantidad { get; set; }
        public string total { get; set; }
        public string devolver { get; set; }
        public VentaTemporal() { 

        }
        public VentaTemporal(Int64 idVenta, string idProducto, string nombre,  string precio,string cantidad, string total)
        {
            this.idVenta = idVenta;
            this.idProducto = idProducto;
            this.nombre = nombre;
            this.precio = precio;
            this.cantidad = cantidad;
            this.total = total;

        
        }
        public VentaTemporal(Int64 idVenta, string idProducto, string nombre, string precio, string cantidad,string devuelta, string total)
        {
            this.idVenta = idVenta;
            this.idProducto = idProducto;
            this.nombre = nombre;
            this.precio = precio;
            this.cantidad = cantidad;
            this.devolver = devuelta;
            this.total = total;


        }
    }
}
