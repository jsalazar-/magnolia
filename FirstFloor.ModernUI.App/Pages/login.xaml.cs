using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.App.Control;
using FirstFloor.ModernUI.App.Modelo;
using FirstFloor.ModernUI.App.Pages.Tabs;
using FirstFloor.ModernUI.App.Pages;
using System.Security.Cryptography;

namespace FirstFloor.ModernUI.App
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : UserControl
    {
        TransLoginToVenta transVenta;
        string rut = "";
        public login()
        {
            InitializeComponent();
              
        }

        public void setInstancia(TransLoginToVenta tr)
        {
            transVenta = tr;
        }
        public TransLoginToVenta getInstancia()
        {
            return transVenta;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string rut = txtrut.Text;
            
            //Verificar usuario
            vendedorFacade vendFac = new vendedorFacade();
            bool passVendedor = vendFac.getExisteVendedor(rut);

            

            //if (passAdmin.Equals(sb.ToString()))
            if (passVendedor)
            {
                //ir a pagina venta
                TransLoginToVenta tl = new TransLoginToVenta();
                //lmsg.Content = "Ir a ventas";
                TransLoginToVenta transVenta = getInstancia();

                //method1();
                //tl.btnLogin.Visibility = Visibility.Visible;
                Ventas toventas = new Ventas(rut);
                toventas.setInstancia(transVenta);
                
                toventas.setRut(rut);
                DateTime fechaactual = DateTime.Now.Date;
                string fecha = fechaactual.ToString("d");
                vendFac.actualizarUltimaFechaIngrVend(rut, fecha);
                //toventas.ladmin.Content = "Administrador:" + vendFac.getNombreAdminByRut(txtrut.Text);
                transVenta.pageTransitionControl.ShowPage(toventas);


                
            }
            else
            {
                lmsg.Content = "Vendedor no existe";
                /*System.Threading.Thread.Sleep(600);
                Microsoft.VisualBasic.Interaction.AppActivate(
                     System.Diagnostics.Process.GetCurrentProcess().Id);
                System.Windows.Forms.SendKeys.SendWait(" ");
                lmsg.Content = "";*/

            }

        }

    
    }
}
