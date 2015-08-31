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
using System.Text.RegularExpressions;
using FirstFloor.ModernUI.App.Control;
using FirstFloor.ModernUI.App.Modelo;
using System.Collections.ObjectModel;
using System.Collections;
namespace FirstFloor.ModernUI.App.Pages.Tabs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Devolucion : Window
    {
        private readonly Regex _numMatch;
        ObservableCollection<VentaTemporal> venta = new ObservableCollection<VentaTemporal> { };
        int cantidad = 0;
        int total = 0;
        int subtotal = 0;
        Ventas cventas;
        VentaTemporal vtemp = new VentaTemporal();
        public Devolucion()
        {
            InitializeComponent();
            _numMatch = new Regex(@"^-?\d+$");
            Maximum = 9999;
            Minimum = 1;
            TextBoxValue.Text = "1";
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

        public void setInstancia(Ventas v)
        {
            cventas = v;
        }
        public Ventas getInstancia()
        {
            return cventas;
        }

    
        private void codProdVenta_TextChanged(object sender, TextChangedEventArgs e)
        {
            

        }
        //Recorre el datagrid de ventas temporal para buscar idprod y asi sumar +1 cantidad
        public IEnumerable<System.Windows.Controls.DataGridRow> GetDataGridRows(System.Windows.Controls.DataGrid grid)
        {
            var itemsSource = datagridVentas.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as System.Windows.Controls.DataGridRow;
                if (null != row) yield return row;
            }
        }

     
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnBuscarVenta_Click(object sender, RoutedEventArgs e)
        {

            if (txtidventa.Text.Equals(""))
            {
                MessageBox.Show("Ingresar idventa", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (txtidProducto.Text.Equals(""))
            {
                MessageBox.Show("Ingresar idProducto", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (fechaventa.Text.Equals(""))
            {
                MessageBox.Show("Ingresar Fecha", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {


                ventasFacade ventFac = new ventasFacade();
                List<MVentas> listaVentaDevolucion = ventFac.getVentasForDevolucion(Convert.ToDouble(txtidventa.Text), txtidProducto.Text, Convert.ToDateTime(fechaventa.Text));
                var rows = GetDataGridRows(datagridVentas);
                if (listaVentaDevolucion.Count != 0)
                {
                    venta.Clear();
                    datagridVentas.ItemsSource = venta;
                    //llenar datagridVenta para devoulcion
                    foreach (var item in listaVentaDevolucion)
                    {
                        ProductoFacade prodFobtener = new ProductoFacade();
                        Producto Prod = new Producto();
                        Prod = prodFobtener.getProductosByID(item.idProducto);
                        vtemp = new VentaTemporal(item.idVenta, item.idProducto, Prod.nombre, Prod.precio, item.cantidad.ToString(), "1", item.total.ToString());
                        venta.Add(vtemp);
                        cantidad = cantidad + 1;
                        total = total + Convert.ToInt32(item.total);

                        //ltotal.Content = Prod.precio;
                        ltotal.Content = item.total;
                        TextBoxValue.Text = item.cantidad.ToString();
                    }
                    datagridVentas.ItemsSource = venta;
                }
                else
                {
                    MessageBox.Show("No se han encontrado ventas con estos datos.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }

        private void textbox_NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }






        //############################################################
        //################           Spin Box       ##################
        //############################################################

        private void ResetText(TextBox tb)
        {
            tb.Text = 1 < Minimum ? Minimum.ToString() : "1";

            tb.SelectAll();
        }

        private void value_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = (TextBox)sender;
            var text = tb.Text.Insert(tb.CaretIndex, e.Text);

            e.Handled = !_numMatch.IsMatch(text);
        }

        private void value_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            if (!_numMatch.IsMatch(tb.Text)) ResetText(tb);
            Valor = Convert.ToInt32(tb.Text);
            if (Valor < Minimum) Valor = Minimum;
            if (Valor > Maximum) Valor = Maximum;



            RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }
        //Al presionar enter en spinkbox que se actualice en tabla la cantidad 
        private void TextBoxValue_KeyDown(Object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                if (datagridVentas.Items.Count > 0)
                {
                    if (datagridVentas.SelectedItem != null)
                    {
                        if (datagridVentas.SelectedItem is VentaTemporal)
                        {
                            var row = (VentaTemporal)datagridVentas.SelectedItem;
                            ProductoFacade prodFobtener = new ProductoFacade();
                            Producto Prod = new Producto();
                            Prod = prodFobtener.getProductosByID(row.idProducto);
                            int filaProd = 0;
                            int num = 0;
                            if (row != null)
                            {
                                //Tomar valor de cantidad  datagrid y aumentar 
                                List<VentaTemporal> vtemporal = new List<VentaTemporal>();
                                var rows = GetDataGridRows(datagridVentas);
                                foreach (DataGridRow r in rows)
                                {
                                    VentaTemporal rv = (VentaTemporal)r.Item;
                                    vtemporal.Add(rv);
                                }
                                //buscar el id de list con el seleccionado en datagrid
                                foreach (var r in vtemporal)
                                {
                                    if (row.idProducto.Equals(r.idProducto))
                                    {
                                        filaProd = num;
                                    }
                                    num = num + 1;

                                }

                                //Verificar stock de producto
                                ProductoFacade prodcantiFacade = new ProductoFacade();
                                int maxstock = Convert.ToInt32(prodcantiFacade.getStockProductoByidProd(row.idProducto));
                                if (Valor <= Convert.ToInt32(row.cantidad))
                                {
                                    int total = Convert.ToInt32(Prod.precio) * Convert.ToInt32(Valor);
                                    vtemporal[filaProd].devolver = Valor.ToString();
                                    //vtemporal[filaProd].total = total.ToString();
                                    ltotal.Content = total.ToString();
                                    datagridVentas.ItemsSource = null;
                                    datagridVentas.ItemsSource = vtemporal;
                                    datagridVentas.SelectedIndex = filaProd;
                                }
                                else
                                {
                                    //MessageBox.Show("Producto, segun inventario, no cuenta con mas stock");
                                    Valor = Convert.ToInt32(row.cantidad);
                                }

                            }
                        }
                    }
                }
                //Recorrer tabla para sumar total
                /* subtotal = 0;

                 foreach (var i in venta)
                 {
                     subtotal = subtotal + Convert.ToInt32(i.total);
                     //MessageBox.Show(i.total.ToString());
                 }
                 txtsubtotal.Text = subtotal.ToString();*/
                subtotal = 0;
                int numProducto = 0;
                foreach (var i in venta)
                {
                    subtotal = subtotal + Convert.ToInt32(i.total);
                    //MessageBox.Show(i.total.ToString());
                    numProducto = numProducto + 1;
                }
                /*txtsubtotal.Text = subtotal.ToString();
                txtCantidadProductos.Content = numProducto.ToString();
                if (Convert.ToInt32(txtdescuento.Text) == 0)
                {
                    txttotal.Text = txtsubtotal.Text;
                }
                else
                {
                    double des = Convert.ToDouble(txtdescuento.Text) / Convert.ToDouble(100);
                    txttotal.Text = (Convert.ToInt32(txtsubtotal.Text) - Convert.ToDouble(txtsubtotal.Text) * des).ToString();
                }*/
            }

        }
        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (Valor < Maximum)
            {
               
                /*Aumentar en tabla valor  de cantidad de valor seleccionado de producto*/
                if (datagridVentas.SelectedItem != null)
                {
                    if (datagridVentas.Items.Count > 0)
                    {
                        if (datagridVentas.SelectedItem is VentaTemporal)
                        {
                            Valor++;
                            var row = (VentaTemporal)datagridVentas.SelectedItem;
                            ProductoFacade prodFobtener = new ProductoFacade();
                            Producto Prod = new Producto();
                            Prod = prodFobtener.getProductosByID(row.idProducto);
                            int filaProd = 0;
                            int num = 0;
                            if (row != null)
                            {

                                //MessageBox.Show(row.cantidad); 
                                //Tomar valor de cantidad  datagrid y aumentar 
                                List<VentaTemporal> vtemporal = new List<VentaTemporal>();
                                var rows = GetDataGridRows(datagridVentas);
                                foreach (DataGridRow r in rows)
                                {
                                    VentaTemporal rv = (VentaTemporal)r.Item;
                                    vtemporal.Add(rv);
                                }
                                //buscar el id de list con el seleccionado en datagrid
                                foreach (var r in vtemporal)
                                {
                                    if (row.idProducto.Equals(r.idProducto))
                                    {
                                        filaProd = num;
                                    }
                                    num = num + 1;

                                }
                                ProductoFacade prodcantiFacade = new ProductoFacade();
                                int maxstock = Convert.ToInt32(prodcantiFacade.getStockProductoByidProd(row.idProducto));
                                if (Valor <= Convert.ToInt32(row.cantidad))
                                {
                                    int total = Convert.ToInt32(Prod.precio) * Convert.ToInt32(Valor);
                                    vtemporal[filaProd].devolver = Valor.ToString();
                                    //vtemporal[filaProd].total = total.ToString();
                                    ltotal.Content =  total.ToString();
                                    datagridVentas.ItemsSource = null;
                                    datagridVentas.ItemsSource = vtemporal;
                                    datagridVentas.SelectedIndex = filaProd;
                                }
                                else
                                {
                                    //MessageBox.Show("Producto, segun inventario, no cuenta con mas stock");
                                    Valor = Convert.ToInt32(row.cantidad);
                                }

                            }
                        }
                    }
                }
                //Recorrer tabla para sumar total
                /*  subtotal = 0;

                  foreach (var i in venta)
                  {
                      subtotal = subtotal + Convert.ToInt32(i.total);
                      //MessageBox.Show(i.total.ToString());
                  }
                  txtsubtotal.Text = subtotal.ToString*/
                //Recorrer tabla para sumar total
                subtotal = 0;
                int numProducto = 0;
                foreach (var i in venta)
                {
                    subtotal = subtotal + Convert.ToInt32(i.total);
                    //MessageBox.Show(i.total.ToString());
                    numProducto = numProducto + 1;
                }
               /* txtsubtotal.Text = subtotal.ToString();
                txtCantidadProductos.Content = numProducto.ToString();
                if (Convert.ToInt32(txtdescuento.Text) == 0)
                {
                    txttotal.Text = txtsubtotal.Text;
                }
                else
                {
                    double des = Convert.ToDouble(txtdescuento.Text) / Convert.ToDouble(100);
                    txttotal.Text = (Convert.ToInt32(txtsubtotal.Text) - Convert.ToDouble(txtsubtotal.Text) * des).ToString();
                }*/
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (Valor > Minimum)
            {
                Valor--;
                if (datagridVentas.SelectedItem != null)
                {
                    if (datagridVentas.Items.Count > 0)
                    {
                        if (datagridVentas.SelectedItem is VentaTemporal)
                        {
                            var row = (VentaTemporal)datagridVentas.SelectedItem;
                            ProductoFacade prodFobtener = new ProductoFacade();
                            Producto Prod = new Producto();
                            Prod = prodFobtener.getProductosByID(row.idProducto);
                            int filaProd = 0;
                            int num = 0;
                            if (row != null)
                            {

                                //MessageBox.Show(row.cantidad); 
                                //Tomar valor de cantidad  datagrid y aumentar 
                                List<VentaTemporal> vtemporal = new List<VentaTemporal>();
                                var rows = GetDataGridRows(datagridVentas);
                                foreach (DataGridRow r in rows)
                                {
                                    VentaTemporal rv = (VentaTemporal)r.Item;
                                    vtemporal.Add(rv);
                                }
                                //buscar el id de list con el seleccionado en datagrid
                                foreach (var r in vtemporal)
                                {
                                    if (row.idProducto.Equals(r.idProducto))
                                    {
                                        filaProd = num;
                                    }
                                    num = num + 1;

                                }

                                int total = Convert.ToInt32(Prod.precio) * Convert.ToInt32(Valor);
                                vtemporal[filaProd].devolver = Valor.ToString();
                                //vtemporal[filaProd].total = total.ToString();
                                ltotal.Content = total.ToString();
                                datagridVentas.ItemsSource = null;
                                datagridVentas.ItemsSource = vtemporal;
                                datagridVentas.SelectedIndex = filaProd;



                            }
                        }
                    }
                }
                //Recorrer tabla para sumar total
                /*subtotal = 0;

                foreach (var i in venta)
                {
                    subtotal = subtotal + Convert.ToInt32(i.total);
                    //MessageBox.Show(i.total.ToString());
                }
                txtsubtotal.Text = subtotal.ToString();*/
                subtotal = 0;
                int numProducto = 0;
                foreach (var i in venta)
                {
                    subtotal = subtotal + Convert.ToInt32(i.total);
                    //MessageBox.Show(i.total.ToString());
                    numProducto = numProducto + 1;
                }
                /*
                txtsubtotal.Text = subtotal.ToString();
                txtCantidadProductos.Content = numProducto.ToString();
                if (Convert.ToInt32(txtdescuento.Text) == 0)
                {
                    txttotal.Text = txtsubtotal.Text;
                }
                else
                {
                    double des = Convert.ToDouble(txtdescuento.Text) / Convert.ToDouble(100);
                    txttotal.Text = (Convert.ToInt32(txtsubtotal.Text) - Convert.ToDouble(txtsubtotal.Text) * des).ToString();
                }*/
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
            }
        }

        /// <summary>The Value property represents the TextBoxValue of the control.</summary>
        /// <returns>The current TextBoxValue of the control</returns>      

        public int Valor
        {
            get
            {

                return (int)GetValue(ValueProperty);
            }
            set
            {
                TextBoxValue.Text = value.ToString();
                SetValue(ValueProperty, value);

            }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Valor", typeof(int), typeof(Devolucion),
              new PropertyMetadata(0, new PropertyChangedCallback(OnSomeValuePropertyChanged)));


        private static void OnSomeValuePropertyChanged(
        DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Devolucion numericBox = target as Devolucion;
            numericBox.TextBoxValue.Text = e.NewValue.ToString();
        }

        /// <summary>
        /// Maximum value for the Numeric Up Down control
        /// </summary>
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int), typeof(Devolucion), new UIPropertyMetadata(100));

        /// <summary>
        /// Minimum value of the numeric up down conrol.
        /// </summary>
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(Devolucion), new UIPropertyMetadata(0));


        // Value changed
        private static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Devolucion));

        /// <summary>The ValueChanged event is called when the TextBoxValue of the control changes.</summary>
        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        //Increase button clicked
        private static readonly RoutedEvent IncreaseClickedEvent =
            EventManager.RegisterRoutedEvent("IncreaseClicked", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Devolucion));

        /// <summary>The IncreaseClicked event is called when the Increase button clicked</summary>
        public event RoutedEventHandler IncreaseClicked
        {
            add { AddHandler(IncreaseClickedEvent, value); }
            remove { RemoveHandler(IncreaseClickedEvent, value); }
        }

        //Increase button clicked
        private static readonly RoutedEvent DecreaseClickedEvent =
            EventManager.RegisterRoutedEvent("DecreaseClicked", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Devolucion));

        /// <summary>The DecreaseClicked event is called when the Decrease button clicked</summary>
        public event RoutedEventHandler DecreaseClicked
        {
            add { AddHandler(DecreaseClickedEvent, value); }
            remove { RemoveHandler(DecreaseClickedEvent, value); }
        }

        /// <summary>
        /// Checking for Up and Down events and updating the value accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void value_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Up && Valor < Maximum)
            {
                Valor++;
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
            else if (e.IsDown && e.Key == Key.Down && Valor > Minimum)
            {
                Valor--;
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));

            }
        }

        private void btnDevolver_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (datagridVentas.Items.Count > 0)
                {
                    if (datagridVentas.SelectedItem != null)
                    {
                        var row = GetDataGridRows(datagridVentas);
                        if (row != null)
                        {
                            VentaTemporal rv=new VentaTemporal();
                            foreach (DataGridRow r in row)
                            {
                                 rv = (VentaTemporal)r.Item;
                            }
                                int difDevo = Convert.ToInt32(rv.cantidad) - Convert.ToInt32(rv.devolver);
                                //MessageBox.Show("dif:" + difDevo.ToString());
                                if (difDevo == 0)
                                {//si cantidad devolucion es igul a cantidad venta, actualizar stock producto y borrar registro de venta
                                    int sumStock = Convert.ToInt32(rv.devolver);
                                    ProductoFacade prodFac = new ProductoFacade();
                                    string actStock = prodFac.actualizarStockProductoDevolucion(rv.idProducto, sumStock);
                                    ventasFacade ventFac = new ventasFacade();
                                    string borrar = ventFac.borrarventaByidVenta(Convert.ToDouble(txtidventa.Text), txtidProducto.Text, Convert.ToDateTime(fechaventa.Text));

                                    if (!string.IsNullOrEmpty(actStock))
                                    {
                                        MessageBox.Show("Error al actualizar stock:" + actStock + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else if (!string.IsNullOrEmpty(borrar))
                                    {
                                        MessageBox.Show("Error al borrar venta:" + borrar + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Devolucion ingresada correctamente.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                        txtidventa.Text = "";
                                        txtidProducto.Text = "";
                                        fechaventa.Text = "";
                                        venta.Clear();
                                        datagridVentas.ItemsSource = venta;
                                        ltotal.Content = "0";
                                        Valor = 1;
                                        this.Close();
                                    }
                                }
                                else
                                {//si cantidad devolucion no es igual a cantidad en venta , actualizar venta e stock producto
                                    int sumStock = Convert.ToInt32(rv.devolver);
                                    ProductoFacade prodFac = new ProductoFacade();
                                    string actStock = prodFac.actualizarStockProductoDevolucion(rv.idProducto, sumStock);
                                    ventasFacade ventFac = new ventasFacade();
                                    string actVenta = ventFac.actualizarventaDevolucion(Convert.ToDouble(txtidventa.Text), txtidProducto.Text, Convert.ToDateTime(fechaventa.Text), difDevo, difDevo * Convert.ToInt32(rv.precio));

                                    if (!string.IsNullOrEmpty(actStock))
                                    {
                                        MessageBox.Show("Error al actualizar stock:" + actStock + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else if (!string.IsNullOrEmpty(actVenta))
                                    {
                                        MessageBox.Show("Error al actualizar venta:" + actVenta + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Devolucion ingresada correctamente.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                        txtidventa.Text = "";
                                        txtidProducto.Text = "";
                                        fechaventa.Text = "";
                                        venta.Clear();
                                        datagridVentas.ItemsSource = venta;
                                        ltotal.Content = "0";
                                        Valor = 1;
                                        this.Close();
                                    }

                                }
                            

                        }
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (datagridVentas.Items.Count > 0)
            {
                string total = ltotal.Content.ToString();
                cventas.txtTotaldevolucion.Text = total;
                cventas.setVenta(vtemp);
                cventas.setDineroDevolucion(Convert.ToInt32(total));
                cventas.btnPagar.Content = "Cambiar";
                cventas.setFechaDevolucion(fechaventa.ToString());
                cventas.ltotalDevolucion.Visibility = Visibility.Visible;
                cventas.txtTotaldevolucion.Visibility = Visibility.Visible;
                cventas.lDiferencia.Visibility = Visibility.Visible;
                cventas.txtDiferencia.Visibility = Visibility.Visible;
                cventas.setListaVenta();
                /*cventas.datagridVentas.ItemsSource = null;
                cventas.txtsubtotal.Text = "0";
                cventas.txttotal.Text = "0";*/
                cventas.limpiarRegistroVentas();

                this.Close();
            }

        }

        private void btnbuscarID_Click(object sender, RoutedEventArgs e)
        {
            BuscarProducto bus = new BuscarProducto();
            bus.setInstancia(this);
            bus.Show();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }



    }

}
