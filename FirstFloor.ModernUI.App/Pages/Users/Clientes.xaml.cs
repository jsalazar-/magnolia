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
using System.Text.RegularExpressions;
using FirstFloor.ModernUI.App.Modelo;
using FirstFloor.ModernUI.App.Control;

namespace FirstFloor.ModernUI.App.Pages.Users
{
    /// <summary>
    /// Interaction logic for Clientes.xaml
    /// </summary>
    public partial class Clientes : UserControl
    {
        List<Cliente> ListCliente = new List<Cliente>();
        CollectionViewSource itemCollectionViewSource;
        public Clientes()
        {
            InitializeComponent();
            llenarTablaCliente();
        }

        public bool validarRut(string rut)
        {

            bool validacion = false;
            try
            {
                rut = rut.ToUpper();
                rut = rut.Replace(".", "");
                rut = rut.Replace("-", "");
                int rutAux = int.Parse(rut.Substring(0, rut.Length - 1));

                char dv = char.Parse(rut.Substring(rut.Length - 1, 1));

                int m = 0, s = 1;
                for (; rutAux != 0; rutAux /= 10)
                {
                    s = (s + rutAux % 10 * (9 - m++ % 6)) % 11;
                }
                if (dv == (char)(s != 0 ? s + 47 : 75))
                {
                    validacion = true;
                }
            }
            catch (Exception)
            {
            }
            return validacion;
        }

        private void textbox_NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        //############################################################
        //################           CLIENTE       ##################
        //############################################################
        private void llenarTablaCliente()
        {

            clienteFacade prodF = new clienteFacade();

            var listaCliente = prodF.getClientes();
            ListCliente.Clear();
            if (listaCliente.Count > 0)
            {
                foreach (var item in listaCliente)
                {
                    ListCliente.Add(new Cliente { rut = item.rut, nombre = item.nombre, cantidadDescuento = item.cantidadDescuento, deuda = item.deuda, fechaUltimaCompra = item.fechaUltimaCompra, totalCompras = item.totalCompras });
                }

                itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSourceAllCliente"));
                //itemCollectionViewSource.Source = ListCliente;
                dtgridCliente.ItemsSource = listaCliente;

            }
            else
            {
                DateTime fvacio = Convert.ToDateTime("15/08/2008");
                ListCliente.Add(new Cliente { rut = "Sin Clientes", nombre = "", cantidadDescuento = "", deuda = 0, totalCompras = 0, fechaUltimaCompra = fvacio });
                dtgridCliente.ItemsSource = listaCliente;


            }
        }

