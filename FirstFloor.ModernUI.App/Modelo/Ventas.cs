using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFloor.ModernUI.App.Modelo
{
    class MVentas
    {
        public Int64 idVenta { get; set; }
        public string idProducto { get; set; }
        public string nombreProducto { get; set; }
        public string rutCliente { get; set; }
        public string rutVendedor { get; set; }
        public int cantidad { get; set; }
        public double total { get; set; }
        public DateTime fecha { get; set; }
        public string tipoVenta { get; set; }
        public int idCategoria { get; set; }
        public string precioReal { get; set; }

        public MVentas() { }
        //Para reporte, recibir consulta de productos agrupados por fecha 

        public MVentas(DateTime fecha, string idProducto, string nombre, string precioReal, int cantidad, double total, int idCategoria)
        {
            this.fecha = fecha;
            this.idProducto = idProducto;
            this.nombreProducto = nombre;
            this.precioReal = precioReal;
            this.cantidad = cantidad;
            this.total = total;
            this.idCategoria = idCategoria;

        }

        public MVentas(DateTime fecha, string idProducto, string nombre, int cantidad, double total, int idCategoria)
        {
            this.fecha = fecha;
            this.idProducto = idProducto;
            this.nombreProducto = nombre;
            this.cantidad = cantidad;
            this.total = total;
            this.idCategoria = idCategoria;

        }

        public MVentas(Int64 idVenta, string idProducto, string rutCliente, string rutVendedor, int cantidad, double total, DateTime fecha, string tipoVenta)
        {
            this.idVenta = idVenta;
            this.idProducto = idProducto;
            this.rutCliente = rutCliente;
            this.rutVendedor = rutVendedor;
            this.cantidad = cantidad;
            this.total = total;
            this.fecha = fecha;
            this.tipoVenta = tipoVenta;
        }

    }
}
