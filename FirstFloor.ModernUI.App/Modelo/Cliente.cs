using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFloor.ModernUI.App.Modelo
{
    class Cliente
    {
        
        public string rut { get; set; }
        public string nombre { get; set; }
        public string cantidadDescuento { get; set; }
        public double deuda { get; set; }
        public DateTime fechaUltimaCompra { get; set; }
        public int totalCompras { get; set; }
        

        public Cliente() { }
        public Cliente(string rut, string nombre, string cantidadDescuento, double deuda, DateTime fechaUltimaCompra, int totalCompras)
        {
            
            this.rut = rut;
            this.nombre = nombre;
            this.cantidadDescuento = cantidadDescuento;
            this.deuda = deuda;
            this.fechaUltimaCompra = fechaUltimaCompra;
            this.totalCompras = totalCompras;

        
        }
    }
}
