using System;
using System.IO;
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

using FirstFloor.ModernUI.App.Control;
using FirstFloor.ModernUI.App.Modelo;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using FirstFloor.ModernUI.App.Pages.Tabs.Inv;


namespace FirstFloor.ModernUI.App.Pages.Tabs.Inv
{
    /// <summary>
    /// Interaction logic for Codigos.xaml
    /// </summary>
    public partial class Codigos : UserControl
    {
        BarcodeLib.Barcode b = new BarcodeLib.Barcode();
        string tempPath = System.IO.Path.GetTempPath();
        string url = "";
        string urlimagen = "";
        //Productos prod = new Productos();
        string urlpdf = "";
        List<Producto> ListProductos = new List<Producto>();
        public Codigos()
        {
            try
            {
            InitializeComponent();
            
                
                url = @tempPath + "pdf";
                if (!System.IO.Directory.Exists(url))
                {
                    System.IO.Directory.CreateDirectory(url);
                }
                urlimagen = @tempPath + "pdf\\imgCodigo.png";
                //urlpdf = @tempPath + "pdf\\pdfCodigoGenerado.pdf";


                txttop.IsEnabled = false;
                txtbotom.IsEnabled = false;
                txtleft.IsEnabled = false;
                txtright.IsEnabled = false;

                llenarTablaProductoCodBarra();
                llenarArbolCategoriaCodBarra();
                //prod.setInstCodigo(this);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }


        }

        public void actualizarArbolyTablaProductos()
        {
            llenarTablaProductoCodBarra();
            llenarArbolCategoriaCodBarra();

        }


        private void textbox_NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //############################################################
        //################           Codigo barra       ##################
        //############################################################

        public void llenarTablaProductoCodBarra()
        {

            ProductoFacade prodF = new ProductoFacade();
            
            ListProductos.Clear();
            datagridProducto_Imprimir.ItemsSource = null;
            var ListProd = prodF.getProductos();

            if (ListProd.Count > 0)
            {
                foreach (var item in ListProd)
                {
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = item.stock, precio = item.precio, idCategoria = item.idCategoria });
                }

                //CollectionViewSource itemCollectionViewSource;
                //itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSourceAllProductoImprimir"));
                datagridProducto_Imprimir.ItemsSource = ListProductos;

            }
            else
            {
                ListProductos.Add(new Producto { idProducto = "Sin productos", nombre = "", stock = "", precio = "", idCategoria = 0 });


                /*CollectionViewSource itemCollectionViewSource;
                itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSourceAllProductoImprimir"));
                itemCollectionViewSource.Source = ListProductos;*/
                datagridProducto_Imprimir.ItemsSource = ListProductos;



            }
        }
        private void llenarArbolCategoriaCodBarra()
        {

            treeViewCategoria_Imprimir.Items.Clear();
            TreeViewItem parent = new TreeViewItem();
            parent.Header = "Categorias";
            treeViewCategoria_Imprimir.Items.Add(parent);


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
        public void llenarTablaProductoBynombreCatCodBarra(string nombreCategoria)
        {
            ProductoFacade prodF = new ProductoFacade();
            
            var listaProd = prodF.getProductosBynombreCategoria(nombreCategoria);
            ListProductos.Clear();
            datagridProducto_Imprimir.ItemsSource = null;
            if (listaProd.Count > 0)
            {
                foreach (var item in listaProd)
                {
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = item.stock, precio = item.precio, idCategoria = item.idCategoria });
                }

                /*CollectionViewSource itemCollectionViewSource;
                itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSourceAllProductoImprimir"));
                itemCollectionViewSource.Source = itemList;*/
                datagridProducto_Imprimir.ItemsSource = ListProductos;

            }
            else
            {
                ListProductos.Add(new Producto { idProducto = "Categoria sin productos", nombre = "", stock = "", precio = "", idCategoria = 0 });


                /*CollectionViewSource itemCollectionViewSource;
                itemCollectionViewSource = (CollectionViewSource)(FindResource("ItemCollectionViewSourceAllProductoImprimir"));
                itemCollectionViewSource.Source = itemList;*/
                datagridProducto_Imprimir.ItemsSource = ListProductos;

            }

        }

        private void SelectionCategoriaCodBarraChanged(object sender, RoutedPropertyChangedEventArgs<Object> e)
        {
            //string selectedItem = ((TreeViewItem)((TreeView)sender).SelectedItem).Header.ToString();
            string catSele = treeViewCategoria_Imprimir.SelectedValue.ToString();
            //Perform actions when SelectedItem changes
            //MessageBox.Show(catSele);
            if (catSele.Length < 67 && !catSele.Equals("All"))
            {
                //MessageBox.Show(catSele);
                //buscar productos de categoria asociada 
                llenarTablaProductoBynombreCatCodBarra(catSele);

            }
            else if (catSele.Equals("All"))
            {
                llenarTablaProductoCodBarra();
            }
        }
        bool IsNumber(string s)
        {
            return s.Length > 0 && s.All(c => Char.IsDigit(c));
        }
        private void datagridProducto_Imprimir_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datagridProducto_Imprimir.SelectedItem != null)
            {
                if (datagridProducto_Imprimir.SelectedItem is Producto)
                {
                    var row = (Producto)datagridProducto_Imprimir.SelectedItem;

                    if (row != null)
                    {

                        txtCodigoToImprimir.Text = row.idProducto;

                    }
                }
            }

        }

        private void btnGenerarCodBarra_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show(urlimagen);
            if (chboxNoAgrupada.IsChecked == true)
            {
                if (!string.IsNullOrEmpty(txtCodigoToImprimir.Text))
                {
                    int numeEti = listBoxEtiquetas.SelectedIndex;

                    if (numeEti == 0)
                    {
                        try
                        {
                            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                            folderDialog.SelectedPath = "C:\\";
                            folderDialog.Description = "Seleccionar carpeta donde se guardará el pdf con codigo generado";
                            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();

                            if (string.IsNullOrEmpty(urlpdf))
                            {
                                if (result.ToString() == "OK")
                                {
                                    urlpdf = folderDialog.SelectedPath + "\\pdf";
                                    if (!System.IO.Directory.Exists(urlpdf))
                                    {
                                        System.IO.Directory.CreateDirectory(urlpdf);
                                    }
                                    txtcolumnas.IsEnabled = false;
                                    txtcolumnas.Text = "1";
                                    GenerarCodBarra(0);

                                }
                            }
                            else 
                            {
                                txtcolumnas.IsEnabled = false;
                                txtcolumnas.Text = "1";
                                GenerarCodBarra(0);

                            }
                        

                           

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error List etiqueta:" + ex.Message + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    else if (numeEti == 1)
                    {
                        try
                        {
                            txtcolumnas.IsEnabled = false;
                            txtcolumnas.Text = "3";


                            GenerarCodBarra(1);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error List etiqueta:" + ex.Message + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                    else if (numeEti == 2)
                    {//Etiqueta personalizada
                        try
                        {
                            txtcolumnas.IsEnabled = true;

                            GenerarCodBarra(2);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error List etiqueta:" + ex.Message + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Ingresar codigo de producto para generar codigo de barra", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }else if(chboxAgrupada.IsChecked==true)
            {
                //verificar si hay productos en bd
                ProductoFacade prodFac = new ProductoFacade();
                List<Producto> listaProd = prodFac.getProductos();

                if (listaProd.Count>0)
                {
                    int numeEti = listBoxEtiquetas.SelectedIndex;

                    if (numeEti == 0)
                    {

                        System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                        folderDialog.SelectedPath = "C:\\";
                        folderDialog.Description = "Seleccionar carpeta donde se guardarán los pdfs con codigos asociados a cada categoria";
                        System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();
                        string urlG = "";
                        if (result.ToString() == "OK")
                        {

                            try
                            {
                                urlG = folderDialog.SelectedPath + "\\Codigos\\";
                                if (!System.IO.Directory.Exists(urlG))
                                {
                                    System.IO.Directory.CreateDirectory(urlG);
                                }

                                txtcolumnas.IsEnabled = false;
                                txtcolumnas.Text = "1";
                                //crear x pdf con nombre de categorias en la url seleccionadad

                                categoriaFacade catFac = new categoriaFacade();
                                //obtener listCategoria que tengan productos
                                List<Categoria> listCat = catFac.getCategoriaConProductosParaImprimirCodigos();
                                //MessageBox.Show(listCat.Count.ToString());
                                //RECORRER LISTA DE CATEGORIA Y OBTENER NOMBRE Y PRODUCTO ASOCIADO

                                int W = Convert.ToInt32(this.txtAncho.Text.Trim());
                                int H = Convert.ToInt32(this.txtAlto.Text.Trim());
                                b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;
                                //b.LabelFont = new Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Regular);
                                try
                                {
                                    foreach (var cat in listCat)
                                    {
                                        if (type != BarcodeLib.TYPE.UNSPECIFIED)
                                        {
                                            b.IncludeLabel = true;
                                            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                                            //b.AlternateLabel = "Texto";
                                            List<Producto> listProductos = prodFac.getProductosBynombreCategoria(cat.nombreCategoria);


                                            if (crearPdfCategorizado(urlG + cat.nombreCategoria, listProductos, 0))
                                            {
                                                //Cargar Pdf en vista
                                                //pdfViewer.LoadFile(urlpdf);
                                            }


                                        }//if
                                    }//foreach


                                }//try
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error al crear CodigoBarra:" + ex.Message + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }//catch

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error List etiqueta:" + ex.Message + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            System.Diagnostics.Process.Start(@urlG);
                        }


                    }
                    else if (numeEti == 1)
                    {
                        
                        //string folderpath = "";
                        //FolderBrowserDialog fbd=new FolderBrowserDialog();
                        //DialogResult dr=fbd.ShowDialog();

                        System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                        folderDialog.SelectedPath = "C:\\";
                        folderDialog.Description = "Seleccionar carpeta donde se guardarán los pdfs con codigos asociados a cada categoria";
                        System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();
                        string urlG = "";
                        if (result.ToString() == "OK")
                        {
                            
                            try
                            {
                                urlG = folderDialog.SelectedPath + "\\Codigos\\";
                                if (!System.IO.Directory.Exists(urlG))
                                {
                                    System.IO.Directory.CreateDirectory(urlG);
                                }

                                txtcolumnas.IsEnabled = false;
                                txtcolumnas.Text = "3";
                                //crear x pdf con nombre de categorias en la url seleccionadad

                                categoriaFacade catFac = new categoriaFacade();
                                //obtener listCategoria que tengan productos
                                List<Categoria> listCat = catFac.getCategoriaConProductosParaImprimirCodigos();
                                //MessageBox.Show(listCat.Count.ToString());
                                //RECORRER LISTA DE CATEGORIA Y OBTENER NOMBRE Y PRODUCTO ASOCIADO

                                int W = Convert.ToInt32(this.txtAncho.Text.Trim());
                                int H = Convert.ToInt32(this.txtAlto.Text.Trim());
                                b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;
                                //b.LabelFont = new Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Regular);
                                try
                                {
                                    foreach (var cat in listCat)
                                    {
                                        if (type != BarcodeLib.TYPE.UNSPECIFIED)
                                        {
                                            b.IncludeLabel = true;
                                            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                                            //b.AlternateLabel = "Texto";
                                            List<Producto> listProductos = prodFac.getProductosBynombreCategoria(cat.nombreCategoria);

                                            
                                            if (crearPdfCategorizado(urlG + cat.nombreCategoria,listProductos, 1))
                                            {
                                                //Cargar Pdf en vista
                                                //pdfViewer.LoadFile(urlpdf);
                                            }


                                        }//if
                                    }//foreach
                                

                                }//try
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error al crear CodigoBarra:" + ex.Message + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }//catch
                                
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error List etiqueta:" + ex.Message + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            System.Diagnostics.Process.Start(@urlG);
                        }

                    }
                    else if (numeEti == 2)
                    {//Etiqueta personalizada

                        System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                        folderDialog.SelectedPath = "C:\\";
                        folderDialog.Description = "Seleccionar carpeta donde se guardarán los pdfs con codigos asociados a cada categoria";
                        System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();
                        string urlG = "";
                        if (result.ToString() == "OK")
                        {

                            try
                            {
                                urlG = folderDialog.SelectedPath + "\\Codigos\\";
                                if (!System.IO.Directory.Exists(urlG))
                                {
                                    System.IO.Directory.CreateDirectory(urlG);
                                }

                                txtcolumnas.IsEnabled = true;
                                
                                //crear x pdf con nombre de categorias en la url seleccionadad

                                categoriaFacade catFac = new categoriaFacade();
                                //obtener listCategoria que tengan productos
                                List<Categoria> listCat = catFac.getCategoriaConProductosParaImprimirCodigos();
                                //MessageBox.Show(listCat.Count.ToString());
                                //RECORRER LISTA DE CATEGORIA Y OBTENER NOMBRE Y PRODUCTO ASOCIADO

                                int W = Convert.ToInt32(this.txtAncho.Text.Trim());
                                int H = Convert.ToInt32(this.txtAlto.Text.Trim());
                                b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                                BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;
                                //b.LabelFont = new Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Regular);
                                try
                                {
                                    foreach (var cat in listCat)
                                    {
                                        if (type != BarcodeLib.TYPE.UNSPECIFIED)
                                        {
                                            b.IncludeLabel = true;
                                            b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                                            //b.AlternateLabel = "Texto";
                                            List<Producto> listProductos = prodFac.getProductosBynombreCategoria(cat.nombreCategoria);


                                            if (crearPdfCategorizado(urlG + cat.nombreCategoria, listProductos, 2))
                                            {
                                                //Cargar Pdf en vista
                                                //pdfViewer.LoadFile(urlpdf);
                                            }


                                        }//if
                                    }//foreach


                                }//try
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error al crear CodigoBarra:" + ex.Message + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                                }//catch

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error List etiqueta:" + ex.Message + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            System.Diagnostics.Process.Start(@urlG);
                        }


                    }
                }
                else
                {
                    MessageBox.Show("No hay productos registrados", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
        }
        private void GenerarCodBarra(int numEti)
        {
            int W = Convert.ToInt32(this.txtAncho.Text.Trim());
            int H = Convert.ToInt32(this.txtAlto.Text.Trim());
            b.Alignment = BarcodeLib.AlignmentPositions.CENTER;




            BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;
            //b.LabelFont = new Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Regular);
            try
            {
                if (type != BarcodeLib.TYPE.UNSPECIFIED)
                {
                    b.IncludeLabel = true;
                    b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                    //b.AlternateLabel = "Texto";

                    b.Encode(type, txtCodigoToImprimir.Text.Trim(), W, H);
                    BarcodeLib.SaveTypes savetype = BarcodeLib.SaveTypes.PNG;
                    //b.IncludeLabel = true;
                    b.SaveImage(urlimagen, savetype);
                    string resultado = crearPdf(numEti);
                    if (!string.IsNullOrEmpty(resultado))
                    {
                        //Cargar Pdf en vista
                        System.Diagnostics.Process.Start(resultado);
                        System.Diagnostics.Process.Start(urlpdf);
                        //pdfViewer.LoadFile(urlpdf);
                    }

                }//if


            }//try
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear CodigoBarra:" + ex.Message + "", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
            }//catch
        }


        private bool crearPdfCategorizado(string urlpdfcategorizado,List<Producto> listProducto,int numEtiq)
        {
            bool correct = false;
            try
            {
                //Etiqueta para rollo con 1 columnas de mica
                if (numEtiq == 0)
                {
                    // Creamos el documento con el tamaño de página tradicional
                    Document doc = new Document(PageSize.LETTER);
                    // Indicamos donde vamos a guardar el documento
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(urlpdfcategorizado+".pdf", FileMode.Create));
                    doc.AddCreator("Magnolia");
                    doc.Open();
                    BaseFont serif = BaseFont.CreateFont(@"C:\Windows\Fonts\micross.ttf", "Identity-H", BaseFont.EMBEDDED);
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(serif, Convert.ToInt32(txtTamanoFuente.Text), iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);

                    int col = 1;
                    int row = Convert.ToInt32(txtfila.Text);
                    PdfPTable tblPrueba = new PdfPTable(col);
                    doc.SetMargins(0f, 0f, 0f, 0f);
                    tblPrueba.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblPrueba.WidthPercentage = 50;

                    foreach (var prod in listProducto)
                    {
                        int W = Convert.ToInt32(this.txtAncho.Text.Trim());
                        int H = Convert.ToInt32(this.txtAlto.Text.Trim());
                        b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                        BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;
                        b.IncludeLabel = true;
                        b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                        b.Encode(type, prod.idProducto, W, H);
                        BarcodeLib.SaveTypes savetype = BarcodeLib.SaveTypes.PNG;
                        b.SaveImage(urlimagen, savetype);

                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlimagen);
                        imagen.BorderWidth = 0;
                        imagen.Alignment = Element.ALIGN_CENTER;
                        imagen.ScaleToFit(150f, 150f);

                        string txtsobreBarra = "";
                        if (chkGenerateLabel.IsChecked.Value)
                        {
                            MessageBox.Show(prod.nombre.Length.ToString() + ";" + prod.nombre);
                            if (prod.nombre.Length > 25)
                            {
                                txtsobreBarra = prod.nombre.Substring(0, 25) + " $" + prod.precio;
                            }
                            else
                            {
                                txtsobreBarra = prod.nombre + " $" + prod.precio;
                            }
                            
                        }

                        for (int c = 0; c < col; c++)
                        {
                            for (int f = 0; f < row; f++)
                            {

                                PdfPCell clNombre = new PdfPCell { };
                                clNombre.BorderWidth = 0;
                                clNombre.Padding = 13;
                                clNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                                iTextSharp.text.Paragraph textsobre = new iTextSharp.text.Paragraph(new Phrase(txtsobreBarra, _standardFont));
                                textsobre.Alignment = 1;
                                clNombre.AddElement(textsobre);
                                clNombre.AddElement(imagen);
                                tblPrueba.AddCell(clNombre);
                            }
                        }
                    }
                    doc.Add(tblPrueba);
                    doc.Close();
                    writer.Close();
                    //Etiqueta para rollo con 3 columnas de  mica
                    correct = true;
                }
                else if (numEtiq == 1)
                {
                    Document doc = new Document(PageSize.LETTER, 10f, 10f, 10f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(urlpdfcategorizado + ".pdf", FileMode.Create));
                    doc.AddTitle("pdf");
                    doc.AddCreator("Magnolia");
                    doc.Open();
                    doc.Add(Chunk.NEWLINE);
                    BaseFont serif = BaseFont.CreateFont(@"C:\Windows\Fonts\micross.ttf", "Identity-H", BaseFont.EMBEDDED);
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(serif, Convert.ToInt32(txtTamanoFuente.Text), iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                    int col = 3;
                    int row = Convert.ToInt32(txtfila.Text);
                    PdfPTable tblPrueba = new PdfPTable(col);
                    tblPrueba.WidthPercentage = 100;


                    foreach (var prod in listProducto)
                    {
                        int W = Convert.ToInt32(this.txtAncho.Text.Trim());
                        int H = Convert.ToInt32(this.txtAlto.Text.Trim());
                        b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                        BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;
                        b.IncludeLabel = true;
                        b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                        b.Encode(type, prod.idProducto, W, H);
                        BarcodeLib.SaveTypes savetype = BarcodeLib.SaveTypes.PNG;
                        b.SaveImage(urlimagen, savetype);

                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlimagen);
                        imagen.BorderWidth = 0;
                        imagen.Alignment = Element.ALIGN_CENTER;
                        imagen.ScaleToFit(150f, 150f);

                        string txtsobreBarra = "";
                        if (chkGenerateLabel.IsChecked.Value)
                        {
                            txtsobreBarra = prod.nombre + " $"+ prod.precio;
                        }

                        for (int c = 0; c < col; c++)
                        {
                            for (int f = 0; f < row; f++)
                            {

                                PdfPCell clNombre = new PdfPCell { };
                                clNombre.BorderWidth = 0;
                                clNombre.Padding = 13;
                                clNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                                iTextSharp.text.Paragraph textsobre = new iTextSharp.text.Paragraph(new Phrase(txtsobreBarra, _standardFont));
                                textsobre.Alignment = 1;
                                clNombre.AddElement(textsobre);
                                clNombre.AddElement(imagen);
                                tblPrueba.AddCell(clNombre);
                            }
                        }
                    }
                    doc.Add(tblPrueba);
                    doc.Close();
                    writer.Close();
                    correct = true;
                }
                else if (numEtiq == 2)
                {

                    if (!string.IsNullOrEmpty(txtleft.Text))
                    {
                        if (!string.IsNullOrEmpty(txtright.Text))
                        {
                            if (!string.IsNullOrEmpty(txttop.Text))
                            {
                                if (!string.IsNullOrEmpty(txtbotom.Text))
                                {
                                    float left = float.Parse(txtleft.Text);
                                    float right = float.Parse(txtright.Text);
                                    float top = float.Parse(txttop.Text);
                                    float bottom = float.Parse(txtbotom.Text);


                                    Document doc = new Document(PageSize.LETTER, left, right, top, bottom);
                                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(urlpdfcategorizado + ".pdf", FileMode.Create));
                                    doc.AddTitle("pdf");
                                    doc.AddCreator("Magnolia");
                                    doc.Open();
                                    doc.Add(Chunk.NEWLINE);
                                    //System.Drawing.Font f=  new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Regular);
                                    //FontFactory.GetFont("Microsoft Sans Serif", 10)
                                    BaseFont serif = BaseFont.CreateFont(@"C:\Windows\Fonts\micross.ttf", "Identity-H", BaseFont.EMBEDDED);
                                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(serif, Convert.ToInt32(txtTamanoFuente.Text), iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);

                                    int col = Convert.ToInt32(txtcolumnas.Text); ;
                                    int row = Convert.ToInt32(txtfila.Text);
                                    PdfPTable tblPrueba = new PdfPTable(col);
                                    tblPrueba.WidthPercentage = 100;

                                    foreach (var prod in listProducto)
                                    {
                                        int W = Convert.ToInt32(this.txtAncho.Text.Trim());
                                        int H = Convert.ToInt32(this.txtAlto.Text.Trim());
                                        b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                                        BarcodeLib.TYPE type = BarcodeLib.TYPE.CODE128;
                                        b.IncludeLabel = true;
                                        b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;
                                        b.Encode(type, prod.idProducto, W, H);
                                        BarcodeLib.SaveTypes savetype = BarcodeLib.SaveTypes.PNG;
                                        b.SaveImage(urlimagen, savetype);

                                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlimagen);
                                        imagen.BorderWidth = 0;
                                        imagen.Alignment = Element.ALIGN_CENTER;
                                        imagen.ScaleToFit(150f, 150f);

                                        string txtsobreBarra = "";
                                        if (chkGenerateLabel.IsChecked.Value)
                                        {
                                            txtsobreBarra = prod.nombre + " $" + prod.precio;
                                        }

                                        for (int c = 0; c < col; c++)
                                        {
                                            for (int f = 0; f < row; f++)
                                            {

                                                PdfPCell clNombre = new PdfPCell { };
                                                clNombre.BorderWidth = 0;
                                                clNombre.Padding = 13;
                                                clNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                                                iTextSharp.text.Paragraph textsobre = new iTextSharp.text.Paragraph(new Phrase(txtsobreBarra, _standardFont));
                                                textsobre.Alignment = 1;
                                                clNombre.AddElement(textsobre);
                                                clNombre.AddElement(imagen);
                                                tblPrueba.AddCell(clNombre);
                                            }
                                        }
                                    }
                                    doc.Add(tblPrueba);
                                    doc.Close();
                                    writer.Close();
                                    correct = true;

                                }
                                else
                                {
                                    MessageBox.Show("Ingresar margen: Abajo", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                                }
                            }
                            else
                            {
                                MessageBox.Show("Ingresar margen: Arriba", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                            }
                        }
                        else
                        {
                            MessageBox.Show("Ingresar margen:Derecho", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                    }
                    else
                    {
                        MessageBox.Show("Ingresar margen:Izquierdo", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Cerrar PDf(s) abiertos", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return correct;
        }
        private string crearPdf(int numEtiq)
        {
            string correct = "";
            try
            {
                //Etiqueta para rollo con 1 columnas de mica
                if (numEtiq == 0)
                {
                    // Creamos el documento con el tamaño de página tradicional
                    Document doc = new Document(PageSize.LETTER);
                    // Indicamos donde vamos a guardar el documento
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(urlpdf+"//pdfGenerado1Col.pdf", FileMode.Create));
                    
                    doc.AddCreator("Magnolia");
                    doc.Open();
                    BaseFont serif = BaseFont.CreateFont(@"C:\Windows\Fonts\micross.ttf", "Identity-H", BaseFont.EMBEDDED);
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(serif, Convert.ToInt32(txtTamanoFuente.Text), iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);

                    int col = 1;
                    int row = Convert.ToInt32(txtfila.Text);
                    PdfPTable tblPrueba = new PdfPTable(col);
                    doc.SetMargins(0f, 0f, 0f, 0f);
                    tblPrueba.HorizontalAlignment = Element.ALIGN_CENTER;
                    tblPrueba.WidthPercentage = 50;
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlimagen);
                    imagen.Alignment = Element.ALIGN_CENTER;
                    imagen.ScaleToFit(140f, 120f);

                    string txtsobreBarra = "";
                    if (chkGenerateLabel.IsChecked.Value)
                    {
                        txtsobreBarra = txtSobreBarra.Text;
                    }

                    for (int c = 0; c < col; c++)
                    {
                        for (int f = 0; f < row; f++)
                        {
                            PdfPCell clLabel = new PdfPCell(new Phrase(txtsobreBarra, _standardFont));
                            clLabel.HorizontalAlignment = Element.ALIGN_CENTER;
                            clLabel.BorderWidth = 0;

                            PdfPCell clNombre = new PdfPCell(imagen);
                            clNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                            clNombre.BorderWidth = 0;
                            clNombre.PaddingBottom = 15;
                            tblPrueba.AddCell(clLabel);
                            tblPrueba.AddCell(clNombre);
                        }
                    }
                    doc.Add(tblPrueba);
                    doc.Close();
                    writer.Close();
                    //Etiqueta para rollo con 3 columnas de  mica
                    correct = urlpdf + "//pdfGenerado1Col.pdf";
                }
                else if (numEtiq == 1)
                {
                    Document doc = new Document(PageSize.LETTER, 10f, 10f, 10f, 0f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(urlpdf + "//pdfGenerado3Col.pdf", FileMode.Create));
                    
                    doc.AddTitle("pdf");
                    doc.AddCreator("Magnolia");
                    doc.Open();
                    doc.Add(Chunk.NEWLINE);
                    BaseFont serif = BaseFont.CreateFont(@"C:\Windows\Fonts\micross.ttf", "Identity-H", BaseFont.EMBEDDED);
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(serif, Convert.ToInt32(txtTamanoFuente.Text), iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                    int col = 3;
                    int row = Convert.ToInt32(txtfila.Text);
                    PdfPTable tblPrueba = new PdfPTable(col);
                    tblPrueba.WidthPercentage = 100;
                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlimagen);
                    imagen.BorderWidth = 0;
                    imagen.Alignment = Element.ALIGN_CENTER;
                    imagen.ScaleToFit(150f, 150f);

                    string txtsobreBarra = "";
                    if (chkGenerateLabel.IsChecked.Value)
                    {
                        txtsobreBarra = txtSobreBarra.Text;
                    }

                    for (int c = 0; c < col; c++)
                    {
                        for (int f = 0; f < row; f++)
                        {

                            PdfPCell clNombre = new PdfPCell { };
                            clNombre.BorderWidth = 0;
                            clNombre.Padding = 13;
                            clNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                            iTextSharp.text.Paragraph textsobre = new iTextSharp.text.Paragraph(new Phrase(txtsobreBarra, _standardFont));
                            textsobre.Alignment = 1;
                            clNombre.AddElement(textsobre);
                            clNombre.AddElement(imagen);
                            tblPrueba.AddCell(clNombre);
                        }
                    }
                    doc.Add(tblPrueba);
                    doc.Close();
                    writer.Close();
                    correct = urlpdf + "//pdfGenerado3Col.pdf";
                }
                else if (numEtiq == 2)
                {

                    if (!string.IsNullOrEmpty(txtleft.Text))
                    {
                        if (!string.IsNullOrEmpty(txtright.Text))
                        {
                            if (!string.IsNullOrEmpty(txttop.Text))
                            {
                                if (!string.IsNullOrEmpty(txtbotom.Text))
                                {
                                    float left = float.Parse(txtleft.Text);
                                    float right = float.Parse(txtright.Text);
                                    float top = float.Parse(txttop.Text);
                                    float bottom = float.Parse(txtbotom.Text);


                                    Document doc = new Document(PageSize.LETTER, left, right, top, bottom);
                                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(urlpdf + "//pdfGeneradoPersonalizado.pdf", FileMode.Create));
                                    correct = urlpdf + "//pdfGeneradoPersonalizado.pdf";
                                    doc.AddTitle("pdf");
                                    doc.AddCreator("Magnolia");
                                    doc.Open();
                                    doc.Add(Chunk.NEWLINE);
                                    //System.Drawing.Font f=  new System.Drawing.Font("Microsoft Sans Serif", 10, System.Drawing.FontStyle.Regular);
                                    //FontFactory.GetFont("Microsoft Sans Serif", 10)
                                    BaseFont serif = BaseFont.CreateFont(@"C:\Windows\Fonts\micross.ttf", "Identity-H", BaseFont.EMBEDDED);
                                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(serif, Convert.ToInt32(txtTamanoFuente.Text), iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);

                                    int col = Convert.ToInt32(txtcolumnas.Text); ;
                                    int row = Convert.ToInt32(txtfila.Text);
                                    PdfPTable tblPrueba = new PdfPTable(col);
                                    tblPrueba.WidthPercentage = 100;

                                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlimagen);
                                    imagen.BorderWidth = 0;
                                    imagen.Alignment = Element.ALIGN_CENTER;
                                    imagen.ScaleToFit(150f, 150f);
                                    string txtsobreBarra = "";
                                    if (chkGenerateLabel.IsChecked.Value)
                                    {
                                        txtsobreBarra = txtSobreBarra.Text;
                                    }

                                    for (int c = 0; c < col; c++)
                                    {
                                        for (int f = 0; f < row; f++)
                                        {

                                            PdfPCell clNombre = new PdfPCell { };
                                            clNombre.BorderWidth = 0;
                                            clNombre.Padding = 13;
                                            clNombre.HorizontalAlignment = Element.ALIGN_CENTER;
                                            iTextSharp.text.Paragraph textsobre = new iTextSharp.text.Paragraph(new Phrase(txtsobreBarra, _standardFont));
                                            textsobre.Alignment = 1;
                                            clNombre.AddElement(textsobre);
                                            clNombre.AddElement(imagen);
                                            tblPrueba.AddCell(clNombre);
                                        }
                                    }
                                    doc.Add(tblPrueba);
                                    doc.Close();
                                    writer.Close();
                                    correct = urlpdf + "//pdfGeneradoPersonalizado.pdf";

                                }
                                else
                                {
                                    MessageBox.Show("Ingresar margen: Abajo", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                                }
                            }
                            else
                            {
                                MessageBox.Show("Ingresar margen: Arriba", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                            }
                        }
                        else
                        {
                            MessageBox.Show("Ingresar margen:Derecho", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

                        }
                    }
                    else
                    {
                        MessageBox.Show("Ingresar margen:Izquierdo", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Cerrar PDf(s) abiertos", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return correct;
        }
        private void ListBoxEtiquetas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int numeEti = listBoxEtiquetas.SelectedIndex;

            if (numeEti == 0)
            {
                try
                {
                    txtcolumnas.Text = "1";
                    txtcolumnas.IsEnabled = false;
                    txttop.IsEnabled = false;
                    txtbotom.IsEnabled = false;
                    txtleft.IsEnabled = false;
                    txtright.IsEnabled = false;
                    txttop.Text = "0";
                    txtbotom.Text = "0";
                    txtleft.Text = "0";
                    txtright.Text = "0";
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error List etiqueta:" + ex.Message);
                }

            }
            else if (numeEti == 1)
            {
                try
                {
                    txtcolumnas.Text = "3";
                    txtcolumnas.IsEnabled = false;
                    txttop.IsEnabled = false;
                    txtbotom.IsEnabled = false;
                    txtleft.IsEnabled = false;
                    txtright.IsEnabled = false;
                    txttop.Text = "10";
                    txtbotom.Text = "0";
                    txtleft.Text = "10";
                    txtright.Text = "10";
                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error List etiqueta:" + ex.Message);
                }

            }
            else if (numeEti == 2)
            {
                try
                {
                    txtcolumnas.IsEnabled = true;
                    txttop.IsEnabled = true;
                    txtbotom.IsEnabled = true;
                    txtleft.IsEnabled = true;
                    txtright.IsEnabled = true;
                    txttop.Text = "0";
                    txtbotom.Text = "0";
                    txtleft.Text = "0";
                    txtright.Text = "0";

                }
                catch (Exception ex)
                {
                    //MessageBox.Show("Error List etiqueta:" + ex.Message);
                }

            }

        }

        private void btnrecargar_Click(object sender, RoutedEventArgs e)
        {
            llenarTablaProductoCodBarra();
            llenarArbolCategoriaCodBarra();
        }

        private void chboxNoAgrupada_Checked(object sender, RoutedEventArgs e)
        {
            txtCodigoToImprimir.IsEnabled = true;
            txtSobreBarra.Text = "";
            txtSobreBarra.IsEnabled = true;
        }

        private void chboxAgrupada_Checked(object sender, RoutedEventArgs e)
        {
            txtCodigoToImprimir.Text = "";
            txtCodigoToImprimir.IsEnabled = false;
            txtSobreBarra.Text = "nomProd+Precio";
            txtSobreBarra.IsEnabled = false;
        }


        public void llenarTablaProductobynombre(string nombre)
        {

            ProductoFacade prodF = new ProductoFacade();
            var itemList = new List<Producto>();
            var listaProd = prodF.getProductobyNombre(nombre);
            ListProductos.Clear();
            datagridProducto_Imprimir.ItemsSource = null;
            if (listaProd.Count > 0)
            {
                foreach (var item in listaProd)
                {
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = item.stock, precioReal = item.precioReal, precio = item.precio, idCategoria = item.idCategoria, fecha = item.fecha });
                }

                datagridProducto_Imprimir.ItemsSource = ListProductos;
            }
            else
            {
                // DateTime fvacio = Convert.ToDateTime("15/08/2008");
                ListProductos.Add(new Producto { idProducto = "Sin productos", nombre = "", stock = "", precioReal = "", precio = "", idCategoria = 0 });

                datagridProducto_Imprimir.ItemsSource = ListProductos;

            }
        }
        public void llenarTablaProductobyidCodigo(string idCodigo)
        {

            ProductoFacade prodF = new ProductoFacade();
            var itemList = new List<Producto>();
            var listaProd = prodF.getProductobyCodigo(idCodigo);
            ListProductos.Clear();
            datagridProducto_Imprimir.ItemsSource = null;
            if (listaProd.Count > 0)
            {
                foreach (var item in listaProd)
                {
                    ListProductos.Add(new Producto { idProducto = item.idProducto, nombre = item.nombre, stock = item.stock, precioReal = item.precioReal, precio = item.precio, idCategoria = item.idCategoria, fecha = item.fecha });
                }

                datagridProducto_Imprimir.ItemsSource = ListProductos;
            }
            else
            {
                // DateTime fvacio = Convert.ToDateTime("15/08/2008");
                ListProductos.Add(new Producto { idProducto = "Sin productos", nombre = "", stock = "", precioReal = "", precio = "", idCategoria = 0 });

                datagridProducto_Imprimir.ItemsSource = ListProductos;

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
    }
}
