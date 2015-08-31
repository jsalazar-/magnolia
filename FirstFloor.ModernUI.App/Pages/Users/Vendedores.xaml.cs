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
using Microsoft.VisualBasic;
using System.Security.Cryptography;

namespace FirstFloor.ModernUI.App.Pages.Users
{
    /// <summary>
    /// Interaction logic for Registro.xaml
    /// </summary>
    public partial class Vendedores : UserControl
    {
        public Vendedores()
        {
            InitializeComponent();
            llenarTablaVendedor();
            txtfechaIngreso.DisplayDateEnd = DateTime.Today;
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
        //################           Vendedor       ##################
        //############################################################

        private void llenarTablaVendedor()
        {

            vendedorFacade prodF = new vendedorFacade();
            var itemList = new List<Vendedor>();
            var listaVendedor = prodF.getVendedor();

            if (listaVendedor.Count > 0)
            {
                foreach (var item in listaVendedor)
                {
                    if (item.tipo == 1)
                    {
                        itemList.Add(new Vendedor { rut = item.rut, nombre = item.nombre, fechaIngresoTrabajar = item.fechaIngresoTrabajar, fechaUltimoAcceso = item.fechaUltimoAcceso, totalVentas = item.totalVentas, contrasena = "Admin" });
                    }
                    else
                    {
                        itemList.Add(new Vendedor { rut = item.rut, nombre = item.nombre, fechaIngresoTrabajar = item.fechaIngresoTrabajar, fechaUltimoAcceso = item.fechaUltimoAcceso, totalVentas = item.totalVentas, contrasena = "" });
                    }
                }

                CollectionViewSource itemCollectionViewSource;
                itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSourceAllVendedor"));
                itemCollectionViewSource.Source = itemList;

                btnEliminarVendedor.IsEnabled = true;
                btnActualizarVendedor.IsEnabled = true;
            }
            else
            {
                itemList.Add(new Vendedor { rut = "Sin Vendedores", nombre = "", fechaIngresoTrabajar = "", fechaUltimoAcceso = "", totalVentas = 0 });


                CollectionViewSource itemCollectionViewSource;
                itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSourceAllVendedor"));
                itemCollectionViewSource.Source = itemList;
                btnEliminarVendedor.IsEnabled = false;
                btnActualizarVendedor.IsEnabled = false;


            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;

            DateTime? date = picker.SelectedDate;
            if (date == null)
            {
            }
            else
            {

            }
        }

        private void btnguardarVendedor_Click(object sender, RoutedEventArgs e)
        {


            string rut = txtrutvendedor.Text;
            string nombre = txtNombreVendedor.Text;
            //string fechaingreso = txtfechaIngreso.Text;
            DateTime? date = txtfechaIngreso.SelectedDate;
            if (string.IsNullOrWhiteSpace(rut))
            {
                MessageBox.Show("Ingresar Rut", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingresar Nombre", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else
            {
                bool rutVal = validarRut(txtrutvendedor.Text);
                if (rutVal)
                {
                    vendedorFacade vendFac = new vendedorFacade();
                    bool existe = vendFac.getExistAdmin();

                    if (rdbtnAdmin.IsChecked == true)
                    {
                        if (existe)
                        {
                            MessageBox.Show("Ya existe 1 Administrador", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            rdbtnAdmin.IsChecked = false;
                            rdbtnvendedor.IsChecked = true;
                        }
                        else
                        {

                            if (String.IsNullOrWhiteSpace(txtContraseña.Password))
                            {
                                MessageBox.Show("Ingresar Contraseña", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                //verificar si rut ya esta en bd
                                vendedorFacade clienteF = new vendedorFacade();

                                bool existeVendedor = clienteF.getExisteVendedor(rut);
                                if (existeVendedor)
                                {
                                    MessageBox.Show("Vendedor ya existe", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                                else
                                {
                                    //Administrador Cliente
                                    DateTime fechaactual = DateTime.Now.Date;
                                    string fecha = fechaactual.ToString("d");

                                    string contrasena = txtContraseña.Password;

                                    string res = clienteF.GuardarVendedor(rut, nombre, fecha, "", 0, 1, contrasena.Trim());

                                    if (res.Equals(""))
                                    {
                                        MessageBox.Show("Administrador guardado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                        llenarTablaVendedor();
                                        txtrutvendedor.Text = "";
                                        txtNombreVendedor.Text = "";
                                        txtfechaIngreso.Text = "";
                                        lfechaingreso.Visibility = System.Windows.Visibility.Visible;
                                        txtfechaIngreso.Visibility = Visibility.Visible;
                                        lcontraseña.Visibility = Visibility.Hidden;
                                        txtContraseña.Visibility = Visibility.Hidden;
                                        rdbtnvendedor.IsChecked = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error al guardar vendedor" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        /*if (rdbtnvendedor.IsChecked == true)
                        {*/
                        if (date == null)
                        {
                            MessageBox.Show("Ingresar Fecha Ingreso", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            //Guardar Cliente
                            vendedorFacade clienteF = new vendedorFacade();

                            bool existeVendedor = clienteF.getExisteVendedor(rut);
                            if (existeVendedor)
                            {
                                MessageBox.Show("Vendedor ya existe", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            else
                            {

                                string fecha = date.Value.ToString("d");
                                DateTime FechAc = DateTime.Now.Date;


                                string res = clienteF.GuardarVendedor(rut, nombre, fecha, "", 0, 0, "");

                                if (res.Equals(""))
                                {
                                    MessageBox.Show("Vendedor guardado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                    llenarTablaVendedor();
                                    txtrutvendedor.Text = "";
                                    txtNombreVendedor.Text = "";
                                    txtfechaIngreso.Text = "";
                                    lfechaingreso.Visibility = System.Windows.Visibility.Visible;
                                    txtfechaIngreso.Visibility = Visibility.Visible;
                                    lcontraseña.Visibility = Visibility.Hidden;
                                    txtContraseña.Visibility = Visibility.Hidden;
                                    rdbtnvendedor.IsChecked = true;
                                }
                                else
                                {
                                    MessageBox.Show("Error al guardar vendedor" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }

                    }

                }
                else
                {
                    MessageBox.Show("Rut invalido asociado a vendedor", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }

        }
        private void btnGuardarActualizarVendedor_Click(object sender, RoutedEventArgs e)
        {
            string rut = txtrutvendedor.Text;
            string nombre = txtNombreVendedor.Text;
            string fechaingreso = txtfechaIngreso.Text;
            if (string.IsNullOrWhiteSpace(rut))
            {
                MessageBox.Show("Ingresar Rut", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Ingresar Nombre", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                vendedorFacade vendFac = new vendedorFacade();
                bool existe = vendFac.getExistAdmin();
                if (rdbtnAdmin.IsChecked == true)
                {


                    if (String.IsNullOrWhiteSpace(txtContraseña.Password))
                    {
                        MessageBox.Show("Ingresar Contraseña", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        //verificar si rut ya esta en bd
                        vendedorFacade clienteF = new vendedorFacade();


                        //Administrador Cliente

                        string contrasena = txtContraseña.Password;

                        //MessageBox.Show(contrasena);

                        string passAdmin = clienteF.getpassbyRut(rut);
                        //                        MessageBox.Show(passAdmin);
                        ControlPass ctrlpass = new ControlPass();
                        ctrlpass.ShowDialog();
                        string passingresada = ctrlpass.getpass();

                        byte[] data = Encoding.ASCII.GetBytes(passingresada);
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



                        SHA1 sha = new SHA1CryptoServiceProvider();
                        ASCIIEncoding encoder = new ASCIIEncoding();
                        byte[] combined = encoder.GetBytes(passAdmin);
                        string hashi = BitConverter.ToString(sha.ComputeHash(combined)).Replace("-", "");

                        //MessageBox.Show(sb.ToString() + "==" + passAdmin.ToString());
                        if (passAdmin.Equals(sb.ToString()))
                        {
                            string contra = txtContraseña.Password;
                            string res = clienteF.actualizarAdmin(rut, nombre, contra.Trim());

                            if (res.Equals(""))
                            {
                                MessageBox.Show("Administrador Actualizado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                llenarTablaVendedor();
                                txtrutvendedor.Text = "";
                                txtNombreVendedor.Text = "";
                                txtfechaIngreso.Text = "";
                                lfechaingreso.Visibility = System.Windows.Visibility.Hidden;
                                txtfechaIngreso.Visibility = System.Windows.Visibility.Hidden;
                                rdbtnvendedor.Visibility = Visibility.Visible;
                                lcontraseña.Visibility = System.Windows.Visibility.Visible;
                                txtContraseña.Visibility = System.Windows.Visibility.Visible;
                                rdbtnvendedor.IsChecked = true;
                                btnguardarVendedor.Visibility = Visibility.Visible;
                                btncancelarEditarVendedor.Visibility = Visibility.Hidden;
                                btnGuardarActualizarVendedor.Visibility = Visibility.Hidden;

                                txtContraseña.Password = "";
                                lcontraseña.Content = "Contraseña";
                                txtrutvendedor.IsEnabled = true;

                            }
                            else
                            {
                                MessageBox.Show("Error al actualizar administrador" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                        }
                        else if (!string.IsNullOrWhiteSpace(passingresada))
                        {
                            MessageBox.Show("Contraseña incorrecta", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);

                        }



                    }
                }
                else
                {
                    /*if (rdbtnvendedor.IsChecked == true)
                    {*/
                    if (fechaingreso == null)
                    {
                        MessageBox.Show("Ingresar Fecha Ingreso", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        if (rdbtnAdmin.IsChecked == true)
                        {
                            if (existe)
                            {
                                MessageBox.Show("Ya existe 1 Administrador", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                rdbtnAdmin.IsChecked = false;
                                rdbtnvendedor.IsChecked = true;
                            }
                            else
                            {
                                //Guardar Cliente
                                vendedorFacade clienteF = new vendedorFacade();


                                string res = clienteF.actualizarVendedor(rut, nombre, fechaingreso);

                                if (res.Equals(""))
                                {
                                    MessageBox.Show("Vendedor Actualizado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                    llenarTablaVendedor();
                                    txtrutvendedor.Text = "";
                                    txtNombreVendedor.Text = "";
                                    txtfechaIngreso.Text = "";
                                    lfechaingreso.Visibility = System.Windows.Visibility.Visible;
                                    txtfechaIngreso.Visibility = Visibility.Visible;
                                    lcontraseña.Visibility = Visibility.Hidden;
                                    txtContraseña.Visibility = Visibility.Hidden;
                                    rdbtnvendedor.IsChecked = true;
                                    btnguardarVendedor.Visibility = Visibility.Visible;
                                    btncancelarEditarVendedor.Visibility = Visibility.Hidden;
                                    btnGuardarActualizarVendedor.Visibility = Visibility.Hidden;
                                    txtrutvendedor.IsEnabled = true;

                                }
                                else
                                {
                                    MessageBox.Show("Error al guardar vendedor" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }
                        else
                        {
                            //Guardar Cliente
                            vendedorFacade clienteF = new vendedorFacade();


                            string res = clienteF.actualizarVendedor(rut, nombre, fechaingreso);

                            if (res.Equals(""))
                            {
                                MessageBox.Show("Vendedor Actualizado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                llenarTablaVendedor();
                                txtrutvendedor.Text = "";
                                txtNombreVendedor.Text = "";
                                txtfechaIngreso.Text = "";
                                lfechaingreso.Visibility = System.Windows.Visibility.Visible;
                                txtfechaIngreso.Visibility = Visibility.Visible;
                                lcontraseña.Visibility = Visibility.Hidden;
                                txtContraseña.Visibility = Visibility.Hidden;
                                rdbtnvendedor.IsChecked = true;
                                btnguardarVendedor.Visibility = Visibility.Visible;
                                btncancelarEditarVendedor.Visibility = Visibility.Hidden;
                                btnGuardarActualizarVendedor.Visibility = Visibility.Hidden;
                                txtrutvendedor.IsEnabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Error al guardar vendedor" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }

                    }

                    //Guardar Cliente
                    /* vendedorFacade clienteF = new vendedorFacade();
                     string res = clienteF.actualizarVendedor(rut, nombre, fechaingreso);

                     if (res.Equals(""))
                     {
                         MessageBox.Show("Vendedor actualizado");
                         llenarTablaVendedor();
                         txtrutvendedor.Text = "";
                         txtNombreVendedor.Text = "";
                         txtfechaIngreso.Text = "";
                         btnguardarVendedor.Visibility = Visibility.Visible;
                         btncancelarEditarVendedor.Visibility = Visibility.Hidden;
                         btnGuardarActualizarVendedor.Visibility = Visibility.Hidden;
                     }
                     else
                     {
                         MessageBox.Show("Error al actualizar vendedor" + res);
                     }*/
                }
            }


        }
        private void btnActualizarVendedor_Click(object sender, RoutedEventArgs e)
        {

            //cargar Vendedor para edicion
            if (datagridVendedor.SelectedItem != null)
            {
                if (datagridVendedor.SelectedItem is Vendedor)
                {
                    var row = (Vendedor)datagridVendedor.SelectedItem;

                    if (row != null)
                    {
                        btnguardarVendedor.Visibility = Visibility.Hidden;
                        btncancelarEditarVendedor.Visibility = Visibility.Visible;
                        btnGuardarActualizarVendedor.Visibility = Visibility.Visible;
                        vendedorFacade v = new vendedorFacade();

                        bool existe = v.getExistAdminByRut(row.rut);
                        Vendedor vend = v.getVendedorbyRut(row.rut);
                        if (existe)
                        {
                            lfechaingreso.Visibility = System.Windows.Visibility.Hidden;
                            txtfechaIngreso.Visibility = System.Windows.Visibility.Hidden;
                            rdbtnvendedor.Visibility = Visibility.Hidden;
                            rdbtnAdmin.Visibility = Visibility.Visible;
                            lcontraseña.Visibility = System.Windows.Visibility.Visible;
                            lcontraseña.Content = "Nueva Contraseña";
                            txtContraseña.Visibility = System.Windows.Visibility.Visible;
                            txtrutvendedor.Text = vend.rut;
                            txtNombreVendedor.Text = vend.nombre;

                            txtContraseña.Password = "";

                            rdbtnAdmin.IsChecked = true;
                            txtrutvendedor.IsEnabled = false;


                        }
                        else
                        {
                            lfechaingreso.Visibility = System.Windows.Visibility.Visible;
                            txtfechaIngreso.Visibility = System.Windows.Visibility.Visible;
                            rdbtnvendedor.Visibility = Visibility.Visible;
                            rdbtnAdmin.Visibility = Visibility.Hidden;
                            lcontraseña.Visibility = System.Windows.Visibility.Hidden;
                            txtContraseña.Visibility = System.Windows.Visibility.Hidden;
                            txtrutvendedor.Text = vend.rut;
                            txtNombreVendedor.Text = vend.nombre;
                            txtfechaIngreso.Text = vend.fechaIngresoTrabajar;
                            rdbtnvendedor.IsChecked = true;
                            lcontraseña.Content = "Contraseña";
                            txtrutvendedor.IsEnabled = false;

                        }



                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccionar vendedor para edicion", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void btncancelarEditarVendedor_Click(object sender, RoutedEventArgs e)
        {
            btnguardarVendedor.Visibility = Visibility.Visible;
            btncancelarEditarVendedor.Visibility = Visibility.Hidden;
            btnGuardarActualizarVendedor.Visibility = Visibility.Hidden;
            //cargar Vendedor para edicion
            if (datagridVendedor.SelectedItem != null)
            {
                if (datagridVendedor.SelectedItem is Vendedor)
                {
                    var row = (Vendedor)datagridVendedor.SelectedItem;

                    if (row != null)
                    {
                        txtrutvendedor.Text = "";
                        txtNombreVendedor.Text = "";
                        txtfechaIngreso.Text = "";
                        txtContraseña.Password = "";
                        lfechaingreso.Visibility = System.Windows.Visibility.Visible;
                        txtfechaIngreso.Visibility = System.Windows.Visibility.Visible;
                        rdbtnvendedor.Visibility = Visibility.Visible;
                        lcontraseña.Visibility = System.Windows.Visibility.Hidden;
                        txtContraseña.Visibility = System.Windows.Visibility.Hidden;
                        rdbtnvendedor.IsChecked = true;
                        txtrutvendedor.IsEnabled = true;
                    }
                }
            }

        }
        private void btnEliminarVendedor_Click(object sender, RoutedEventArgs e)
        {
            if (datagridVendedor.SelectedItem != null)
            {
                if (datagridVendedor.SelectedItem is Vendedor)
                {
                    var row = (Vendedor)datagridVendedor.SelectedItem;

                    if (row != null)
                    {
                        //Elimnar Vendedor
                        vendedorFacade clienteF = new vendedorFacade();

                        bool esAdmin = clienteF.getVerificarsiesAdmin(row.rut);

                        if (esAdmin)
                        {
                            MessageBox.Show(row.nombre + " es administrador. No se puede eliminar", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            if (MessageBox.Show("¿Esta seguro de eliminar vendedor:" + row.nombre + "?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                string res = clienteF.borrarVendedorByRut(row.rut);
                                if (res.Equals(""))
                                {
                                    MessageBox.Show("Vendedor borrado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                    llenarTablaVendedor();
                                    txtrutvendedor.Text = "";
                                    txtNombreVendedor.Text = "";
                                    txtfechaIngreso.Text = "";
                                    btnguardarVendedor.Visibility = Visibility.Visible;
                                    btncancelarEditarVendedor.Visibility = Visibility.Hidden;
                                    btnGuardarActualizarVendedor.Visibility = Visibility.Hidden;
                                    lfechaingreso.Visibility = System.Windows.Visibility.Visible;
                                    txtfechaIngreso.Visibility = System.Windows.Visibility.Visible;
                                    rdbtnvendedor.Visibility = Visibility.Visible;
                                    rdbtnAdmin.Visibility = Visibility.Visible;
                                    lcontraseña.Visibility = System.Windows.Visibility.Hidden;
                                    txtContraseña.Visibility = System.Windows.Visibility.Hidden;
                                    rdbtnvendedor.IsChecked = true;
                                    txtrutvendedor.IsEnabled = true;
                                }
                                else
                                {
                                    MessageBox.Show("Error al borrar vendedor" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }


                        }

                    }
                }
            }

        }

        private void btnEliminarTodosVendedores_Click(object sender, RoutedEventArgs e)
        {
            vendedorFacade vendedorFtotal = new vendedorFacade();
            int totalVend = vendedorFtotal.getTotalVendedor();
            if (MessageBox.Show("Esta seguro de borrar " + totalVend.ToString() + " Vendedor(es) ", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                //Elimnar Vendedor
                vendedorFacade clienteF = new vendedorFacade();
                string res = clienteF.borrarAllVendedor();

                if (res.Equals(""))
                {
                    MessageBox.Show("Vendedor(es) borrado(s)", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                    llenarTablaVendedor();
                    txtrutvendedor.Text = "";
                    txtNombreVendedor.Text = "";
                    txtfechaIngreso.Text = "";
                    btnguardarVendedor.Visibility = Visibility.Visible;
                    btncancelarEditarVendedor.Visibility = Visibility.Hidden;
                    btnGuardarActualizarVendedor.Visibility = Visibility.Hidden;
                    txtrutvendedor.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Error al borrar vendedor(es)" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }

        }

        private void rdbtnAdmin_Checked(object sender, RoutedEventArgs e)
        {
            //obtener en tabla vendedor el tipo ,si es 1 ya hay administrador
            /*vendedorFacade vendFac = new vendedorFacade();
            bool existe = vendFac.getExistAdmin();
            if (existe)
            {
                MessageBox.Show("Ya existe 1 administrador");
            }
            else
            {*/
            lfechaingreso.Visibility = System.Windows.Visibility.Hidden;
            txtfechaIngreso.Visibility = Visibility.Hidden;
            lcontraseña.Visibility = Visibility.Visible;
            txtContraseña.Visibility = Visibility.Visible;

            /*   }*/
        }

        private void rdbtnvendedor_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                lfechaingreso.Visibility = System.Windows.Visibility.Visible;
                txtfechaIngreso.Visibility = System.Windows.Visibility.Visible;
                lcontraseña.Visibility = System.Windows.Visibility.Hidden;
                txtContraseña.Visibility = System.Windows.Visibility.Hidden;
                lcontraseña.Content = "Contraseña";
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }

        }

        private void btnRecargar_Click(object sender, RoutedEventArgs e)
        {
            llenarTablaVendedor();
        }


    }
}