        private void btnguardarCliente_Click(object sender, RoutedEventArgs e)
        {
            string rut = txtrut.Text;
            string nombre = txtNombre.Text;
            string descuento = txtDescuento.Text;
            if (string.IsNullOrWhiteSpace(rut))
            {
                MessageBox.Show("Ingresar Rut", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingresar Nombre", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(descuento))
            {
                MessageBox.Show("Ingresar Cantidad de Descuento", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                bool valRut = validarRut(txtrut.Text);
                if (valRut)
                {
                    if (Convert.ToInt32(descuento) > 100)
                    {
                        MessageBox.Show("Descuento asociado a cliente es invalido.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    else if (Convert.ToInt32(descuento) == 100)
                    {
                        //Guardar Cliente
                        if (MessageBox.Show("Esta seguro de ingresar cliente FREE(no tendra costo en compra por descuento de 100%)? ", "Cliente FREE", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            clienteFacade clienteF = new clienteFacade();
                            //Verificar si existe cliente

                            bool existeCliente = clienteF.getExisteCliente(rut);

                            if (existeCliente)
                            {
                                MessageBox.Show("Cliente con rut:" + rut + " ya existe", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                //Guardar Cliente

                                DateTime fvacio = Convert.ToDateTime("15/08/2008");
                                string res = clienteF.GuardarClientes(rut, nombre, descuento, "", fvacio, 0);

                                if (res.Equals(""))
                                {
                                    MessageBox.Show("Cliente guardado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                    llenarTablaCliente();
                                    txtrut.Text = "";
                                    txtNombre.Text = "";
                                    txtDescuento.Text = "";
                                }
                                else
                                {
                                    MessageBox.Show("Error al guardar cliente" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }

                        }
                    }
                    else
                    {

                        clienteFacade clienteF = new clienteFacade();
                        //Verificar si existe cliente

                        bool existeCliente = clienteF.getExisteCliente(rut);

                        if (existeCliente)
                        {
                            MessageBox.Show("Cliente con rut:" + rut + " ya existe", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            //Guardar Cliente

                            DateTime fvacio = Convert.ToDateTime("15/08/2008");
                            string res = clienteF.GuardarClientes(rut, nombre, descuento, "", fvacio, 0);

                            if (res.Equals(""))
                            {
                                MessageBox.Show("Cliente guardado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                llenarTablaCliente();
                                txtrut.Text = "";
                                txtNombre.Text = "";
                                txtDescuento.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("Error al guardar cliente" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Rut invalido asociado a cliente", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void btnGuardarActualizarCliente_Click(object sender, RoutedEventArgs e)
        {
            //if (dtgridCliente.SelectedItem != null)
            //{

            string rut = txtrut.Text;
            string nombre = txtNombre.Text;
            string descuento = txtDescuento.Text;

            clienteFacade cl = new clienteFacade();
            if (string.IsNullOrWhiteSpace(txtrut.Text))
            {
                MessageBox.Show("Ingresar Rut", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingresar Nombre", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(txtDescuento.Text))
            {
                MessageBox.Show("Ingresar Cantidad de Descuento", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else
            {
                //verificar ingreso deuda

                if (cl.getDeudaCliente(rut) > 0)
                {
                    if (string.IsNullOrWhiteSpace(txtDeuda.Text))
                    {
                        MessageBox.Show("Deuda debe estar en 0 o segun corresponda", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        if (Convert.ToInt32(descuento) > 100)
                        {
                            MessageBox.Show("Descuento asociado a cliente es invalido.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);

                        }
                        else if (Convert.ToInt32(descuento) == 100)
                        {
                            //Guardar Cliente
                            if (MessageBox.Show("Esta seguro de ingresar cliente FREE(no tendra costo en compra por descuento de 100%)? ", "Cliente FREE", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {



                                string res = "";
                                double deuda = Convert.ToDouble(txtDeuda.Text);
                                clienteFacade clienteF = new clienteFacade();
                                res = clienteF.actualizarCliente(rut, nombre, descuento, deuda);


                                if (res.Equals(""))
                                {
                                    MessageBox.Show("Cliente actualizado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                    txtrut.Text = "";
                                    txtNombre.Text = "";
                                    txtDescuento.Text = "";
                                    btnguardarCliente.Visibility = Visibility.Visible;
                                    btncancelarEditarCliente.Visibility = Visibility.Hidden;
                                    btnGuardarActualizarCliente.Visibility = Visibility.Hidden;
                                    txtrut.IsEnabled = true;
                                    ldeuda.Visibility = Visibility.Hidden;
                                    txtDeuda.Visibility = Visibility.Hidden;
                                    llenarTablaCliente();


                                }
                                else
                                {
                                    MessageBox.Show("Error al actualizar cliente" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }

                        }
                        else
                        {
                            string res = "";
                            double deuda = Convert.ToDouble(txtDeuda.Text);
                            clienteFacade clienteF = new clienteFacade();
                            res = clienteF.actualizarCliente(rut, nombre, descuento, deuda);



                            if (res.Equals(""))
                            {
                                MessageBox.Show("Cliente actualizado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                txtrut.Text = "";
                                txtNombre.Text = "";
                                txtDescuento.Text = "";
                                btnguardarCliente.Visibility = Visibility.Visible;
                                btncancelarEditarCliente.Visibility = Visibility.Hidden;
                                btnGuardarActualizarCliente.Visibility = Visibility.Hidden;
                                ldeuda.Visibility = Visibility.Hidden;
                                txtDeuda.Visibility = Visibility.Hidden;
                                txtrut.IsEnabled = true;

                                llenarTablaCliente();


                            }
                            else
                            {
                                MessageBox.Show("Error al actualizar cliente" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                        }
                    }
                }
                else
                {
                    if (Convert.ToInt32(descuento) > 100)
                    {
                        MessageBox.Show("Descuento asociado a cliente es invalido.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    else if (Convert.ToInt32(descuento) == 100)
                    {
                        //Guardar Cliente
                        if (MessageBox.Show("Esta seguro de ingresar cliente FREE(no tendra costo en compra por descuento de 100%)? ", "Cliente FREE", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {



                            string res = "";

                            clienteFacade clienteF = new clienteFacade();
                            res = clienteF.actualizarCliente(rut, nombre, descuento, 0);



                            if (res.Equals(""))
                            {
                                MessageBox.Show("Cliente actualizado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                txtrut.Text = "";
                                txtNombre.Text = "";
                                txtDescuento.Text = "";
                                btnguardarCliente.Visibility = Visibility.Visible;
                                btncancelarEditarCliente.Visibility = Visibility.Hidden;
                                btnGuardarActualizarCliente.Visibility = Visibility.Hidden;
                                txtrut.IsEnabled = true;

                                llenarTablaCliente();


                            }
                            else
                            {
                                MessageBox.Show("Error al actualizar cliente" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                    else
                    {

                        string res = "";

                        clienteFacade clienteF = new clienteFacade();
                        res = clienteF.actualizarCliente(rut, nombre, descuento, 0);



                        if (res.Equals(""))
                        {
                            MessageBox.Show("Cliente actualizado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                            txtrut.Text = "";
                            txtNombre.Text = "";
                            txtDescuento.Text = "";
                            btnguardarCliente.Visibility = Visibility.Visible;
                            btncancelarEditarCliente.Visibility = Visibility.Hidden;
                            btnGuardarActualizarCliente.Visibility = Visibility.Hidden;
                            txtrut.IsEnabled = true;

                            llenarTablaCliente();


                        }
                        else
                        {
                            MessageBox.Show("Error al actualizar cliente" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }




            /*}
            else
            {
                MessageBox.Show("Seleccionar un cliente para actualizar");
            }*/

        }
        private void btnActualizarCliente_Click(object sender, RoutedEventArgs e)
        {

            //cargar Clientes para edicion
            if (dtgridCliente.SelectedItem != null)
            {
                if (dtgridCliente.SelectedItem is Cliente)
                {
                    var row = (Cliente)dtgridCliente.SelectedItem;

                    if (row != null)
                    {
                        //si deuda es mayor a 0 mostrar campo para actualizar deuda
                        clienteFacade cl = new clienteFacade();
                        double deuda = cl.getDeudaCliente(row.rut);
                        if (deuda == 0)
                        {
                            btnguardarCliente.Visibility = Visibility.Hidden;
                            btncancelarEditarCliente.Visibility = Visibility.Visible;
                            btnGuardarActualizarCliente.Visibility = Visibility.Visible;
                            txtrut.Text = row.rut;
                            txtrut.IsEnabled = false;
                            txtNombre.Text = row.nombre;
                            txtDescuento.Text = row.cantidadDescuento;
                        }
                        else
                        {
                            btnguardarCliente.Visibility = Visibility.Hidden;
                            btncancelarEditarCliente.Visibility = Visibility.Visible;
                            btnGuardarActualizarCliente.Visibility = Visibility.Visible;
                            txtrut.Text = row.rut;
                            txtrut.IsEnabled = false;
                            txtNombre.Text = row.nombre;
                            txtDescuento.Text = row.cantidadDescuento;
                            ldeuda.Visibility = Visibility.Visible;
                            txtDeuda.Visibility = Visibility.Visible;
                            txtDeuda.Text = deuda.ToString();
                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Seleccionar un cliente para actualizar", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btncancelarEditarCliente_Click(object sender, RoutedEventArgs e)
        {
            btnguardarCliente.Visibility = Visibility.Visible;
            btncancelarEditarCliente.Visibility = Visibility.Hidden;
            btnGuardarActualizarCliente.Visibility = Visibility.Hidden;
            ldeuda.Visibility = Visibility.Hidden;
            txtDeuda.Visibility = Visibility.Hidden;
            if (dtgridCliente.SelectedItem != null)
            {
                if (dtgridCliente.SelectedItem is Cliente)
                {
                    var row = (Cliente)dtgridCliente.SelectedItem;

                    if (row != null)
                    {
                        txtrut.Text = "";
                        txtNombre.Text = "";
                        txtDescuento.Text = "";
                        txtrut.IsEnabled = true;
                    }
                }
            }

        }


        private void btnEliminarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (dtgridCliente.SelectedItem != null)
            {
                if (dtgridCliente.SelectedItem is Cliente)
                {
                    var row = (Cliente)dtgridCliente.SelectedItem;

                    if (row != null)
                    {
                        clienteFacade clientF = new clienteFacade();
                        string res = clientF.borrarClienteByRut(row.rut);

                        if (res.Equals(""))
                        {
                            MessageBox.Show("Cliente borrado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                            txtrut.Text = "";
                            txtNombre.Text = "";
                            txtDescuento.Text = "";
                            btnguardarCliente.Visibility = Visibility.Visible;
                            btncancelarEditarCliente.Visibility = Visibility.Hidden;
                            btnGuardarActualizarCliente.Visibility = Visibility.Hidden;
                            ldeuda.Visibility = Visibility.Hidden;
                            txtDeuda.Visibility = Visibility.Hidden;
                            llenarTablaCliente();
                            llenarTablaCliente();
                            txtrut.IsEnabled = true;

                        }
                        else
                        {
                            MessageBox.Show("Error al borrar cliente" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccionar un cliente para eliminar", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void btnEliminarTodosClientes_Click(object sender, RoutedEventArgs e)
        {
            clienteFacade prodFtotal = new clienteFacade();
            int totalProd = prodFtotal.getTotalClientes();
            if (MessageBox.Show("Esta seguro de borrar " + totalProd.ToString() + " clientes ", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                clienteFacade prodF = new clienteFacade();
                string res = prodF.borrarAllCliente();

                if (res.Equals(""))
                {
                    MessageBox.Show(totalProd + " clientes eliminados", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                    llenarTablaCliente();
                    txtrut.Text = "";
                    txtNombre.Text = "";
                    txtDescuento.Text = "";
                    btnguardarCliente.Visibility = Visibility.Visible;
                    btncancelarEditarCliente.Visibility = Visibility.Hidden;
                    btnGuardarActualizarCliente.Visibility = Visibility.Hidden;
                    ldeuda.Visibility = Visibility.Hidden;
                    txtDeuda.Visibility = Visibility.Hidden;
                    llenarTablaCliente();
                    llenarTablaCliente();
                    txtrut.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Error al borrar todos los clientes:" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        private void llenarTablaClientebyNombre(string nombre)
        {

            clienteFacade prodF = new clienteFacade();

            var listaCliente = prodF.getClientesbyNombre(nombre);
            ListCliente.Clear();
            dtgridCliente.ItemsSource = null;
            if (listaCliente.Count > 0)
            {

                foreach (var item in listaCliente)
                {
                    ListCliente.Add(new Cliente { rut = item.rut, nombre = item.nombre, cantidadDescuento = item.cantidadDescuento, deuda = item.deuda, fechaUltimaCompra = item.fechaUltimaCompra });
                }

                dtgridCliente.ItemsSource = ListCliente;

            }
            else
            {
                DateTime fvacia = Convert.ToDateTime("15/08/2008");
                ListCliente.Add(new Cliente { rut = "Cliente No encontrado", nombre = "", cantidadDescuento = "", deuda = 0, fechaUltimaCompra = fvacia });


                dtgridCliente.ItemsSource = ListCliente;


            }
        }
        private void llenarTablaClientebyRut(string rut)
        {

            clienteFacade prodF = new clienteFacade();

            var listaCliente = prodF.getClientesbyRut(rut);

            ListCliente.Clear();
            dtgridCliente.ItemsSource = null;
            if (listaCliente.Count > 0)
            {

                foreach (var item in listaCliente)
                {
                    ListCliente.Add(new Cliente { rut = item.rut, nombre = item.nombre, cantidadDescuento = item.cantidadDescuento, deuda = item.deuda, fechaUltimaCompra = item.fechaUltimaCompra });
                }

                dtgridCliente.ItemsSource = ListCliente;

            }
            else
            {
                DateTime fvacio = Convert.ToDateTime("15/08/2008");
                ListCliente.Add(new Cliente { rut = "Cliente No encontrado", nombre = "", cantidadDescuento = "", deuda = 0, fechaUltimaCompra = fvacio });


                dtgridCliente.ItemsSource = ListCliente;


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

        private void txtrut_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
            e.Handled = ascci >= 48 && ascci <= 57 ? false : true;
        }

        private void btnreload_Click(object sender, RoutedEventArgs e)
        {
            llenarTablaCliente();
        }

    }
}
