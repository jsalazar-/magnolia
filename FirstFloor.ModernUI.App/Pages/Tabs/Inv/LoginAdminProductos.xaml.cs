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
namespace FirstFloor.ModernUI.App.Pages.Tabs.Inv
{
    /// <summary>
    /// Interaction logic for LoginAdmin.xaml
    /// </summary>
    public partial class LoginAdminProductos : UserControl
    {

        vendedorFacade vendFac = new vendedorFacade();

        TransLoginToProductosAdmin trans;
        public LoginAdminProductos()
        {
            InitializeComponent();
            txtrut.Text = vendFac.getRutAdmin();
            //trans = l;
        }
        public void setLogin(TransLoginToProductosAdmin l)
        {
            trans = l;

        }
        public TransLoginToProductosAdmin getContent()
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

                TransLoginToProductosAdmin tl = getContent();

                //method1();
                //tl.btnLogin.Visibility = Visibility.Visible;
                ProductosAdmin prodAdmin = new ProductosAdmin();
                prodAdmin.setContent(tl);
                prodAdmin.ladmin.Content = "Administrador:" + vendFac.getNombreAdminByRut(txtrut.Text);
                tl.pageTransitionControl.ShowPage(prodAdmin);

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

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            Productos p = new Productos();
            p.setContent(trans);
            trans.pageTransitionControl.ShowPage(p);
        }
    }
}
