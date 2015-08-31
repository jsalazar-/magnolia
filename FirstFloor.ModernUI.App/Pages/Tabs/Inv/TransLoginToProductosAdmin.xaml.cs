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
using WpfPageTransitions;
using FirstFloor.ModernUI.App.Pages.Tabs.Inv;
namespace FirstFloor.ModernUI.App.Pages.Tabs.Inv
{
    /// <summary>
    /// Interaction logic for TransLoginToReporte.xaml
    /// </summary>
    public partial class TransLoginToProductosAdmin : UserControl
    {
         
        Productos p = new Productos();
        public TransLoginToProductosAdmin()
        {
            InitializeComponent();

            p.setContent(this);
            pageTransitionControl.ShowPage(p);

        }




        /* private void btnLogin_Click(object sender, RoutedEventArgs e)
         {
             //btn para retornar a login
             LoginAdmin nuevo = new LoginAdmin();
             nuevo.setLogin(this);
             pageTransitionControl.ShowPage(nuevo);
             btnLogin.Visibility = Visibility.Hidden;
             //contentMain.Content = new LoginAdmin();


         }*/




    }
}
