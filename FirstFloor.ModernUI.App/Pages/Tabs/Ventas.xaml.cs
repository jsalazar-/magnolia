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
using System.Collections.ObjectModel;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections;
using System.Windows.Controls.Primitives;
using System.Configuration;
using FirstFloor.ModernUI.App.Modelo;
using FirstFloor.ModernUI.App.Control;
using System.Globalization;
using System.Threading;

namespace FirstFloor.ModernUI.App.Pages.Tabs
{
    /// <summary>
    /// Interaction logic for MVentas.xaml
    /// </summary>
    public partial class Ventas : UserControl
    {
        private DataGridRow previousRow;
        private readonly Regex _numMatch;
        ObservableCollection<VentaTemporal> venta = new ObservableCollection<VentaTemporal> { };
        int subtotal = 0;
        Cheque getCheque = new Cheque();
        PagoconCheque pc = new PagoconCheque();
        string rutcliente = "";
        string nombreCliente = "";
        string rutLogin = "";
        TransLoginToVenta transVenta;
        vendedorFacade vendFac = new vendedorFacade();

        //Usados en etapa de devolucion
        int dineroDevolucion = 0;
        VentaTemporal ventaTemp = new VentaTemporal();
        string fechaDevolucion = "";
        CultureInfo ci = new CultureInfo("en-us");
        CultureInfo us = new CultureInfo("en-US");

        public Ventas(string rutofLogin)
        {
            InitializeComponent();
            txtcodProdVenta.Focus();
            //control spinBox
            _numMatch = new Regex(@"^-?\d+$");
            Maximum = 9999;
            Minimum = 1;
            TextBoxValue.Text = "0";
            txtsubtotal.Text = "0";
            //MessageBox.Show(getRut());
            lVendedor.Content = "Vendedor:" + vendFac.getNombreAdminByRut(rutofLogin);

        }
        public void setDineroDevolucion(int t)
        {
            dineroDevolucion = t;
        }

        //Datos de devolucion
        public void setListaVenta()
        {
            venta = new ObservableCollection<VentaTemporal> { };
        }
        public void setFechaDevolucion(string v)
        {
            fechaDevolucion = v;
        }
        public void setVenta(VentaTemporal v)
        {
            ventaTemp = v;
        }
        public VentaTemporal getVenta()
        {
            return ventaTemp;
        }

        //Rut de login 
        public void setRut(string rutset)
        {
            rutLogin = rutset;
        }
        public string getRut()
        {
            return rutLogin;
        }
        public void setInstancia(TransLoginToVenta tr)
        {
            transVenta = tr;
        }
        public TransLoginToVenta getInstancia()
        {
            return transVenta;
        }

