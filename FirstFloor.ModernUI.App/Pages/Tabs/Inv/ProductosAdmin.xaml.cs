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
using FirstFloor.ModernUI.App.Control;
using FirstFloor.ModernUI.App.Modelo;
using System.Globalization;

namespace FirstFloor.ModernUI.App.Pages.Tabs.Inv
{
    /// <summary>
    /// Interaction logic for TabPage1.xaml
    /// </summary>
    public partial class ProductosAdmin : UserControl
    {
        CultureInfo us = new CultureInfo("en-US");
        List<Producto> ListProductos = new List<Producto>();
        CollectionViewSource itemCollectionViewSource;
        Codigos tabCodigos = new Codigos();
        TransLoginToProductosAdmin translogin = new TransLoginToProductosAdmin();
        public ProductosAdmin()
        {
            InitializeComponent();
            //METODOS
            InitializeComponent();
            llenarArbolCategoria();
            llenarComboBCategoria();
            llenarTablaProducto();
            /*llenarTablaCliente();
            llenarTablaVendedor();
            llenarTablaProductoCodBarra();
            llenarArbolCategoriaCodBarra();
            CantidadTotalRubrosyProductos();*/
        }

        public void setContent(TransLoginToProductosAdmin t)
        {
            translogin = t;

        }
        public TransLoginToProductosAdmin getContent()
        {
            return translogin;
        }
        public void setInstCodigo(Codigos c)
        {
            tabCodigos = c;
        }

        //############################################################
        //################           VALIDACION       ##################
        //############################################################

        private void textbox_NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }



