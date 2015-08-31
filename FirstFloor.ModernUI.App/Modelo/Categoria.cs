using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFloor.ModernUI.App.Modelo
{
    public class Categoria
    {
        public int idCategoria { get; set; }
        public string nombreCategoria { get; set; }
        public DateTime fecha { get; set; }
        public Categoria(int idCategoria, string nombreCategoria,DateTime fecha) {
            this.idCategoria = idCategoria;
            this.nombreCategoria = nombreCategoria;
            this.fecha = fecha;
        
        }
    }
}
