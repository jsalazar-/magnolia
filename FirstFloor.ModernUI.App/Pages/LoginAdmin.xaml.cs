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
namespace FirstFloor.ModernUI.App.Pages
{
    /// <summary>
    /// Interaction logic for LoginAdmin.xaml
    /// </summary>
    public partial class LoginAdmin : UserControl
    {
        
        vendedorFacade vendFac = new vendedorFacade();

        TransLoginToReporte trans;
        public LoginAdmin()
        {
            InitializeComponent();
            txtrut.Text = vendFac.getRutAdmin();
            //trans = l;
        }
        public void setLogin(TransLoginToReporte l)
        {
            trans = l;

        }
        public TransLoginToReporte getContent()
        {
            return trans;
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string rut = txtrut.Text;
            string pass = txtpass.Password;
            //Verificar usuario
            vendedorFacade vendFac = new vendedorFacade();
            string passAdmin = vendFac.getpassbyRut(rut);

            byte[] data = Encoding.ASCII.GetBytes(pass);
            SHA1 algorithm = SHA1.Create();
            byte[] hash = algorithm.ComputeHash(data);
            StringBuilder sb = new StringBuilder();
            //string base64 = Convert.ToBase64String(hash);
            /*for (int i = 0; i < hash.Length; i++)
            {
                sb.AppendFormat("{x2}", hash[i]);
            }*/
            foreach (Byte b in hash)
                sb.Append(b.ToString("x2"));

            if (passAdmin.Equals(sb.ToString()))
            {
                //ir a pagina reporte 
                
                TransLoginToReporte tl = getContent();
                
                //method1();
                //tl.btnLogin.Visibility = Visibility.Visible;
                Reportes reporte = new Reportes();
                reporte.setContent(tl);
                reporte.ladmin.Content = "Administrador:"+vendFac.getNombreAdminByRut(txtrut.Text);
                tl.pageTransitionControl.ShowPage(reporte);

                //method1();


            }
            else
            {
                lmsg.Content = "Administrador no existe";
               /* System.Threading.Thread.Sleep(600);
                Microsoft.VisualBasic.Interaction.AppActivate(
                     System.Diagnostics.Process.GetCurrentProcess().Id);
                System.Windows.Forms.SendKeys.SendWait(" ");
                lmsg.Content = "";*/

            }

        }
    }
}
