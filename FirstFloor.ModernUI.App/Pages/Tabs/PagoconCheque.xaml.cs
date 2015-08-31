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
using FirstFloor.ModernUI.App.Control;
using FirstFloor.ModernUI.App.Modelo;
using System.Text.RegularExpressions;

namespace FirstFloor.ModernUI.App.Pages.Tabs
{
    /// <summary>
    /// Interaction logic for PagoconCheque.xaml
    /// </summary>
    public partial class PagoconCheque : Window
    {
        DateTime fechaemision;
        DateTime fechaexpiracion;
        Cheque ch = new Cheque();
        string sRut = "";
        public PagoconCheque()
        {
            InitializeComponent();
            datepickerEmision.SelectedDate = DateTime.Now.Date;
            DateTime f = DateTime.Now.Date;
            f = f.AddDays(1);
            datepickerExpiracion.SelectedDate = f;
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

        public string getrutCheque()
        {
            return sRut;
        }
        public void setDatauser(string rut, string nombre)
        {
            txtRutemiso.Text = rut;
            txtNombremiso.Text = nombre;
        }
        public void setrutCheque(string rut)
        {
            sRut = rut;
        }
        public Cheque getformCheque()
        {
            return ch;
        }
        public void setformCheque(string rut, string nombre, string nombrebanco, string fechaemision, string fechaexpiracion, string fechaVenta, string monto)
        {
            ch.rut = rut;
            ch.nombre = nombre;
            ch.nombreBanco = nombrebanco;
            ch.fechaemision = fechaemision;
            ch.fechaexpiracion = fechaexpiracion;
            ch.fechaVenta = fechaVenta;
            ch.monto = monto;
        }

        public int CalcularDiasDeDiferencia(DateTime primerFecha, DateTime segundaFecha)
        {
            TimeSpan diferencia;
            diferencia = primerFecha - segundaFecha;

            return diferencia.Days;
        }
        public int CalcularMesesDeDiferencia(DateTime fechaDesde, DateTime fechaHasta)
        {
            return (fechaDesde.Month - fechaHasta.Month) + 12 * (fechaDesde.Year - fechaHasta.Year);
        }
        private bool verificarFecha(DateTime fechaemision, DateTime fechaexpiracion)
        {
            bool resp = false;
            DateTime fechaactual = DateTime.Now.Date;
            //MessageBox.Show(fechaemision.ToString() + ":" + fechaactual);
            if (fechaemision < fechaactual)
            {
                MessageBox.Show("Fecha emision debe ser mayor a actual", "Informacion", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (fechaexpiracion < fechaactual)
            {
                MessageBox.Show("Fecha expiracion debe ser mayor a actual", "Informacion", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (fechaemision > fechaexpiracion)
            {
                MessageBox.Show("Fecha emision debe ser menor que fecha expiracion", "Informacion", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (fechaexpiracion == fechaactual)
            {
                MessageBox.Show("Fecha emision debe ser mayor a fecha expiración", "Informacion", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                txtDiferencias.Content = "";

                int dias = CalcularDiasDeDiferencia(fechaexpiracion, fechaemision);
                int mes = CalcularMesesDeDiferencia(fechaexpiracion, fechaemision);
                if (dias < 0 || mes < 0)
                {
                    txtDiferencias.Content = "0 Dia(s)";
                }
                else
                {
                    if (dias < 31)
                    {
                        txtDiferencias.Content = dias.ToString() + " Dia(s)";
                    }
                    else
                    {
                        txtDiferencias.Content = mes.ToString() + " Mes(ses)";
                    }
                }

                resp = true;

            }
            return resp;

        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            //(this.Owner as Venta).cbTipoPago.SelectedIndex = 0;
            //((this.Parent)  as Ventas).cbTipoPago.SelectedIndex = 0;
            
            this.Close();
        }
        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRutemiso.Text))
            {
                MessageBox.Show("Ingresar rut", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrWhiteSpace(txtNombremiso.Text))
            {
                MessageBox.Show("Ingresar Nombre emisor", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrWhiteSpace(txtnombrebanco.Text))
            {
                MessageBox.Show("Ingresar nombre banco", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (string.IsNullOrWhiteSpace(txtMonto.Text))
            {
                MessageBox.Show("Ingresar monto de cheque", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                fechaemision = datepickerEmision.SelectedDate.Value.Date;
                fechaexpiracion = datepickerExpiracion.SelectedDate.Value.Date;
                Boolean resp = verificarFecha(fechaemision, fechaexpiracion);
                if (resp)
                {
                    if (validarRut(txtRutemiso.Text))
                    {
                        DateTime Hoy = DateTime.Today;
                        string fechaVenta = Hoy.ToString("dd-MM-yyyy");
                        //string fechaVenta = "06/06/2016";
                        //string fechaVenta = DateTime.Now.Date.ToString("d");
                        //MainWindow mw = new MainWindow();

                        Uri img = new Uri("/Imagenes/ok.png", UriKind.Relative);
                        BitmapImage imagen = new BitmapImage(img);

                        //mw.imgOk.Source = imagen;

                        //((this.Parent) as Venta).img
                        //((this.Parent) as Ventas).imgOk.Source = imagen;
                        //Enviar datos de formulario cheque a mainwindows

                        setformCheque(txtRutemiso.Text, txtNombremiso.Text, txtnombrebanco.Text, fechaemision.ToString("d"), fechaVenta, fechaexpiracion.ToString("d"), txtMonto.Text);
                        setrutCheque(txtRutemiso.Text);
                        //MessageBox.Show(txtRutemiso.Text);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Rut no es válido", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void datepickerEmision_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fechaemision = datepickerEmision.SelectedDate.Value.Date;

            //verificarFecha(fechaemision, fechaexpiracion);
            txtDiferencias.Content = "";

            int dias = CalcularDiasDeDiferencia(fechaexpiracion, fechaemision);
            int mes = CalcularMesesDeDiferencia(fechaexpiracion, fechaemision);
            if (dias < 0 || mes < 0)
            {
                txtDiferencias.Content = "0 Dia(s)";
            }
            else
            {
                if (dias < 31)
                {
                    txtDiferencias.Content = dias.ToString() + " Dia(s)";
                }
                else
                {
                    txtDiferencias.Content = mes.ToString() + " Mes(ses)";
                }
            }
        }

        private void datepickerExpiracion_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            fechaexpiracion = datepickerExpiracion.SelectedDate.Value.Date;
            //verificarFecha(fechaemision, fechaexpiracion);
            txtDiferencias.Content = "";

            int dias = CalcularDiasDeDiferencia(fechaexpiracion, fechaemision);
            int mes = CalcularMesesDeDiferencia(fechaexpiracion, fechaemision);
            if (dias < 0 || mes < 0)
            {
                txtDiferencias.Content = "0 Dia(s)";
            }
            else
            {
                if (dias < 31)
                {
                    txtDiferencias.Content = dias.ToString() + " Dia(s)";
                }
                else
                {
                    txtDiferencias.Content = mes.ToString() + " Mes(ses)";
                }
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
