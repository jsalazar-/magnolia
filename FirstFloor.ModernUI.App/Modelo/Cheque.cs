using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFloor.ModernUI.App.Modelo
{
    public class Cheque
    {
         public string rut { get; set; }
         public string nombre { get; set; }
         public string nombreBanco { get; set; }
         public string fechaemision { get; set; }
         public string fechaexpiracion { get; set; }
         public string fechaVenta { get; set; }
        
         public string monto { get; set; }

        public Cheque() { }
        public Cheque(string rut,string nombre, string nombrebanco,string fechaemision,string fechaexpiracion,string fechaVenta,string monto) 
        {
            this.rut = rut;
            this.nombre = nombre;
            this.nombreBanco = nombrebanco;
            this.fechaemision = fechaemision;
            this.fechaexpiracion = fechaexpiracion;
            this.fechaVenta = fechaVenta;
            this.monto = monto;
        }


        
    }
}