        private void cbTipoCodigo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int sel = cbTipoCodigo.SelectedIndex;
                //MessageBox.Show(sel.ToString());
                if (sel == 0)
                {
                    this.txtcodprod.IsEnabled = false;
                }
                else if (sel == 1)
                {
                    this.txtcodprod.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }

        }

        //############################################################
        //################           PRODUCTO       ##################
        //############################################################
        public void llenarTablaProducto()
        {

            ProductoFacade prodF = new ProductoFacade();
            var itemList = new List<Producto>();
            var listaProd = prodF.getProductos();
            ListProductos.Clear();
            datagridProducto.ItemsSource = null;
            if (listaProd.Count > 0)
            {
                foreach (var item in listaProd)
                {
                    int p = ToEntero(item.precioReal, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string m = p.ToString("#,#", CultureInfo.InvariantCulture);
                    int p1 = ToEntero(item.precio, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string m1 = p1.ToString("#,#", CultureInfo.InvariantCulture);
                    int st = ToEntero(item.stock, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string stp = st.ToString("#,#", CultureInfo.InvariantCulture);
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = stp, precioReal = m, precio = m1, idCategoria = item.idCategoria, fecha = item.fecha });
                }

                datagridProducto.ItemsSource = ListProductos;
                btnEditarProducto.IsEnabled = true;
                btnEliminarProducto.IsEnabled = true;
                btnEliminarTodoProducto.IsEnabled = true;
            }
            else
            {
                // DateTime fvacio = Convert.ToDateTime("15/08/2008");
                ListProductos.Add(new Producto { idProducto = "Sin productos", nombre = "", stock = "", precioReal = "", precio = "", idCategoria = 0 });

                datagridProducto.ItemsSource = ListProductos;
                btnEditarProducto.IsEnabled = false;
                btnEliminarProducto.IsEnabled = false;
                btnEliminarTodoProducto.IsEnabled = false;

            }
        }
        public void llenarTablaProductobynombre(string nombre)
        {

            ProductoFacade prodF = new ProductoFacade();
            var itemList = new List<Producto>();
            var listaProd = prodF.getProductobyNombre(nombre);
            ListProductos.Clear();
            datagridProducto.ItemsSource = null;
            if (listaProd.Count > 0)
            {
                foreach (var item in listaProd)
                {
                    int p = ToEntero(item.precioReal, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string m = p.ToString("#,#", CultureInfo.InvariantCulture);
                    int p1 = ToEntero(item.precio, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string m1 = p1.ToString("#,#", CultureInfo.InvariantCulture);
                    int st = ToEntero(item.stock, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string stp = st.ToString("#,#", CultureInfo.InvariantCulture);
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = stp, precioReal = m, precio = m1, idCategoria = item.idCategoria, fecha = item.fecha });
                }

                datagridProducto.ItemsSource = ListProductos;
            }
            else
            {
                // DateTime fvacio = Convert.ToDateTime("15/08/2008");
                ListProductos.Add(new Producto { idProducto = "Sin productos", nombre = "", stock = "", precioReal = "", precio = "", idCategoria = 0 });

                datagridProducto.ItemsSource = ListProductos;

            }
        }
        public void llenarTablaProductobyidCodigo(string idCodigo)
        {

            ProductoFacade prodF = new ProductoFacade();
            var itemList = new List<Producto>();
            var listaProd = prodF.getProductobyCodigo(idCodigo);
            ListProductos.Clear();
            datagridProducto.ItemsSource = null;
            if (listaProd.Count > 0)
            {
                foreach (var item in listaProd)
                {
                    int p = ToEntero(item.precioReal, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string m = p.ToString("#,#", CultureInfo.InvariantCulture);
                    int p1 = ToEntero(item.precio, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string m1 = p1.ToString("#,#", CultureInfo.InvariantCulture);
                    int st = ToEntero(item.stock, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string stp = st.ToString("#,#", CultureInfo.InvariantCulture);
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = stp, precioReal = m, precio = m1, idCategoria = item.idCategoria, fecha = item.fecha });
                }

                datagridProducto.ItemsSource = ListProductos;
            }
            else
            {
                // DateTime fvacio = Convert.ToDateTime("15/08/2008");
                ListProductos.Add(new Producto { idProducto = "Sin productos", nombre = "", stock = "", precioReal = "", precio = "", idCategoria = 0 });

                datagridProducto.ItemsSource = ListProductos;

            }
        }
        private void limpiarTxtProducto()
        {
            txtcodprod.Text = "";
            txtnombreproducto.Text = "";
            txtstock.Text = "";
            txtprecioReal.Text = "";
            txtprecio.Text = "";

        }
        private void btnGuardarProducto_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(txtnombreproducto.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtstock.Text))
                {
                    if (!string.IsNullOrWhiteSpace(txtprecioReal.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(txtprecio.Text))
                        {
                            int compra = ToEntero(txtprecioReal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                            int venta = ToEntero(txtprecio.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                            if (compra < venta)
                            {


                                if (cbTipoCodigo.SelectedIndex == 1)
                                {
                                    if (!string.IsNullOrWhiteSpace(txtcodprod.Text))
                                    {
                                        //ingresar producto con id ingresado por usuario


                                        if (cbCategoria.Items.Count > 0)
                                        {
                                            ProductoFacade prodF = new ProductoFacade();

                                            string codProd = txtcodprod.Text;
                                            string nombreProd = txtnombreproducto.Text;
                                            string nombreCat = cbCategoria.SelectedValue.ToString();
                                            
                                            int st = ToEntero(txtstock.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                                            string stock = st.ToString();

                                            int pR = ToEntero(txtprecioReal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                                            string precioReal = pR.ToString();
                                            int p = ToEntero(txtprecio.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                                            string precio = p.ToString();
                                            //string res=prodF.GuardarProducto()

                                            //Agregar idCategoria al idProducto + el numItem
                                            ProductoFacade prodFCod = new ProductoFacade();
                                            categoriaFacade catFCod = new categoriaFacade();
                                            List<Producto> listProducto = prodFCod.getProductosBynombreCategoria(nombreCat);

                                            //Verificar si producto con nombre y categoria existe
                                            ProductoFacade prodFExiste = new ProductoFacade();
                                            bool existeProd = prodFExiste.getExisteProductoBynombreYidCat(nombreProd, nombreCat);
                                            if (existeProd)
                                            {
                                                MessageBox.Show("Producto con nombre:\"" + nombreProd + "\" ya existe para categoria:\"" + nombreCat + "\"" + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                                            }
                                            else
                                            {

                                                DateTime fechaactual = DateTime.Now.Date;
                                                string res = prodF.GuardarProducto(txtcodprod.Text, nombreProd, stock, precioReal, precio, nombreCat, fechaactual, 1);
                                                if (res.Equals(""))
                                                {
                                                    MessageBox.Show("Producto guardado correctamente!", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                                    limpiarTxtProducto();
                                                    llenarTablaProductoBynombreCat(nombreCat);
                                                    cbTipoCodigo.SelectedIndex = 0;
                                                    tabCodigos.actualizarArbolyTablaProductos();
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Error al guardar producto:" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);

                                                }






                                            }


                                        }
                                        else
                                        {
                                            MessageBox.Show("Ingresar al menos 1 categoria!", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);


                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Ingresar idproducto a producto", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);


                                    }
                                }
                                else
                                {
                                    //ingresar idproductogenerado

                                    Guid guid = Guid.NewGuid();
                                    string a = guid.ToString();

                                    string idPnuevo = string.Empty;
                                    long val = 0;

                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        if (Char.IsDigit(a[i]))
                                            if (idPnuevo.Length < 8)
                                            {
                                                idPnuevo += a[i];
                                            }
                                            else
                                            {
                                                break;
                                            }
                                    }
                                    if (cbCategoria.Items.Count > 0)
                                    {
                                        ProductoFacade prodF = new ProductoFacade();


                                        string nombreProd = txtnombreproducto.Text;
                                        string nombreCat = cbCategoria.SelectedValue.ToString();
                                        int st = ToEntero(txtstock.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                                        string stock = st.ToString();
                                        int pR = ToEntero(txtprecioReal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                                        string precioReal = pR.ToString();
                                        int p = ToEntero(txtprecio.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                                        string precio = p.ToString();
                                        //string res=prodF.GuardarProducto()

                                        //Agregar idCategoria al idProducto + el numItem
                                        ProductoFacade prodFCod = new ProductoFacade();
                                        categoriaFacade catFCod = new categoriaFacade();
                                        List<Producto> listProducto = prodFCod.getProductosBynombreCategoria(nombreCat);

                                        //Verificar si producto con nombre y categoria existe
                                        ProductoFacade prodFExiste = new ProductoFacade();
                                        bool existeProd = prodFExiste.getExisteProductoBynombreYidCat(nombreProd, nombreCat);
                                        if (existeProd)
                                        {
                                            MessageBox.Show("Producto con nombre:\"" + nombreProd + "\" ya existe para categoria:\"" + nombreCat + "\"" + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                                        }
                                        else
                                        {
                                            //Producto ya tiene productos asociados a categoria agregar en la ultima posicion


                                            //Guardar producto con idproducto 
                                            DateTime fechaactual = DateTime.Now.Date;
                                            string res = prodF.GuardarProducto(idPnuevo, nombreProd, stock, precioReal, precio, nombreCat, fechaactual, 0);
                                            if (res.Equals(""))
                                            {
                                                MessageBox.Show("Producto guardado correctamente!", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                                limpiarTxtProducto();
                                                llenarTablaProductoBynombreCat(nombreCat);
                                                tabCodigos.actualizarArbolyTablaProductos();
                                                //llenarTablaProductoCodBarra();
                                                //CantidadTotalRubrosyProductos();
                                            }
                                            else
                                            {
                                                MessageBox.Show("Error al guardar producto:" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);

                                            }


                                        }


                                    }
                                    else
                                    {
                                        MessageBox.Show("Ingresar al menos 1 categoria!", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);


                                    }
                                }

                            }
                            else
                            {
                                MessageBox.Show("Precio de venta debe ser mayor a precio de compra(?!)", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                            }
                        }
                        else
                        {
                            MessageBox.Show("Ingresar Precio Venta a producto", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                    }
                    else
                    {
                        MessageBox.Show("Ingresar Precio Compra a producto", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                    }
                }
                else
                {
                    MessageBox.Show("Ingresar Stock!", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
            }
            else
            {
                MessageBox.Show("Ingresar nombre a producto", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        private void btnEditarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (datagridProducto.SelectedItem != null)
            {
                if (datagridProducto.SelectedItem is Producto)
                {
                    var row = (Producto)datagridProducto.SelectedItem;

                    if (row != null)
                    {
                        //MessageBox.Show(row.nombre);
                        btnGuardarProducto.Visibility = Visibility.Hidden;
                        btncancelarEditarProd.Visibility = Visibility.Visible;
                        btnGuardarActualizarProducto.Visibility = Visibility.Visible;
                        txtcodprod.Text = row.idProducto;
                        txtnombreproducto.Text = row.nombre;
                        txtstock.Text = row.stock;
                        int pR = ToEntero(row.precioReal, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                        txtprecioReal.Text = pR.ToString("#,#", CultureInfo.InvariantCulture);


                        int p = ToEntero(row.precio, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                        txtprecio.Text = p.ToString("#,#", CultureInfo.InvariantCulture);

                        ProductoFacade prodF = new ProductoFacade();
                        categoriaFacade catFac = new categoriaFacade();
                        string nombreCat = catFac.getCategoriaById(prodF.getIdCatbyidProd(row.idProducto));
                        //MessageBox.Show(nombreCat);
                        cbCategoria.SelectedValue = nombreCat;
                        cbTipoCodigo.IsEnabled = false;
                        txtcodprod.IsEnabled = false;

                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccionar Producto a editar", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }
        private void btnGuardarActualizarProducto_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(txtnombreproducto.Text))
            {
                if (!string.IsNullOrWhiteSpace(txtstock.Text))
                {
                    if (!string.IsNullOrWhiteSpace(txtprecioReal.Text))
                    {
                        if (!string.IsNullOrWhiteSpace(txtprecio.Text))
                        {
                            int compra = ToEntero(txtprecioReal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                            int venta = ToEntero(txtprecio.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                            
                            if (compra < venta)
                            {
                                //MessageBox.Show(val.ToString().Length.ToString());
                                if (cbCategoria.Items.Count > 0)
                                {

                                    ProductoFacade prodF = new ProductoFacade();

                                    string codProd = txtcodprod.Text;
                                    string nombreProd = txtnombreproducto.Text;
                                    string nombreCat = cbCategoria.SelectedValue.ToString();
                                    int st = ToEntero(txtstock.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                                    string stock = st.ToString();
                                    int pR = ToEntero(txtprecioReal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                                    string precioReal = pR.ToString();
                                    int p = ToEntero(txtprecio.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                                    string precio = p.ToString();
                                    //string res=prodF.GuardarProducto()

                                    //Agregar idCategoria al idProducto + el numItem
                                    ProductoFacade prodFCod = new ProductoFacade();
                                    categoriaFacade catFCod = new categoriaFacade();

                                    //verificar si cod es generado o ingresado por usuario
                                    int generado = prodF.getIdGenerado(txtcodprod.Text);
                                    DateTime fechaactual = DateTime.Now.Date;

                                    //actualizar con id que habia ingresado el usuario
                                    string res = prodF.ActualizarProducto(txtcodprod.Text, txtcodprod.Text, nombreProd, stock, precioReal, precio, nombreCat, fechaactual);
                                    if (res.Equals(""))
                                    {
                                        MessageBox.Show("Producto actualizado correctamente!", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                                        limpiarTxtProducto();
                                        btnGuardarProducto.Visibility = Visibility.Visible;
                                        btncancelarEditarProd.Visibility = Visibility.Hidden;
                                        btnGuardarActualizarProducto.Visibility = Visibility.Hidden;
                                        llenarTablaProductoBynombreCat(nombreCat);
                                        cbTipoCodigo.IsEnabled = true;
                                        tabCodigos.actualizarArbolyTablaProductos();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error al actualizar producto:" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);

                                    }


                                }
                                else
                                {
                                    MessageBox.Show("Ingresar al menos 1 categoria!", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                                }
                            }
                            else
                            {
                                MessageBox.Show("Precio de venta debe ser mayor a precio de compra(?!)", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                            }
                        }
                        else
                        {
                            MessageBox.Show("Ingresar precio venta a producto", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Ingresar precio compra a producto!", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
                else
                {
                    MessageBox.Show("Ingresar stock a producto", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            else
            {
                MessageBox.Show("Ingresar nombre a producto", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void btncancelarEditarProd_Click(object sender, RoutedEventArgs e)
        {
            btnGuardarProducto.Visibility = Visibility.Visible;
            btncancelarEditarProd.Visibility = Visibility.Hidden;
            btnGuardarActualizarProducto.Visibility = Visibility.Hidden;
            limpiarTxtProducto();
            cbTipoCodigo.IsEnabled = true;

        }

        private void btnEliminarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (datagridProducto.SelectedItem != null)
            {
                if (datagridProducto.SelectedItem is Producto)
                {
                    var row = (Producto)datagridProducto.SelectedItem;

                    if (row != null)
                    {
                        ProductoFacade prodF = new ProductoFacade();
                        categoriaFacade catFac = new categoriaFacade();
                        string catcargar = catFac.getCategoriaById(prodF.getIdCatbyidProd(row.idProducto));
                        string res = prodF.borrarProductoByid(row.idProducto);
                        if (res.Equals(""))
                        {
                            MessageBox.Show("Producto borrado ", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                            btnGuardarProducto.Visibility = Visibility.Visible;
                            btncancelarEditarProd.Visibility = Visibility.Hidden;
                            btnGuardarActualizarProducto.Visibility = Visibility.Hidden;
                            limpiarTxtProducto();
                            //recargar tabla segun item de arbolseleccionado
                            /*string selcat = treeViewCategoria.SelectedValue.ToString();
                            if (catcargar.Equals("All"))
                            {
                                llenarTablaProducto();
                            }
                            else
                            {*/
                            //treeViewCategoria.item=catcargar;
                            llenarTablaProductoBynombreCat(catcargar);
                            tabCodigos.actualizarArbolyTablaProductos();
                            //}
                            // CantidadTotalRubrosyProductos();

                        }
                        else
                        {
                            MessageBox.Show("Error al borrar producto:" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccionar producto a borrar", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void btnEliminarTodoProducto_Click(object sender, RoutedEventArgs e)
        {
            ProductoFacade prodFtotal = new ProductoFacade();
            int totalProd = prodFtotal.getTotalProductos();
            if (MessageBox.Show("Esta seguro de borrar " + totalProd.ToString() + " Productos ", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ProductoFacade prodF = new ProductoFacade();
                string res = prodF.borrarAllProducto();

                if (res.Equals(""))
                {
                    MessageBox.Show(totalProd + " Productos Eliminados" + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                    llenarTablaProducto();
                    btnGuardarProducto.Visibility = Visibility.Visible;
                    btncancelarEditarProd.Visibility = Visibility.Hidden;
                    btnGuardarActualizarProducto.Visibility = Visibility.Hidden;
                    limpiarTxtProducto();
                    tabCodigos.actualizarArbolyTablaProductos();
                    //CantidadTotalRubrosyProductos();
                }
                else
                {
                    MessageBox.Show("Error al borrar todos los productos:" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }




        //############################################################
        //################           CATEGORIA       ##################
        //############################################################
        public void llenarTablaProductoBynombreCat(string nombreCategoria)
        {
            ProductoFacade prodF = new ProductoFacade();
            var itemList = new List<Producto>();
            var listaProd = prodF.getProductosBynombreCategoria(nombreCategoria);
            ListProductos.Clear();
            datagridProducto.ItemsSource = null;
            if (listaProd.Count > 0)
            {
                foreach (var item in listaProd)
                {
                    int p = ToEntero(item.precioReal, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string m = p.ToString("#,#", CultureInfo.InvariantCulture);
                    int p1 = ToEntero(item.precio, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string m1 = p1.ToString("#,#", CultureInfo.InvariantCulture);
                    int st = ToEntero(item.stock, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
                    string stp = st.ToString("#,#", CultureInfo.InvariantCulture);
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = stp, precioReal = m, precio = m1, idCategoria = item.idCategoria, fecha = item.fecha });
                }

                datagridProducto.ItemsSource = ListProductos;
                btnEditarProducto.IsEnabled = true;
                btnEliminarProducto.IsEnabled = true;
                btnEliminarTodoProducto.IsEnabled = true;
            }
            else
            {
                ListProductos.Add(new Producto { idProducto = "Categoria sin productos", nombre = "", stock = "", precioReal = "", precio = "", idCategoria = 0 });

                datagridProducto.ItemsSource = ListProductos;
                btnEditarProducto.IsEnabled = false;
                btnEliminarProducto.IsEnabled = false;
                btnEliminarTodoProducto.IsEnabled = false;

            }

        }


        private void llenarComboBCategoria()
        {
            categoriaFacade catFlista = new categoriaFacade();
            List<Categoria> listaCategoria = catFlista.getCategoria();

            cbCategoria.Items.Clear();
            foreach (Categoria v in listaCategoria)
            {
                cbCategoria.Items.Add(v.nombreCategoria);

            }
            cbCategoria.SelectedIndex = 0;

        }
        private void llenarArbolCategoria()
        {

            treeViewCategoria.Items.Clear();
            TreeViewItem parent = new TreeViewItem();
            parent.Header = "Categorias";
            parent.IsSelected = true;
            treeViewCategoria.Items.Add(parent);


            TreeViewItem nuevoChild = new TreeViewItem();
            nuevoChild.Header = "All";
            nuevoChild.IsSelected = true;
            //newChild.Items.Add(newSubChild1);
            List<string> nombrecat = new List<string>();
            nombrecat.Add("All");

            categoriaFacade catFlista = new categoriaFacade();
            List<Categoria> listaCategoria = catFlista.getCategoria();
            foreach (Categoria v in listaCategoria)
            {
                nombrecat.Add(v.nombreCategoria);

            }


            parent.ItemsSource = nombrecat;


        }
        private void btnNuevaCategoria_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(txtNuevaCat.Text))
            {
                categoriaFacade catF = new categoriaFacade();
                categoriaFacade catFexiste = new categoriaFacade();

                string nombreCategoria = txtNuevaCat.Text;

                bool existe = catFexiste.getExisteCategoria(nombreCategoria);

                if (existe)
                {
                    MessageBox.Show("Categoria con nombre:" + nombreCategoria + " existe" + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {

                    string res = catF.GuardarCategoria(nombreCategoria);

                    if (res.Equals(""))
                    {
                        MessageBox.Show("Categoria Guardada", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                        txtNuevaCat.Text = "";
                        llenarArbolCategoria();
                        llenarComboBCategoria();
                        tabCodigos.actualizarArbolyTablaProductos();
                        //llenarArbolCategoriaCodBarra();
                        //llenarTablaProductoCodBarra();

                        //CantidadTotalRubrosyProductos();

                    }
                    else
                    {
                        MessageBox.Show("Error al guardar categoria:" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
            else
            {
                MessageBox.Show("Ingresar nombre Categoria!", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void btnEditarCategoria_Click(object sender, RoutedEventArgs e)
        {
            string catBorrar = treeViewCategoria.SelectedValue.ToString();
            if (catBorrar.Length < 67 && !catBorrar.Equals("All"))
            {
                btnGuardaActProducto.Visibility = Visibility.Visible;
                btnNuevaCategoria.Visibility = Visibility.Hidden;
                btncancelarEditarcat.Visibility = Visibility.Visible;
                txtNuevaCat.Text = catBorrar;
            }
            else
            {
                MessageBox.Show("Seleccionar un categoría para actualizar", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }




        }
        private void btnGuardaActProducto_Click(object sender, RoutedEventArgs e)
        {
            string nombrenuevaCat = txtNuevaCat.Text;
            if (!string.IsNullOrWhiteSpace(txtNuevaCat.Text))
            {

                string catActualizar = treeViewCategoria.SelectedValue.ToString();
                categoriaFacade catF = new categoriaFacade();
                string res = catF.actualizarCategoria(nombrenuevaCat, catActualizar);
                if (res.Equals(""))
                {
                    MessageBox.Show("Categoria actualizada", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                    btnGuardaActProducto.Visibility = Visibility.Hidden;
                    btnNuevaCategoria.Visibility = Visibility.Visible;
                    btncancelarEditarcat.Visibility = Visibility.Hidden;
                    txtNuevaCat.Text = "";
                    llenarArbolCategoria();
                    llenarComboBCategoria();
                    tabCodigos.actualizarArbolyTablaProductos();
                }
                else
                {
                    MessageBox.Show("Error al actualizar categoria:" + res + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Ingresar nombre a categoria", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
        private void btncancelarEditarcat_Click(object sender, RoutedEventArgs e)
        {
            btnGuardaActProducto.Visibility = Visibility.Hidden;
            btnNuevaCategoria.Visibility = Visibility.Visible;
            btncancelarEditarcat.Visibility = Visibility.Hidden;
            txtNuevaCat.Text = "";
        }

        private void btnEliminarCategoria_Click(object sender, RoutedEventArgs e)
        {
            //borrar Categoria y productos asociados
            categoriaFacade catF = new categoriaFacade();
            ProductoFacade prodF = new ProductoFacade();

            string catBorrar = treeViewCategoria.SelectedValue.ToString();
            if (catBorrar.Length < 67 && !catBorrar.Equals("All"))
            {
                int totalProd = prodF.getTotalProductosBynombreCat(catBorrar);

                if (totalProd > 0)
                {
                    if (MessageBox.Show("Esta seguro de borrar categoria:" + catBorrar + " con (" + totalProd + ") Productos Asociados", "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {

                        btnGuardaActProducto.Visibility = Visibility.Hidden;
                        btnNuevaCategoria.Visibility = Visibility.Visible;
                        btncancelarEditarcat.Visibility = Visibility.Hidden;
                        txtNuevaCat.Text = "";


                        bool res = prodF.borrarProductoYnombreCategoria(catBorrar);

                        if (res)
                        {
                            MessageBox.Show("Categoria  borrada", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                            llenarArbolCategoria();
                            llenarComboBCategoria();
                            tabCodigos.actualizarArbolyTablaProductos();
                            //CantidadTotalRubrosyProductos();
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("Esta seguro de borrar categoria:" + catBorrar, "Borrar", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        btnGuardaActProducto.Visibility = Visibility.Hidden;
                        btnNuevaCategoria.Visibility = Visibility.Visible;
                        btncancelarEditarcat.Visibility = Visibility.Hidden;
                        txtNuevaCat.Text = "";

                        bool res = prodF.borrarProductoYnombreCategoria(catBorrar);

                        if (res)
                        {
                            MessageBox.Show("Categoria  borrada", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Information);
                            llenarArbolCategoria();
                            llenarComboBCategoria();
                            tabCodigos.actualizarArbolyTablaProductos();
                        }
                    }

                }

            }
            else
            {
                MessageBox.Show("Seleccionar categoria a borrar", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }








        }
        private void SelectionCategoriaChanged(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            //string selectedItem = ((TreeViewItem)((TreeView)sender).SelectedItem).Header.ToString();
            string catSele = treeViewCategoria.SelectedValue.ToString();
            //Perform actions when SelectedItem changes
            //MessageBox.Show(catSele);
            if (catSele.Length < 67 && !catSele.Equals("All"))
            {
                //MessageBox.Show(catSele);
                //buscar productos de categoria asociada 
                llenarTablaProductoBynombreCat(catSele);

            }
            else if (catSele.Equals("All"))
            {
                llenarTablaProducto();
            }
        }
        private void txtBuscarProducto_TextChanged(object sender, TextChangedEventArgs e)
        {

            ProductoFacade cf = new ProductoFacade();
            List<Producto> listGetCliente = new List<Producto>();

            listGetCliente = cf.getProductobyNombre(txtbuscarProducto.Text);
            if (listGetCliente.Count > 0)
            {
                //listGetCliente = cf.getClientesbyNombre(txtBuscarCliente.Text);
                llenarTablaProductobynombre(txtbuscarProducto.Text);
                //MessageBox.Show("Por nombre 0");
            }
            else
            {

                llenarTablaProductobyidCodigo(txtbuscarProducto.Text);
                //MessageBox.Show("por rut  0");
            }
            //MessageBox.Show("buscar");

        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            Productos codVend = new Productos();
            codVend.setContent(translogin);
            translogin.pageTransitionControl.ShowPage(codVend);
        }

       
        private void txtprecio_TextChanged(object sender, TextChangedEventArgs e)
        {
            int p = ToEntero(txtprecio.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
            txtprecio.Text = p.ToString("#,#", CultureInfo.InvariantCulture);
            txtprecio.Focus();
            txtprecio.SelectionStart = txtprecio.Text.Length;
        }

        private void txtprecioReal_TextChanged(object sender, TextChangedEventArgs e)
        {
           int p = ToEntero(txtprecioReal.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));
           txtprecioReal.Text = p.ToString("#,#", CultureInfo.InvariantCulture); ;
            txtprecioReal.Focus();
            txtprecioReal.SelectionStart = txtprecioReal.Text.Length;

            /*try
            {
                string value = txtprecioReal.Text.Replace(",", "");
                ulong ul;
                if (ulong.TryParse(value, out ul))
                {
                    txtprecioReal.TextChanged -= txtprecioReal_TextChanged;
                    txtprecioReal.Text = string.Format("{0:#,#}", ul);
                    txtprecioReal.SelectionStart = txtprecioReal.Text.Length;
                    txtprecioReal.TextChanged += txtprecioReal_TextChanged;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }*/
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

        private void txtstock_TextChanged(object sender, TextChangedEventArgs e)
        {
            int p = ToEntero(txtstock.Text, NumberStyles.Float | NumberStyles.AllowThousands, new CultureInfo("en-GB"));

            txtstock.Text = p.ToString("#,#", CultureInfo.InvariantCulture);
            txtstock.Focus();
            txtstock.SelectionStart = txtstock.Text.Length;
        }
    }
}
