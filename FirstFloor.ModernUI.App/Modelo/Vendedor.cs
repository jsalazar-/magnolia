using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFloor.ModernUI.App.Modelo
{
    class Vendedor
    {

        public string rut { get; set; }
        public string nombre { get; set; }
        public string fechaIngresoTrabajar { get; set; }
        public string fechaUltimoAcceso { get; set; }
        public int totalVentas { get; set; }
        public int tipo { get; set; }
        public string contrasena { get; set; }
        public Vendedor() { }
        public Vendedor(string rut, string nombre, string fechaIngresoTrabajar,string fechaUltimoAcceso, int totalVentas,int tipo,string contrasena)
        {
            this.rut = rut;
            this.nombre = nombre;
            this.fechaIngresoTrabajar = fechaIngresoTrabajar;
            this.fechaUltimoAcceso = fechaUltimoAcceso;
            this.totalVentas = totalVentas;
            this.tipo = tipo;
            this.contrasena = contrasena;
        }
        
    }
}
