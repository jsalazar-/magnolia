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
using  FirstFloor.ModernUI.App.Pages.Tabs;
namespace FirstFloor.ModernUI.App.Pages
{
    /// <summary>
    /// Interaction logic for TransLoginToReporte.xaml
    /// </summary>
    public partial class TransLoginToReporte : UserControl
    {
        public TransLoginToReporte MainPage { get; private set; }

        public delegate void ValuePassDelegate();
        
        LoginAdmin l=new LoginAdmin();
        public TransLoginToReporte()
        {
            InitializeComponent();
            
            l.setLogin(this);
            pageTransitionControl.ShowPage(l);
            
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
