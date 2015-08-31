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
using System.Windows.Shapes;
using FirstFloor.ModernUI.App.Modelo;
using FirstFloor.ModernUI.App.Control;
namespace FirstFloor.ModernUI.App.Pages.Tabs
{
    /// <summary>
    /// Interaction logic for DescuentoCliente.xaml
    /// </summary>
    public partial class DescuentoCliente : Window
    {
        List<Cliente> ListCliente = new List<Cliente>();
        CollectionViewSource itemCollectionViewSource;
        int idCliente = 0;
        string rutCliente = "";
        string nombreCliente = "";
        public DescuentoCliente(string v)
        {
            InitializeComponent();
            llenarTablaCliente();
            //MessageBox.Show(v);
        }
        public string getnombreCliente()
        {
            return nombreCliente;
        }
        public void setnombreCliente(string nombre)
        {
            nombreCliente = nombre;
        }
        public string getrut()
        {
            return rutCliente;
        }
        public void setrut(string rut)
        {
            rutCliente = rut;
        }
        public int getValor()
        {
            return idCliente;
        }
        public void setIdCliente(int id)
        {
            idCliente = id;

        }

        private void llenarTablaCliente()
        {

            clienteFacade prodF = new clienteFacade();

            var listaCliente = prodF.getClientes();

            if (listaCliente.Count > 0)
            {
                foreach (var item in listaCliente)
                {
                    ListCliente.Add(new Cliente { rut = item.rut, nombre = item.nombre, cantidadDescuento = item.cantidadDescuento, deuda = item.deuda, fechaUltimaCompra = item.fechaUltimaCompra, totalCompras = item.totalCompras });
                }

                itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSourceAllCliente"));
                itemCollectionViewSource.Source = ListCliente;

            }
            else
            {
                DateTime fvacio = Convert.ToDateTime("15/08/2008");
                ListCliente.Add(new Cliente { rut = "Sin Clientes", nombre = "", cantidadDescuento = "", deuda = 0,  totalCompras = 0 ,fechaUltimaCompra=fvacio});
                datagridCliente.ItemsSource = listaCliente;


            }
        }
        private void llenarTablaClientebyRut(string rut)
        {

            clienteFacade prodF = new clienteFacade();

            var listaCliente = prodF.getClientesbyRut(rut);

            ListCliente.Clear();
            datagridCliente.ItemsSource = null;
            if (listaCliente.Count > 0)
            {

                foreach (var item in listaCliente)
                {
                    ListCliente.Add(new Cliente { rut = item.rut, nombre = item.nombre, cantidadDescuento = item.cantidadDescuento, deuda = item.deuda, fechaUltimaCompra = item.fechaUltimaCompra });
                }

                datagridCliente.ItemsSource = ListCliente;

            }
            else
            {
                DateTime fvacio = Convert.ToDateTime("15/08/2008");
                ListCliente.Add(new Cliente { rut = "Cliente No encontrado", nombre = "", cantidadDescuento = "", deuda = 0,fechaUltimaCompra=fvacio});


                datagridCliente.ItemsSource = ListCliente;


            }
        }
        private void llenarTablaClientebyNombre(string nombre)
        {

            clienteFacade prodF = new clienteFacade();

            var listaCliente = prodF.getClientesbyNombre(nombre);
            ListCliente.Clear();
            datagridCliente.ItemsSource = null;
            if (listaCliente.Count > 0)
            {

                foreach (var item in listaCliente)
                {
                    ListCliente.Add(new Cliente { rut = item.rut, nombre = item.nombre, cantidadDescuento = item.cantidadDescuento, deuda = item.deuda, fechaUltimaCompra = item.fechaUltimaCompra });
                }

                datagridCliente.ItemsSource = ListCliente;

            }
            else
            {
                DateTime fvacia = Convert.ToDateTime("15/08/2008");
                ListCliente.Add(new Cliente { rut = "Cliente No encontrado", nombre = "", cantidadDescuento = "", deuda = 0, fechaUltimaCompra = fvacia });


                datagridCliente.ItemsSource = ListCliente;


            }
        }
        private void txtBuscarCliente_TextChanged(object sender, TextChangedEventArgs e)
        {

            clienteFacade cf = new clienteFacade();
            List<Cliente> listGetCliente = new List<Cliente>();

            listGetCliente = cf.getClientesbyNombre(txtBuscarCliente.Text);
            if (listGetCliente.Count > 0)
            {
                //listGetCliente = cf.getClientesbyNombre(txtBuscarCliente.Text);
                llenarTablaClientebyNombre(txtBuscarCliente.Text);
                //MessageBox.Show("Por nombre 0");
            }
            else
            {

                llenarTablaClientebyRut(txtBuscarCliente.Text);
                //MessageBox.Show("por rut  0");
            }
            //MessageBox.Show("buscar");

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {

            setIdCliente(0);
            this.Close();

            //itemCollectionViewSource.Source = ListCliente;
        }

        private void btnElegir_Click(object sender, RoutedEventArgs e)
        {
            if (datagridCliente.SelectedItem != null)
            {
                if (datagridCliente.SelectedItem is Cliente)
                {
                    var row = (Cliente)datagridCliente.SelectedItem;
                    if (row != null)
                    {
                        //MessageBox.Show(row.nombre);
                        //double des=Convert.ToDouble(row.cantidadDescuento)/Convert.ToDouble(100);
                        //MessageBox.Show(row.cantidadDescuento+"/100="+des.ToString());
                        if (row.rut.Equals("Cliente No encontrado"))
                        {
                            MessageBox.Show("Elegir algun cliente", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            setIdCliente(Convert.ToInt32(row.cantidadDescuento));
                            setrut(row.rut.ToString());
                            setnombreCliente(row.nombre);
                            this.Close();
                        }
                    }
                }
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
