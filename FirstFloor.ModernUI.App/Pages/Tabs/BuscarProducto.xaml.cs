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
    /// Interaction logic for BuscarProducto.xaml
    /// </summary>
    public partial class BuscarProducto : Window
    {
        List<Producto> ListProductos = new List<Producto>();
        CollectionViewSource itemCollectionViewSource;
        Devolucion cventas = new Devolucion();
        public BuscarProducto()
        {
            InitializeComponent();
            llenarArbolCategoria();
            llenarTablaProducto();
        }
        public void setInstancia(Devolucion v)
        {
            cventas = v;
        }
        public Devolucion getInstancia()
        {
            return cventas;
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
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = item.stock, precioReal = item.precioReal, precio = item.precio, idCategoria = item.idCategoria, fecha = item.fecha });
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
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = item.stock, precioReal = item.precioReal, precio = item.precio, idCategoria = item.idCategoria, fecha = item.fecha });
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
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = item.stock, precioReal = item.precioReal, precio = item.precio, idCategoria = item.idCategoria, fecha = item.fecha });
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
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = item.stock, precioReal = item.precioReal, precio = item.precio, idCategoria = item.idCategoria, fecha = item.fecha });
                }

                datagridProducto.ItemsSource = ListProductos;
                
            }
            else
            {
                ListProductos.Add(new Producto { idProducto = "Categoria sin productos", nombre = "", stock = "", precioReal = "", precio = "", idCategoria = 0 });

                datagridProducto.ItemsSource = ListProductos;
                

            }

        }


        
        private void llenarArbolCategoria()
        {

            treeViewCategoria.Items.Clear();
            TreeViewItem parent = new TreeViewItem();
            parent.Header = "Categorias";
            treeViewCategoria.Items.Add(parent);


            TreeViewItem nuevoChild = new TreeViewItem();
            nuevoChild.Header = "All";
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

        private void btnElegir_Click(object sender, RoutedEventArgs e)
        {
            if (datagridProducto.SelectedItem != null)
            {
                if (datagridProducto.SelectedItem is Producto)
                {
                    var row = (Producto)datagridProducto.SelectedItem;
                    if (row != null)
                    {
                        if (!row.idProducto.Equals("Categoria sin productos"))
                        {
                            cventas.txtidProducto.Text = row.idProducto;
                            this.Close();
                        }
                    }
                }
            }
        }

        private void btncancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

    }
}