        private void descInput(object sender, TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
            e.Handled = ascci >= 48 && ascci <= 57 ? false : true;
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
            Value = Convert.ToInt32(tb.Text);
            if (Value < Minimum) Value = Minimum;
            if (Value > Maximum) Value = Maximum;



            RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }
        //Al presionar enter en spinkbox que se actualice en tabla la cantidad 
        private void TextBoxValue_KeyDown(Object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
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
                            if (Value <= maxstock)
                            {
                                int total = Convert.ToInt32(Prod.precio) * Convert.ToInt32(Value);
                                vtemporal[filaProd].cantidad = Value.ToString();
                                vtemporal[filaProd].total = total.ToString();

                                datagridVentas.ItemsSource = null;
                                datagridVentas.ItemsSource = vtemporal;
                                datagridVentas.SelectedIndex = filaProd;
                            }
                            else
                            {
                                MessageBox.Show("El producto no cuenta con más stock", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                Value = maxstock;
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
                int p = ToEntero(txtsubtotal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                txtsubtotal.Text = p.ToString("#,#", CultureInfo.InvariantCulture); ;
                //                txtDiferencia.Text = (Convert.ToInt32(txtsubtotal.Text) - dineroDevolucion).ToString();
                txtCantidadProductos.Content = numProducto.ToString();
                if (Convert.ToInt32(txtdescuento.Text) == 0)
                {

                    /********************************************************/
                    int mtotalDevo = subtotal - dineroDevolucion;
                    if (mtotalDevo < 0)
                    {
                        //Quiere decir que totaldevolucion tiene saldo 
                        txtDiferencia.Text = (mtotalDevo * -1).ToString();
                        mtotalDevo = 0;

                    }
                    else

                    {
                        txttotal.Text = "0";
                        txtDiferencia.Text = "0";
                    }

                    if (mtotalDevo < 0)
                    {
                        mtotalDevo = mtotalDevo * -1;
                    }
                    int pr = ToEntero(mtotalDevo.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txtsubtotal.Text = pr.ToString("#,#", CultureInfo.InvariantCulture); ;

                    int prT = ToEntero(mtotalDevo.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txttotal.Text = prT.ToString("#,#", CultureInfo.InvariantCulture); ;


                    /********************************************************/

                }
                else
                {
                    //falto en varia parte el pasar 
                    double des = Convert.ToDouble(txtdescuento.Text) / Convert.ToInt32(100);
                    int pr = ToEntero(txtsubtotal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    double totalDes = Convert.ToInt32(pr) - Convert.ToDouble(pr) * des;

                    int prT = ToEntero(totalDes.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txttotal.Text = prT.ToString("#,#", CultureInfo.InvariantCulture); ;


                    //txtDiferencia.Text = (totalDes - dineroDevolucion).ToString();
                }
            }

        }
        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (Value < Maximum)
            {
                Value++;
                /*Aumentar en tabla valor  de cantidad de valor seleccionado de producto*/
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
                            if (Value <= maxstock)
                            {
                                int total = Convert.ToInt32(Prod.precio) * Convert.ToInt32(Value);
                                vtemporal[filaProd].cantidad = Value.ToString();
                                vtemporal[filaProd].total = total.ToString();



                                datagridVentas.ItemsSource = null;
                                datagridVentas.ItemsSource = vtemporal;
                                datagridVentas.SelectedIndex = filaProd;
                            }
                            else
                            {
                                MessageBox.Show("El producto no cuenta con más stock", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                Value = maxstock;
                            }

                        }
                    }
                }
                subtotal = 0;
                int numProducto = 0;
                foreach (var i in venta)
                {
                    subtotal = subtotal + Convert.ToInt32(i.total);

                    numProducto = numProducto + 1;
                }

                txtCantidadProductos.Content = numProducto.ToString();

                if (Convert.ToInt32(txtdescuento.Text) == 0)
                {
                    /********************************************************/
                    int mtotalDevo = subtotal - dineroDevolucion;
                    if (mtotalDevo < 0)
                    {
                        //Quiere decir que totaldevolucion tiene saldo 
                        txtDiferencia.Text = (mtotalDevo * -1).ToString();
                        mtotalDevo = 0;

                    }
                    else

                    {
                        txttotal.Text = "0";
                        txtDiferencia.Text = "0";
                    }

                    if (mtotalDevo < 0)
                    {
                        mtotalDevo = mtotalDevo * -1;
                    }
                    int p = ToEntero(mtotalDevo.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txtsubtotal.Text = p.ToString("#,#", CultureInfo.InvariantCulture);

                    int prT = ToEntero(mtotalDevo.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txttotal.Text = prT.ToString("#,#", CultureInfo.InvariantCulture); ;

                    /********************************************************/

                    //txttotal.Text = txtsubtotal.Text;

                }
                else
                {
                    double des = Convert.ToDouble(txtdescuento.Text) / Convert.ToInt32(100);
                    /********************************************************/
                    int mtotalDevo = subtotal - dineroDevolucion;
                    if (mtotalDevo < 0)
                    {
                        //Quiere decir que totaldevolucion tiene saldo 
                        txtDiferencia.Text = (mtotalDevo * -1).ToString();
                        mtotalDevo = 0;

                    }
                    else

                    {
                        txttotal.Text = "0";
                        txtDiferencia.Text = "0";
                    }

                    if (mtotalDevo < 0)
                    {
                        mtotalDevo = mtotalDevo * -1;
                    }
                    int p = ToEntero(mtotalDevo.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txtsubtotal.Text = p.ToString("#,#", CultureInfo.InvariantCulture);

                    double d = (mtotalDevo - mtotalDevo * des);

                    int prT = ToEntero(d.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txttotal.Text = prT.ToString("#,#", CultureInfo.InvariantCulture); ;

                    /********************************************************/
                }
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (Value > Minimum)
            {
                Value--;
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

                            int total = Convert.ToInt32(Prod.precio) * Convert.ToInt32(Value);
                            vtemporal[filaProd].cantidad = Value.ToString();
                            vtemporal[filaProd].total = total.ToString();


                            string d = (Convert.ToDouble(txttotal.Text) - dineroDevolucion).ToString();

                            int prT = ToEntero(d, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                            txttotal.Text = prT.ToString("#,#", CultureInfo.InvariantCulture); ;



                            datagridVentas.ItemsSource = null;
                            datagridVentas.ItemsSource = vtemporal;
                            datagridVentas.SelectedIndex = filaProd;

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
                int p = ToEntero(subtotal.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                txtsubtotal.Text = p.ToString("#,#", CultureInfo.InvariantCulture);

                txtCantidadProductos.Content = numProducto.ToString();
                if (Convert.ToInt32(txtdescuento.Text) == 0)
                {
                    /********************************************************/
                    //En proceso de cambio, al presionar spinbox decre al total se descuenta el valor de cambio
                    int mtotalDevo = subtotal - dineroDevolucion;
                    if (mtotalDevo < 0)
                    {
                        //Quiere decir que totaldevolucion tiene saldo 
                        txtDiferencia.Text = (mtotalDevo * -1).ToString();
                        mtotalDevo = 0;

                    }
                    else

                    {
                        txttotal.Text = "0";
                        txtDiferencia.Text = "0";
                    }

                    if (mtotalDevo < 0)
                    {
                        mtotalDevo = mtotalDevo * -1;
                    }

                    int pr = ToEntero(mtotalDevo.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txtsubtotal.Text = pr.ToString("#,#", CultureInfo.InvariantCulture);
                    int prT = ToEntero(mtotalDevo.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txttotal.Text = prT.ToString("#,#", CultureInfo.InvariantCulture); ;

                    /********************************************************/

                }
                else
                {
                    double des = Convert.ToDouble(txtdescuento.Text) / Convert.ToInt32(100);
                    /********************************************************/
                    int mtotalDevo = subtotal - dineroDevolucion;
                    if (mtotalDevo < 0)
                    {
                        //Quiere decir que totaldevolucion tiene saldo 
                        txtDiferencia.Text = (mtotalDevo * -1).ToString();
                        mtotalDevo = 0;

                    }
                    else

                    {
                        txttotal.Text = "0";
                        txtDiferencia.Text = "0";
                    }

                    if (mtotalDevo < 0)
                    {
                        mtotalDevo = mtotalDevo * -1;
                    }
                    int pr = ToEntero(mtotalDevo.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txtsubtotal.Text = pr.ToString("#,#", CultureInfo.InvariantCulture);

                    string g = (mtotalDevo - mtotalDevo * des).ToString();
                    int prT = ToEntero(g, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    txttotal.Text = prT.ToString("#,#", CultureInfo.InvariantCulture); ;
                    /********************************************************/
                    //                    txttotal.Text = (Convert.ToInt32(txtsubtotal.Text) - Convert.ToDouble(txtsubtotal.Text) * des).ToString();
                }
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
            }
        }

        /// <summary>The Value property represents the TextBoxValue of the control.</summary>
        /// <returns>The current TextBoxValue of the control</returns>      

        public int Value
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
            DependencyProperty.Register("Value", typeof(int), typeof(Ventas),
              new PropertyMetadata(0, new PropertyChangedCallback(OnSomeValuePropertyChanged)));


        private static void OnSomeValuePropertyChanged(
        DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Ventas numericBox = target as Ventas;
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
            DependencyProperty.Register("Maximum", typeof(int), typeof(Ventas), new UIPropertyMetadata(100));

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
            DependencyProperty.Register("Minimum", typeof(int), typeof(Ventas), new UIPropertyMetadata(0));


        // Value changed
        private static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Ventas));

        /// <summary>The ValueChanged event is called when the TextBoxValue of the control changes.</summary>
        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        //Increase button clicked
        private static readonly RoutedEvent IncreaseClickedEvent =
            EventManager.RegisterRoutedEvent("IncreaseClicked", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Ventas));

        /// <summary>The IncreaseClicked event is called when the Increase button clicked</summary>
        public event RoutedEventHandler IncreaseClicked
        {
            add { AddHandler(IncreaseClickedEvent, value); }
            remove { RemoveHandler(IncreaseClickedEvent, value); }
        }

        //Increase button clicked
        private static readonly RoutedEvent DecreaseClickedEvent =
            EventManager.RegisterRoutedEvent("DecreaseClicked", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Ventas));

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
            if (e.IsDown && e.Key == Key.Up && Value < Maximum)
            {
                Value++;
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
            else if (e.IsDown && e.Key == Key.Down && Value > Minimum)
            {
                Value--;
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));

            }
        }


        //############################################################
        //################           Ventas       ##################
        //############################################################


        private void llenartablaVentas()
        {
            DataGridTextColumn colIdProd = new DataGridTextColumn();
            DataGridTextColumn colnombreProd = new DataGridTextColumn();
            DataGridTextColumn colPrecioProd = new DataGridTextColumn();
            DataGridTextColumn colcantidadProd = new DataGridTextColumn();
            DataGridTextColumn coltotalProd = new DataGridTextColumn();

            colIdProd.Header = "idProducto";
            colnombreProd.Header = "Nombre";
            colPrecioProd.Header = "Precio";
            colcantidadProd.Header = "Cantidad";
            coltotalProd.Header = "Total";




            datagridVentas.Columns.Add(colIdProd);
            datagridVentas.Columns.Add(colnombreProd);
            datagridVentas.Columns.Add(colPrecioProd);
            datagridVentas.Columns.Add(colcantidadProd);
            datagridVentas.Columns.Add(coltotalProd);


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
        /*********************************/


        /*********************************/


        //buscar producto mediante id ingresado
        private void codProdVenta_TextChanged(object sender, TextChangedEventArgs e)
        {

            ProductoFacade prodF = new ProductoFacade();
            bool existe = prodF.getExisteProductoByidProd(txtcodProdVenta.Text);


            if (existe)
            {
                ProductoFacade prodFobtener = new ProductoFacade();
                Producto Prod = new Producto();
                Prod = prodFobtener.getProductosByID(txtcodProdVenta.Text);
                //Si producto ya esta en tablapara venta aumentar cantidad
                if (Convert.ToInt32(Prod.stock) <= 0)
                {
                    MessageBox.Show("El producto no cuenta con más stock", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {

                    //comparar en datagrid si esta
                    bool sumarCantidad = false;
                    int cantidad = 0;
                    int totaldatos = datagridVentas.Items.Count;
                    int filaProd = 0;
                    List<VentaTemporal> vtemporal = new List<VentaTemporal>();
                    VentaTemporal vtemp = new VentaTemporal();
                    var rows = GetDataGridRows(datagridVentas);
                    int num = 0;
                    //Se verifica si tabla tiene ya ese producto y asi aumentar canditad
                    if (totaldatos > 0)
                    { //Llenar lista con los productos en datagrid para sumarle uno mas en cantidad
                        foreach (DataGridRow r in rows)
                        {
                            VentaTemporal rv = (VentaTemporal)r.Item;
                            vtemporal.Add(rv);

                        }
                        foreach (DataGridRow r in rows)
                        {
                            VentaTemporal rv = (VentaTemporal)r.Item;
                            //MessageBox.Show(txtcodProdVenta.Text+"="+rv.idProducto);
                            if (txtcodProdVenta.Text.Equals(rv.idProducto))
                            {
                                sumarCantidad = true;
                                /*int pre = ToEntero(rv.precio, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                                string m = pre.ToString("#,#", CultureInfo.InvariantCulture);
                                MessageBox.Show(rv.precio);*/
                                vtemp = new VentaTemporal(rv.idVenta, rv.idProducto, rv.nombre, rv.precio, rv.cantidad, rv.total);
                                cantidad = Convert.ToInt32(rv.cantidad) + 1;
                                filaProd = num;
                            }
                            num = num + 1;
                        }
                    }

                    //Verificar stock de producto
                    ProductoFacade prodcantiFacade = new ProductoFacade();
                    int maxstock = Convert.ToInt32(prodcantiFacade.getStockProductoByidProd(txtcodProdVenta.Text));
                    if (cantidad <= maxstock)
                    {

                        if (sumarCantidad)
                        {
                            //Editar cantidad y setear total precio

                            int totalProducto = Convert.ToInt32(Prod.precio) * Convert.ToInt32(cantidad);
                            //borrar fila delista con id y actualizar la cantidad y precio
                            vtemporal[filaProd].cantidad = cantidad.ToString();
                            vtemporal[filaProd].total = totalProducto.ToString();

                            // subtotal = subtotal + totalProducto;

                            datagridVentas.ItemsSource = null;
                            datagridVentas.ItemsSource = vtemporal;

                            //Llenar textbox con total

                        }
                        else
                        {
                            //MessageBox.Show("Primer producto agregado");
                            //Nuevo producto para venta a  proceso de venta
                            venta.Add(new VentaTemporal { idProducto = Prod.idProducto, nombre = Prod.nombre, precio = Prod.precio, cantidad = "1", total = Prod.precio });
                            datagridVentas.ItemsSource = venta;
                            subtotal = subtotal + Convert.ToInt32(Prod.precio);
                        }
                        //txtcodProdVenta.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("El producto no cuenta con más stock", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                txtcodProdVenta.Text = "";
            }
            else
            {


            }
            //Recorrer tabla para sumar total
            subtotal = 0;
            int numProducto = 0;
            foreach (var i in venta)
            {
                subtotal = subtotal + Convert.ToInt32(i.total);
                //MessageBox.Show(i.total.ToString());
                numProducto = numProducto + 1;
            }
            int mtotalDevo = subtotal - dineroDevolucion;
            if (mtotalDevo < 0)
            {
                //Quiere decir que totaldevolucion tiene saldo 
                txtDiferencia.Text = (mtotalDevo * -1).ToString();
                mtotalDevo = 0;

            }
            else

            {
                txtDiferencia.Text = "0";
            }

            if (mtotalDevo < 0)
            {
                mtotalDevo = mtotalDevo * -1;
            }
            int p = ToEntero(mtotalDevo.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
            txtsubtotal.Text = p.ToString("#,#", CultureInfo.InvariantCulture);


            txtCantidadProductos.Content = numProducto.ToString();
            if (Convert.ToInt32(txtdescuento.Text) == 0)
            {
                int pr = ToEntero(txtsubtotal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                txttotal.Text = pr.ToString("#,#", CultureInfo.InvariantCulture);

            }
            else
            {
                int pr = ToEntero(txtsubtotal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));

                double des = Convert.ToDouble(txtdescuento.Text) / Convert.ToInt32(100);
                double res = (Convert.ToInt32(pr) - Convert.ToDouble(pr) * des);
                txttotal.Text = res.ToString("#,#", CultureInfo.InvariantCulture);
            }

        }

        private void btnBorrarProdVenta_Click(object sender, RoutedEventArgs e)
        {
            //pasar datos a lista y luego borrar posicion obtenida y actualizar datagrid
            VentaTemporal customer = datagridVentas.SelectedItem as VentaTemporal;
            /*venta.Remove(customer);*/
            int totaldatos = datagridVentas.Items.Count;
            int filaProd = 0;
            List<VentaTemporal> vtemporal = new List<VentaTemporal>();
            VentaTemporal vtemp = new VentaTemporal();
            var rows = GetDataGridRows(datagridVentas);
            int num = 0;
            int precioProd = 0;
            //Llenar lista con los productos en datagrid para sumarle uno mas en cantidad


            foreach (DataGridRow r in rows)
            {
                VentaTemporal rv = (VentaTemporal)r.Item;
                vtemporal.Add(rv);

            }

            foreach (var r in vtemporal)
            {
                if (customer.idProducto.Equals(r.idProducto))
                {
                    filaProd = num;
                    precioProd = Convert.ToInt32(r.total);
                }
                num = num + 1;
            }
            subtotal = subtotal - precioProd;
            venta.RemoveAt(filaProd);
            //Setear subtotal de venta 
            if (venta.Count == 0)
            {
                txtsubtotal.Text = "0";
                subtotal = 0;
            }
            datagridVentas.ItemsSource = venta;

            if (subtotal < 0)
            {
                subtotal = 0;
            }
            int numProducto = 0;
            foreach (var i in venta)
            {
                subtotal = subtotal + Convert.ToInt32(i.total);
                //MessageBox.Show(i.total.ToString());
                numProducto = numProducto + 1;
            }

            txtCantidadProductos.Content = numProducto.ToString();
            txtsubtotal.Text = subtotal.ToString();

            int prT = ToEntero(subtotal.ToString(), NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
            txttotal.Text = prT.ToString("#,#", CultureInfo.InvariantCulture); ;


            int d = subtotal - dineroDevolucion;
            if (d < 0)
            {
                txtDiferencia.Text = "0";
            }
            else
            {
                txtDiferencia.Text = d.ToString();

            }


        }
        //Colorear row de datagrid 
        void dataGrid1_MouseMove(object sender, MouseEventArgs e)
        {

            // MessageBox.Show("fsdf");
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            // iteratively traverse the visual tree
            while ((dep != null) &&
                    !(dep is DataGridRow))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null)
                return;

            if (dep is DataGridRow)
            {
                DataGridRow row = dep as DataGridRow;
                if (row.IsMouseOver && previousRow != row)
                {

                    row.Background = new SolidColorBrush(Colors.LightGray);
                    if (previousRow != null)
                    {
                        previousRow.Background = new SolidColorBrush(Colors.White);
                    }
                }
                previousRow = row;
            }
        }
        private void btnCancelarVenta_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ventaTemp.idProducto))
            {
                if (MessageBox.Show("Hay un proceso de cambio de producto.¿Esta seguro de cancelar operación?", "Cambiar Vendedor", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    limpiarRegistroVentas();
                    btnPagar.Content = "Pagar";
                    ventaTemp = new VentaTemporal();
                    ltotalDevolucion.Visibility = Visibility.Hidden;
                    txtTotaldevolucion.Visibility = Visibility.Hidden;
                    lDiferencia.Visibility = Visibility.Hidden;
                    txtDiferencia.Visibility = Visibility.Hidden;
                    dineroDevolucion = 0;
                }
            }
            else
            {
                limpiarRegistroVentas();
            }


        }

        private void btnPagar_Click(object sender, RoutedEventArgs e)
        {


            int value = cbTipoPago.SelectedIndex;
            List<VentaTemporal> listVentatemporal = new List<VentaTemporal>();
            List<MVentas> listVentas = new List<MVentas>();
            List<Producto> listProducto = new List<Producto>();
            MVentas v = new MVentas();
            string tipopago = "";

            //string rutVendedor = "";

            DateTime fechaactual = DateTime.Now.Date;
            //string fechaactual = f.ToShortDateString();

            if (datagridVentas.Items.Count != 0)
            {
                switch (value)
                {
                    case 0:
                        //MessageBox.Show("efectivo");
                        tipopago = "efectivo";
                        rutcliente = "";
                        break;
                    case 1:
                        //MessageBox.Show("cuenta");
                        tipopago = "cuenta";
                        rutcliente = "";
                        break;
                    case 2:
                        //MessageBox.Show("debito");
                        tipopago = "debito";
                        rutcliente = "";
                        break;
                    case 3:
                        //MessageBox.Show("cheque");
                        /*PagoconCheque pc = new PagoconCheque();
                        pc.ShowDialog();*/

                        //Guardar cheque
                        tipopago = "cheque";

                        //Cheque chequeSave = new Cheque(getCheque.rut, getCheque.nombre, getCheque.nombreBanco, getCheque.fechaemision, getCheque.fechaexpiracion, getCheque.monto);



                        //MessageBox.Show(getCheque.rut);
                        break;
                }
                //Obtener datos de datagrid para guardar
                rutcliente = getCheque.rut;
                var rows = GetDataGridRows(datagridVentas);
                foreach (DataGridRow r in rows)
                {
                    VentaTemporal rv = (VentaTemporal)r.Item;
                    listVentatemporal.Add(rv);
                }
                foreach (var item in listVentatemporal)
                {
                    int p = ToEntero(txttotal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    MVentas vts = new MVentas(item.idVenta, item.idProducto, rutcliente, rutLogin, Convert.ToInt32(item.cantidad), Convert.ToDouble(p), fechaactual, tipopago
                        );
                    listVentas.Add(vts);

                }
                //Guardas lista de ventas

                if (rbtnSi.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(rutcliente))
                    {
                        //guardar detalles de deuda a cliente  y detalles de venta

                        //actualizar deuda y total de compras 
                        //MessageBox.Show("guardar usuario a fiar");
                        //Agregar registo a cliente de ventas 
                        //actualizar deuda y total de compras
                        clienteFacade clienteFac = new clienteFacade();
                        //MessageBox.Show("rut cliente a fiar:"+rutcliente);
                        int p = ToEntero(txttotal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));


                        string rActu = clienteFac.actualizar_DFT_Cliente(rutcliente, Convert.ToDouble(p), fechaactual);
                        if (rActu.Equals(""))
                        {
                            MessageBox.Show("Deuda ingresada a usuario:" + nombreCliente + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                            ventasFacade vtfac = new ventasFacade();
                            string resp = vtfac.GuardarVentas(listVentas);

                            if (resp.Equals(""))
                            {
                                int idventaGen = vtfac.getUltimoIngresadoenVentas();
                                MessageBox.Show("Anotar el siguiente idventa para posible devolucion:" + idventaGen.ToString() + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                vendedorFacade vendFac = new vendedorFacade();
                                vendFac.actualizarVentasVend(rutLogin);
                                //Descontar stock en (lista) de producto
                                ProductoFacade prd = new ProductoFacade();
                                foreach (var item in listVentas)
                                {
                                    string res = prd.actualizarStockProducto(item.idProducto.ToString(), item.cantidad.ToString());
                                    if (resp.Equals(""))
                                    {
                                        //MessageBox.Show("stock actualizado:" + item.idProducto);
                                    }
                                }
                                if (!string.IsNullOrEmpty(getCheque.nombre))
                                {
                                    chequeFacade chFac = new chequeFacade();
                                    getCheque = pc.getformCheque();
                                    string rep = chFac.GuardarCheque(getCheque);
                                    if (rep.Equals(""))
                                    {
                                        MessageBox.Show("Detalles cheque guardado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error al guardar detalles cheque:" + rep + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                if (!string.IsNullOrEmpty(ventaTemp.idProducto))
                                {
                                    int difDevo = Convert.ToInt32(ventaTemp.cantidad) - Convert.ToInt32(ventaTemp.devolver);
                                    //MessageBox.Show("dif:" + difDevo.ToString());
                                    if (difDevo == 0)
                                    {//si cantidad devolucion es igul a cantidad venta, actualizar stock producto y borrar registro de venta
                                        int sumStock = Convert.ToInt32(ventaTemp.devolver);
                                        ProductoFacade prodFac = new ProductoFacade();
                                        string actStock = prodFac.actualizarStockProductoDevolucion(ventaTemp.idProducto, sumStock);
                                        ventasFacade ventFac = new ventasFacade();
                                        string borrar = ventFac.borrarventaByidVenta(Convert.ToDouble(ventaTemp.idVenta), ventaTemp.idProducto, Convert.ToDateTime(fechaDevolucion));

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
                                            MessageBox.Show("Devolucion cambiada correctamente.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                            btnPagar.Content = "Pagar";
                                            ventaTemp = new VentaTemporal();
                                            ltotalDevolucion.Visibility = Visibility.Hidden;
                                            txtTotaldevolucion.Visibility = Visibility.Hidden;
                                            lDiferencia.Visibility = Visibility.Hidden;
                                            txtDiferencia.Visibility = Visibility.Hidden;
                                            dineroDevolucion = 0;

                                        }
                                    }
                                    else
                                    {//si cantidad devolucion no es igual a cantidad en venta , actualizar venta e stock producto
                                        int sumStock = Convert.ToInt32(ventaTemp.devolver);
                                        ProductoFacade prodFac = new ProductoFacade();
                                        string actStock = prodFac.actualizarStockProductoDevolucion(ventaTemp.idProducto, sumStock);
                                        ventasFacade ventFac = new ventasFacade();
                                        string actVenta = ventFac.actualizarventaDevolucion(Convert.ToDouble(ventaTemp.idVenta), ventaTemp.idProducto, Convert.ToDateTime(fechaDevolucion), difDevo, difDevo * Convert.ToInt32(ventaTemp.precio));

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
                                            MessageBox.Show("Devolucion cambiada correctamente.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);

                                            btnPagar.Content = "Pagar";
                                            ventaTemp = new VentaTemporal();
                                            ltotalDevolucion.Visibility = Visibility.Hidden;
                                            txtTotaldevolucion.Visibility = Visibility.Hidden;
                                            lDiferencia.Visibility = Visibility.Hidden;
                                            txtDiferencia.Visibility = Visibility.Hidden;
                                            dineroDevolucion = 0;
                                        }

                                    }
                                }
                                limpiarRegistroVentas();
                            }
                            else
                            {
                                MessageBox.Show("Error al guardar detalle ventas:" + resp, "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }


                        }
                        else
                        {
                            MessageBox.Show("Error al ingresar deuda a usuario:" + rActu, "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        // MessageBox.Show("Elegir usuario al que se fiara.");
                        DescuentoCliente dc = new DescuentoCliente("ds");
                        dc.Owner = Window.GetWindow(this); ;
                        dc.btnCancelar.Visibility = Visibility.Visible;
                        dc.Title = "Elegir Cliente";
                        dc.ShowDialog();

                        //txtdescuento.Text = dc.getValor();
                        getCheque.rut = dc.getrut();
                        rutcliente = dc.getrut();
                        nombreCliente = dc.getnombreCliente();
                        if (!rutcliente.Equals(""))
                        {
                            //actualizar deuda y total de compras 
                            //MessageBox.Show("Guardar usuario a fiar", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                            //Agregar registo a cliente de ventas 
                            //actualizar deuda y total de compras
                            clienteFacade clienteFac = new clienteFacade();
                            //MessageBox.Show("rut cliente a fiar:"+rutcliente);
                            string rActu = clienteFac.actualizar_DFT_Cliente(rutcliente, Convert.ToDouble(txttotal.Text), fechaactual);
                            if (rActu.Equals(""))
                            {
                                MessageBox.Show("Deuda ingresada a usuario:" + nombreCliente + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                ventasFacade vtfac = new ventasFacade();
                                string resp = vtfac.GuardarVentas(listVentas);

                                if (resp.Equals(""))
                                {
                                    int idventaGen = vtfac.getUltimoIngresadoenVentas();
                                    MessageBox.Show("Anotar el siguiente idventa para posible devolucion:" + idventaGen.ToString(), "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                                    vendedorFacade vendFac = new vendedorFacade();
                                    vendFac.actualizarVentasVend(rutLogin);
                                    //Descontar stock en (lista) de producto
                                    ProductoFacade prd = new ProductoFacade();
                                    foreach (var item in listVentas)
                                    {
                                        string res = prd.actualizarStockProducto(item.idProducto.ToString(), item.cantidad.ToString());
                                        if (resp.Equals(""))
                                        {
                                            //MessageBox.Show("stock actualizado:" + item.idProducto);
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(getCheque.nombre))
                                    {
                                        chequeFacade chFac = new chequeFacade();
                                        getCheque = pc.getformCheque();
                                        string rep = chFac.GuardarCheque(getCheque);
                                        if (rep.Equals(""))
                                        {
                                            MessageBox.Show("Detalles cheque guardado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);

                                        }
                                        else
                                        {
                                            MessageBox.Show("Error al guardar detalles cheque:" + rep + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                        }
                                    }
                                    //Si esta en proceso de devolucion btn se llamara cambiar
                                    if (!string.IsNullOrEmpty(ventaTemp.idProducto))
                                    {
                                        int difDevo = Convert.ToInt32(ventaTemp.cantidad) - Convert.ToInt32(ventaTemp.devolver);
                                        //MessageBox.Show("dif:" + difDevo.ToString());
                                        if (difDevo == 0)
                                        {//si cantidad devolucion es igul a cantidad venta, actualizar stock producto y borrar registro de venta
                                            int sumStock = Convert.ToInt32(ventaTemp.devolver);
                                            ProductoFacade prodFac = new ProductoFacade();
                                            string actStock = prodFac.actualizarStockProductoDevolucion(ventaTemp.idProducto, sumStock);
                                            ventasFacade ventFac = new ventasFacade();
                                            string borrar = ventFac.borrarventaByidVenta(Convert.ToDouble(ventaTemp.idVenta), ventaTemp.idProducto, Convert.ToDateTime(fechaDevolucion));

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
                                                MessageBox.Show("Devolucion cambiada correctamente.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                                btnPagar.Content = "Pagar";
                                                ventaTemp = new VentaTemporal();
                                                ltotalDevolucion.Visibility = Visibility.Hidden;
                                                txtTotaldevolucion.Visibility = Visibility.Hidden;
                                                lDiferencia.Visibility = Visibility.Hidden;
                                                txtDiferencia.Visibility = Visibility.Hidden;
                                                dineroDevolucion = 0;

                                            }
                                        }
                                        else
                                        {//si cantidad devolucion no es igual a cantidad en venta , actualizar venta e stock producto
                                            int sumStock = Convert.ToInt32(ventaTemp.devolver);
                                            ProductoFacade prodFac = new ProductoFacade();
                                            string actStock = prodFac.actualizarStockProductoDevolucion(ventaTemp.idProducto, sumStock);
                                            ventasFacade ventFac = new ventasFacade();
                                            string actVenta = ventFac.actualizarventaDevolucion(Convert.ToDouble(ventaTemp.idVenta), ventaTemp.idProducto, Convert.ToDateTime(fechaDevolucion), difDevo, difDevo * Convert.ToInt32(ventaTemp.precio));

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
                                                MessageBox.Show("Devolucion cambiada correctamente.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                                btnPagar.Content = "Pagar";
                                                ventaTemp = new VentaTemporal();
                                                ltotalDevolucion.Visibility = Visibility.Hidden;
                                                txtTotaldevolucion.Visibility = Visibility.Hidden;
                                                lDiferencia.Visibility = Visibility.Hidden;
                                                txtDiferencia.Visibility = Visibility.Hidden;
                                                dineroDevolucion = 0;
                                            }

                                        }
                                    }


                                    limpiarRegistroVentas();
                                }
                                else
                                {
                                    MessageBox.Show("Error al guardar detalle ventas:" + resp + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }


                            }
                            else
                            {
                                MessageBox.Show("Error al ingresar deuda a usuario:" + rActu + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }

                    //}
                }
                else if (rbtnNo.IsChecked == true)
                {
                    //MessageBox.Show("Guardar solo detalles venta:no fiar");

                    ventasFacade vtfac = new ventasFacade();
                    if (string.IsNullOrEmpty(rutcliente))
                    {
                        for (int i = 0; i < listVentas.Count; i++)
                        {
                            listVentas[i].rutCliente = "";

                        }
                        string resp = vtfac.GuardarVentas(listVentas);
                        if (resp.Equals(""))
                        {
                            //Descontar stock en (lista) de producto
                            ProductoFacade prd = new ProductoFacade();
                            foreach (var item in listVentas)
                            {
                                string res = prd.actualizarStockProducto(item.idProducto.ToString(), item.cantidad.ToString());
                                // MessageBox.Show("stock actualizado:" + item.idProducto);
                            }

                            int idventaGen = vtfac.getUltimoIngresadoenVentas();
                            MessageBox.Show("Anotar el siguiente idventa para posible devolucion:" + idventaGen.ToString() + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                            vendedorFacade vendFac = new vendedorFacade();
                            vendFac.actualizarVentasVend(rutLogin);
                            //MessageBox.Show("Detalles venta guardado");
                            //Agregar registo a cliente de ventas 
                            if (!string.IsNullOrEmpty(getCheque.nombre))
                            {
                                chequeFacade chFac = new chequeFacade();
                                getCheque = pc.getformCheque();
                                string rep = chFac.GuardarCheque(getCheque);
                                if (rep.Equals(""))
                                {
                                    MessageBox.Show("Detalles cheque guardado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Error al guardar detalles cheque:" + rep + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            if (!string.IsNullOrEmpty(ventaTemp.idProducto))
                            {
                                int difDevo = Convert.ToInt32(ventaTemp.cantidad) - Convert.ToInt32(ventaTemp.devolver);
                                //MessageBox.Show("dif:" + difDevo.ToString());
                                if (difDevo == 0)
                                {//si cantidad devolucion es igul a cantidad venta, actualizar stock producto y borrar registro de venta
                                    int sumStock = Convert.ToInt32(ventaTemp.devolver);
                                    ProductoFacade prodFac = new ProductoFacade();
                                    string actStock = prodFac.actualizarStockProductoDevolucion(ventaTemp.idProducto, sumStock);
                                    ventasFacade ventFac = new ventasFacade();
                                    string borrar = ventFac.borrarventaByidVenta(Convert.ToDouble(ventaTemp.idVenta), ventaTemp.idProducto, Convert.ToDateTime(fechaDevolucion));

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
                                        MessageBox.Show("Devolucion cambiada correctamente.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                        btnPagar.Content = "Pagar";
                                        ventaTemp = new VentaTemporal();
                                        ltotalDevolucion.Visibility = Visibility.Hidden;
                                        txtTotaldevolucion.Visibility = Visibility.Hidden;
                                        lDiferencia.Visibility = Visibility.Hidden;
                                        txtDiferencia.Visibility = Visibility.Hidden;
                                        dineroDevolucion = 0;
                                    }
                                }
                                else
                                {//si cantidad devolucion no es igual a cantidad en venta , actualizar venta e stock producto
                                    int sumStock = Convert.ToInt32(ventaTemp.devolver);
                                    ProductoFacade prodFac = new ProductoFacade();
                                    string actStock = prodFac.actualizarStockProductoDevolucion(ventaTemp.idProducto, sumStock);
                                    ventasFacade ventFac = new ventasFacade();
                                    string actVenta = ventFac.actualizarventaDevolucion(Convert.ToDouble(ventaTemp.idVenta), ventaTemp.idProducto, Convert.ToDateTime(fechaDevolucion), difDevo, difDevo * Convert.ToInt32(ventaTemp.precio));

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
                                        MessageBox.Show("Devolucion cambiada correctamente.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                        btnPagar.Content = "Pagar";
                                        ventaTemp = new VentaTemporal();
                                        ltotalDevolucion.Visibility = Visibility.Hidden;
                                        txtTotaldevolucion.Visibility = Visibility.Hidden;
                                        lDiferencia.Visibility = Visibility.Hidden;
                                        txtDiferencia.Visibility = Visibility.Hidden;
                                        dineroDevolucion = 0;
                                    }

                                }
                            }
                            limpiarRegistroVentas();

                        }
                        else
                        {
                            MessageBox.Show("Error al guardar detalle ventas:" + resp + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    //Si existe cliente pero no se fia de actualiza registro de venta a cliente
                    {
                        string resp = vtfac.GuardarVentas(listVentas);
                        if (resp.Equals(""))
                        {
                            //Descontar stock en (lista) de producto
                            ProductoFacade prd = new ProductoFacade();
                            foreach (var item in listVentas)
                            {
                                string res = prd.actualizarStockProducto(item.idProducto.ToString(), item.cantidad.ToString());
                                //MessageBox.Show("stock actualizado:" + item.idProducto);
                            }
                            int idventaGen = vtfac.getUltimoIngresadoenVentas();
                            MessageBox.Show("Anotar el siguiente idventa para posible devolucion:" + idventaGen.ToString() + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                            vendedorFacade vendFac = new vendedorFacade();
                            vendFac.actualizarVentasVend(rutLogin);
                            if (!string.IsNullOrEmpty(ventaTemp.idProducto))
                            {
                                int difDevo = Convert.ToInt32(ventaTemp.cantidad) - Convert.ToInt32(ventaTemp.devolver);
                                //MessageBox.Show("dif:" + difDevo.ToString());
                                if (difDevo == 0)
                                {//si cantidad devolucion es igul a cantidad venta, actualizar stock producto y borrar registro de venta
                                    int sumStock = Convert.ToInt32(ventaTemp.devolver);
                                    ProductoFacade prodFac = new ProductoFacade();
                                    string actStock = prodFac.actualizarStockProductoDevolucion(ventaTemp.idProducto, sumStock);
                                    ventasFacade ventFac = new ventasFacade();
                                    string borrar = ventFac.borrarventaByidVenta(Convert.ToDouble(ventaTemp.idVenta), ventaTemp.idProducto, Convert.ToDateTime(fechaDevolucion));

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
                                        MessageBox.Show("Devolucion cambiada correctamente.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                        btnPagar.Content = "Pagar";
                                        ventaTemp = new VentaTemporal();
                                        ltotalDevolucion.Visibility = Visibility.Hidden;
                                        txtTotaldevolucion.Visibility = Visibility.Hidden;
                                        lDiferencia.Visibility = Visibility.Hidden;
                                        txtDiferencia.Visibility = Visibility.Hidden;
                                        dineroDevolucion = 0;
                                    }
                                }
                                else
                                {//si cantidad devolucion no es igual a cantidad en venta , actualizar venta e stock producto
                                    int sumStock = Convert.ToInt32(ventaTemp.devolver);
                                    ProductoFacade prodFac = new ProductoFacade();
                                    string actStock = prodFac.actualizarStockProductoDevolucion(ventaTemp.idProducto, sumStock);
                                    ventasFacade ventFac = new ventasFacade();
                                    string actVenta = ventFac.actualizarventaDevolucion(Convert.ToDouble(ventaTemp.idVenta), ventaTemp.idProducto, Convert.ToDateTime(fechaDevolucion), difDevo, difDevo * Convert.ToInt32(ventaTemp.precio));

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
                                        MessageBox.Show("Devolucion cambiada correctamente.", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                        btnPagar.Content = "Pagar";
                                        ventaTemp = new VentaTemporal();
                                        ltotalDevolucion.Visibility = Visibility.Hidden;
                                        txtTotaldevolucion.Visibility = Visibility.Hidden;
                                        lDiferencia.Visibility = Visibility.Hidden;
                                        txtDiferencia.Visibility = Visibility.Hidden;
                                        dineroDevolucion = 0;
                                    }

                                }
                            }

                            //Agregar registo a cliente de ventas 
                            if (!string.IsNullOrEmpty(getCheque.nombre))
                            {
                                chequeFacade chFac = new chequeFacade();
                                getCheque = pc.getformCheque();
                                string rep = chFac.GuardarCheque(getCheque);
                                if (rep.Equals(""))
                                {
                                    MessageBox.Show("Detalles cheque guardado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    MessageBox.Show("Error al guardar detalles cheque:" + rep + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            clienteFacade clienteFac = new clienteFacade();
                            //MessageBox.Show("rut cliente a fiar:"+rutcliente);
                            string rActu = clienteFac.actualizar_DFT_Cliente(rutcliente, 0, fechaactual);
                            if (rActu.Equals(""))
                            {
                                MessageBox.Show("Registro de cliente actualizado", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);


                            }
                            else
                            {
                                MessageBox.Show("Error al actualizar registro cliente:" + rActu + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }



                            limpiarRegistroVentas();

                        }
                        else
                        {
                            MessageBox.Show("Error al guardar detalle ventas:" + resp + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }



                }
            }
            else
            {
                MessageBox.Show("Elegir al menos un producto para venta!", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        public void limpiarRegistroVentas()
        {
            //Limpiar registros de venta de interfaz
            venta.Clear();
            datagridVentas.ItemsSource = null;
            txtsubtotal.Text = "0";
            txtdescuento.Text = "0";
            txttotal.Text = "0";
            cbTipoPago.SelectedIndex = 0;
            txtCantidadProductos.Content = "0";
            getCheque.rut = "";
            pc = new PagoconCheque();
            rutcliente = "";
            nombreCliente = "";
            txtEntregado.Text = "0";
            txtVuelto.Text = "0";
            //txtTotaldevolucion.Text = "0";
            txtEntregado.Text = "0";
            txtDiferencia.Text = "0";



        }

        private void datagridVentas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridVentas.SelectedItem != null)
            {
                if (datagridVentas.SelectedItem is VentaTemporal)
                {
                    var row = (VentaTemporal)datagridVentas.SelectedItem;

                    if (row != null)
                    {
                        SetValue(ValueProperty, Convert.ToInt32(row.cantidad));
                    }
                }
            }

        }

        private void btnInfoCajero_Click(object sender, RoutedEventArgs e)
        {
            //For each dialog we use the same instance of ViewModel
            //var infoCajero = new InfoCajero();

            //No need to check DialogResult - it is respnsibility of ViewModel to interpret the result of commands
            //View only displays the window
            //infoCajero.ShowDialog();
        }

        private void btndeledesc_Button_Click(object sender, RoutedEventArgs e)
        {
            int prT = ToEntero(txtsubtotal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
            txttotal.Text = prT.ToString("#,#", CultureInfo.InvariantCulture); ;

            txtdescuento.Text = "0";
            rutcliente = "";
            nombreCliente = "";
        }
        private void btnadddesc_Button_Click(object sender, RoutedEventArgs e)
        {
            DescuentoCliente dc = new DescuentoCliente("ds");

            dc.ShowDialog();
            txtdescuento.Text = dc.getValor().ToString();
            getCheque.rut = dc.getrut();
            rutcliente = dc.getrut();
            nombreCliente = dc.getnombreCliente();

            double des = Convert.ToDouble(txtdescuento.Text) / Convert.ToInt32(100);
            int p = ToEntero(txtsubtotal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
            double res = (Convert.ToInt32(p) - Convert.ToInt32(p) * des);
            txttotal.Text = res.ToString("#,#", CultureInfo.InvariantCulture); ;

        }

        private void cbTipoPago_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int value = cbTipoPago.SelectedIndex;
            switch (value)
            {
                case 0:
                    //MessageBox.Show("efectivo");
                    break;
                case 1:
                    //MessageBox.Show("cuenta");
                    break;
                case 2:
                    //MessageBox.Show("debito");
                    break;
                case 3:
                    //MessageBox.Show("cheque");

                    pc = new PagoconCheque();
                    pc.Owner = Window.GetWindow(this);
                    pc.setDatauser(rutcliente, nombreCliente);
                    //getCheque = pc.getformCheque().;
                    //MessageBox.Show(pc.getformCheque().rut);
                    pc.Show();
                    // getCheque =new Cheque(pc.getformCheque().rut,pc.getformCheque().nombre,pc.getformCheque().nombreBanco,pc.getformCheque().fechaemision,pc.getformCheque().fechaexpiracion,pc.getformCheque().monto);

                    getCheque.rut = pc.getrutCheque();
                    //MessageBox.Show(getCheque.rut);
                    break;
            }
        }

        //############################################################
        //################           VALIDACION       ##################
        //############################################################


        private void textbox_NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }




        private void rbtnSi_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Rut usuario desde radioSI:"+rutcliente);
            //Verificar si radiobutton esta seleccionado para fiar 
            if (string.IsNullOrEmpty(rutcliente))
            {
                DescuentoCliente dc = new DescuentoCliente("ds");
                dc.Owner = Window.GetWindow(this);
                dc.btnCancelar.Visibility = Visibility.Visible;
                dc.Title = "Elegir Cliente";
                dc.ShowDialog();

                //txtdescuento.Text = dc.getValor();
                getCheque.rut = dc.getrut();
                rutcliente = dc.getrut();
                nombreCliente = dc.getnombreCliente();
                pc = new PagoconCheque();
                pc.Owner = Window.GetWindow(this);
                pc.setDatauser(rutcliente, nombreCliente);

            }
        }


        //Calcular el cambio segun dinero entregado para pagar  segun total
        private void txtEntregado_TextChanged(object sender, TextChangedEventArgs e)
        {


            if (!string.IsNullOrEmpty(txtEntregado.Text))
            {

                int p = ToEntero(txtEntregado.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                int t = ToEntero(txttotal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                txtEntregado.Text = p.ToString("#,#", CultureInfo.InvariantCulture);
                txtEntregado.Focus();
                txtEntregado.SelectionStart = txtEntregado.Text.Length;

                double total = Convert.ToDouble(t);
                double entregado = Convert.ToDouble(p);
                double vuelto = entregado - total;



                if (entregado > total)
                {
                    
                    txtVuelto.Text = vuelto.ToString("#,#", CultureInfo.InvariantCulture);

                }
                else
                {
                    try
                    {

                        txtVuelto.Text = "0";
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.ToString());
                    }

                }


            }

        }

        private void rbtnNo_Checked(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Detalle prodDevolver:" + dineroDevolucion.ToString());

        }

        private void bntCambiarVendedor_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ventaTemp.idProducto))
            {
                if (MessageBox.Show("Hay un proceso de cambio de producto.¿Esta seguro de cambiar vendedor y cancelar operación?", "Cambiar Vendedor", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    login login = new login();
                    login.setInstancia(transVenta);
                    transVenta.pageTransitionControl.ShowPage(login);
                }
            }
            else
            {
                login login = new login();
                login.setInstancia(transVenta);
                transVenta.pageTransitionControl.ShowPage(login);
            }
        }

        private void btnDevolucion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(ventaTemp.idProducto))
                {
                    if (MessageBox.Show("Hay un proceso de cambio de producto.¿Esta seguro de cancelar operación?", "Cambiar Vendedor", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        limpiarRegistroVentas();
                        btnPagar.Content = "Pagar";
                        ventaTemp = new VentaTemporal();
                        ltotalDevolucion.Visibility = Visibility.Hidden;
                        txtTotaldevolucion.Visibility = Visibility.Hidden;
                        lDiferencia.Visibility = Visibility.Hidden;
                        txtDiferencia.Visibility = Visibility.Hidden;
                        dineroDevolucion = 0;
                        Devolucion dev = new Devolucion();
                        dev.setInstancia(this);
                        dev.Show();
                    }
                }
                else
                {
                    Devolucion dev = new Devolucion();
                    dev.setInstancia(this);
                    dev.Show();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private int ToEntero(string value, NumberStyles style, IFormatProvider provider)
        {
            try
            {
                int number = Int32.Parse(value, style, provider);
                //Console.WriteLine("Converted '{0}' to {1}.", value, number);
                return number;
            }
            catch (FormatException)
            {
                //MessageBox.Show("Unable to convert '{0}'.", value);
                return 0;
            }
            catch (OverflowException)
            {
                MessageBox.Show("'{0}' Numero fuera de rango para tipo Int32.", value);
                return 0;
            }
        }

    }
}
