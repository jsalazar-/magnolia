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
using System.Windows.Controls.Primitives;
using System.Drawing;
using Microsoft.Win32;
namespace FirstFloor.ModernUI.App.Pages.Tabs
{
    /// <summary>
    /// Interaction logic for Reportes.xaml
    /// </summary>
    public partial class Reportes : UserControl
    {
        string tempPath = System.IO.Path.GetTempPath();
        string year = "";
        string month = "";
        string urlpdf = "";
        string urlimagen = "";
        string url = "";
        System.Drawing.Image urlLogo = null;
        TransLoginToReporte translogin = new TransLoginToReporte();
        string nombreAdmin = "";
        DateTime MesRubroProd = new DateTime();
        public Reportes()
        {
            InitializeComponent();
            url = @tempPath + "pdf";
            urlimagen = @"Imagenes\Logo.png";
            if (!System.IO.Directory.Exists(url))
            {
                System.IO.Directory.CreateDirectory(url);
            }
            urlpdf = @tempPath + "pdf\\";
            urlLogo = System.Drawing.Image.FromHbitmap(FirstFloor.ModernUI.App.Properties.Resources.logo.GetHbitmap());
        }

        //############################################################
        //################           REPORTES      ##################
        //############################################################
        private void calendarReportes_SelectedDatesChanged(object sender,
        SelectionChangedEventArgs e)
        {

            var calendar = sender as Calendar;
            if (calendarReportes.SelectedDate.HasValue)
            {
                DateTime date = calendar.SelectedDate.Value;
                lfechareporte.Content = date.ToString("d");
                //Buscar ventas por fecha
                ventasFacade vfac = new ventasFacade();
                List<MVentas> listVentasDia = vfac.getVentasByFechaDia(date);
                //MessageBox.Show(date.Month.ToString() + "/" + date.Year.ToString());
                List<MVentas> listVentasMes = vfac.getVentasByFechaMes(date);
                List<MVentas> listVentasAño = vfac.getVentasByFechaAño(date);
                //MessageBox.Show(listVentasDia.Count.ToString());
                int totalVentasDia = 0;
                double totalgananciaDia = 0;
                foreach (var item in listVentasDia)
                {
                    totalVentasDia = totalVentasDia + 1;
                    totalgananciaDia = totalgananciaDia + item.total;

                }

                lventasDia.Content = totalVentasDia.ToString();
                lgananciaDia.Content = "$" + totalgananciaDia.ToString();

                int totalVentasMes = 0;
                double totalgananciaMes = 0;
                foreach (var item in listVentasMes)
                {
                    totalVentasMes = totalVentasMes + 1;
                    totalgananciaMes = totalgananciaMes + item.total;

                }

                lventasMes.Content = totalVentasMes.ToString();
                lgananciaMes.Content = "$" + totalgananciaMes.ToString();

                int totalVentasAño = 0;
                double totalgananciaAño = 0;
                foreach (var item in listVentasAño)
                {
                    totalVentasAño = totalVentasAño + 1;
                    totalgananciaAño = totalgananciaAño + item.total;

                }

                lventasAño.Content = totalVentasAño.ToString();
                lgananciaAño.Content = "$" + totalgananciaAño.ToString();

            }
        }

        private void crearPdfReporteMes()
        {

        }

        private void btnReporteDia_Click(object sender, RoutedEventArgs e)
        {
            if (!lfechareporte.Content.Equals(""))
            {
                if (!lventasDia.Content.Equals("0"))
                {

                    DateTime date = Convert.ToDateTime(lfechareporte.Content);
                    ventasFacade vfac = new ventasFacade();
                    List<MVentas> listVentasDia = vfac.getVentasByFechaDia(date);

                    if (listVentasDia.Count > 0)
                    {
                        SaveFileDialog exportSaveFileDialog = new SaveFileDialog();

                        exportSaveFileDialog.Title = "Guardar reporte de ventas diarias";
                        exportSaveFileDialog.Filter = "PDF(*.pdf)|*.pdf";
                        exportSaveFileDialog.FileName = "ReporteVentasDiarias";
                        exportSaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                        if (exportSaveFileDialog.ShowDialog() == true)
                        {
                            //File.WriteAllText(exportSaveFileDialog.FileName, "ReporteDia");



                            // Creamos el documento con el tamaño de página tradicional
                            Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);
                            PdfWriter writer = null;
                            // Indicamos donde vamos a guardar el documento
                            try
                            {
                                //writer = PdfWriter.GetInstance(doc, new FileStream(urlpdf + "ReporteDia.pdf", FileMode.Create));
                                writer = PdfWriter.GetInstance(doc, new FileStream(exportSaveFileDialog.FileName, FileMode.Create));
                                doc.AddCreator("Magnolia");
                                doc.Open();
                                //Image myImage = FirstFloor.ModernUI.App.Properties.Resources.logo;

                                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlLogo, System.Drawing.Imaging.ImageFormat.Png);
                                imagen.Alignment = Element.ALIGN_CENTER;
                                imagen.ScaleToFit(120f, 120f);
                                doc.Add(imagen);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Font _fontTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 25, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                iTextSharp.text.Paragraph titulo = new iTextSharp.text.Paragraph("Reporte de ventas diaria");
                                titulo.Alignment = Element.ALIGN_CENTER;
                                titulo.Font = _fontTitulo;
                                doc.Add(titulo);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Paragraph dgeneral = new iTextSharp.text.Paragraph("Detalles general");
                                dgeneral.Alignment = Element.ALIGN_LEFT;

                                //doc.AddTitle("Reporte de MVentas");
                                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                //////////////////////////////////////
                                ///////Tabla especificos    //////////
                                /////////////////////////////////////
                                iTextSharp.text.Paragraph despecifico = new iTextSharp.text.Paragraph("Detalles Especificos");
                                despecifico.Alignment = Element.ALIGN_LEFT;

                                // doc.Add(Chunk.NEWLINE);

                                categoriaFacade catFacEsp = new categoriaFacade();
                                ProductoFacade prodFacEsp = new ProductoFacade();
                                ventasFacade ventasFacEsp = new ventasFacade();
                                PdfPTable tblEspecifico = new PdfPTable(5);

                                string tempIdpro = "";

                                int dineroTotalbyCat = 0;
                                List<MVentas> listProductosinCatniInfoProd = new List<MVentas>();
                                List<MVentas> listProductoConCategoria = new List<MVentas>();

                                foreach (var item in listVentasDia)
                                {
                                    if (!tempIdpro.Equals(item.idProducto))
                                    {
                                        string IDCategoria = prodFacEsp.getIdCatbyidProd(item.idProducto);

                                        if (string.IsNullOrEmpty(IDCategoria))
                                        {
                                            //MessageBox.Show(item.idProducto + ":Sin categoria");
                                            //Agregar en celda distinta para obtener el total, no tienen categoria porque posiblemente se borra una categoria durante el mes o tiempo de uso.
                                            //Sin nombre de producto  ni categoria
                                            List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getVentasbyIdProdSinNombreGroupByFechaDia(item.idProducto, date);
                                            foreach (var i in listventaAgrupadabyFecha)
                                            {
                                                MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.cantidad, i.total, i.idCategoria);
                                                bool exists = listProductosinCatniInfoProd.Any(x => x.fecha == sinInfo.fecha && x.idProducto == sinInfo.idProducto);
                                                if (!exists)
                                                {
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }

                                            }

                                            // existeSinCategoria = true;

                                        }
                                        else
                                        {
                                            //Con nombre de producto pero sin categoria
                                            string NombreCat = catFacEsp.getNombreCategoriaById(IDCategoria);
                                            if (string.IsNullOrEmpty(NombreCat))
                                            {

                                                List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getVentasbyIdProdGroupByFechaDia(item.idProducto, date);
                                                foreach (var i in listventaAgrupadabyFecha)
                                                {
                                                    MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.cantidad, i.total, i.idCategoria);
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }

                                            }
                                            else
                                            {

                                                /*Agrupar por categoria los idproducto
                                                  Obtener todas las id categoria para obtener idprodudcto y buscar por idproducto en ventas 
                                                 * si no encuentra idproducto en ventas categoria no ha tenido ventas
                                                 * */
                                                //Obtiene el cantidadTotal , dineroTotal recaudado para producto agrupado por fecha
                                                List<MVentas> listVentaPorIdprod = ventasFacEsp.getVentasbyIdProdGroupByFechaDia(item.idProducto, date);
                                                foreach (var v in listVentaPorIdprod)
                                                {
                                                    //idcategoria que sera igual para distinto idproducto
                                                    MVentas ConInfo = new MVentas(v.fecha, v.idProducto, v.nombreProducto, v.cantidad, v.total, v.idCategoria);
                                                    bool exists = listProductoConCategoria.Any(x => x.fecha == ConInfo.fecha && x.idProducto == ConInfo.idProducto && x.idCategoria == ConInfo.idCategoria);
                                                    if (!exists)
                                                    {
                                                        listProductoConCategoria.Add(ConInfo);


                                                    }

                                                }

                                            }
                                        }
                                    }

                                }
                                //LLenar tabla especifico 
                                PdfPCell clFechaEsp = new PdfPCell();
                                PdfPCell clidprod = new PdfPCell();
                                PdfPCell clnombreprod = new PdfPCell();
                                PdfPCell clCant = new PdfPCell();
                                PdfPCell cltotalEsp = new PdfPCell();

                                listProductoConCategoria = listProductoConCategoria.OrderByDescending(i => i.idCategoria).ToList();

                                var q = from x in listProductoConCategoria
                                        group x.idCategoria by x.idCategoria into g
                                        let count = g.Count()
                                        /*orderby count descending*/
                                        select new { Value = g.Key, Count = count };


                                int posList = 0;
                                foreach (var x in q)
                                {
                                    tblEspecifico.WidthPercentage = 100;
                                    tblEspecifico.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat = new PdfPCell(new Phrase(catFacEsp.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clnombreCat.Colspan = 2;
                                    PdfPCell h1 = new PdfPCell(new Phrase(""));
                                    PdfPCell h2 = new PdfPCell(new Phrase(""));
                                    PdfPCell h3 = new PdfPCell(new Phrase(""));
                                    h1.Border = 0;
                                    h2.Border = 0;
                                    h3.Border = 0;
                                    tblEspecifico.AddCell(clnombreCat);
                                    tblEspecifico.AddCell(h1);
                                    tblEspecifico.AddCell(h2);
                                    tblEspecifico.AddCell(h3);

                                    clFechaEsp = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Cantidad", _standardFont));
                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("Total", _standardFont));

                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);

                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posList];

                                        clFechaEsp = new PdfPCell(new Phrase(v.fecha.ToString("d"), _standardFont));
                                        //clidprod = new PdfPCell(new Phrase(x.Value.ToString(), _standardFont));
                                        clidprod = new PdfPCell(new Phrase(v.idProducto.ToString(), _standardFont));

                                        //clnombreprod = new PdfPCell(new Phrase(v.idCategoria.ToString() + ":" + v.nombreProducto, _standardFont));
                                        clnombreprod = new PdfPCell(new Phrase(v.nombreProducto.ToString(), _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant = new PdfPCell(new Phrase(v.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp = new PdfPCell(new Phrase(v.total.ToString(), _standardFont));

                                        tblEspecifico.AddCell(clFechaEsp);
                                        tblEspecifico.AddCell(clidprod);
                                        tblEspecifico.AddCell(clnombreprod);
                                        tblEspecifico.AddCell(clCant);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblEspecifico.AddCell(cltotalEsp);
                                        dineroTotalbyCat = dineroTotalbyCat + Convert.ToInt32(v.total);
                                        tempIdpro = v.idProducto;
                                        posList = posList + 1;
                                    }
                                    //#################################
                                    //en cada categoria 
                                    //#################################
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase(dineroTotalbyCat.ToString(), _standardFont));
                                    dineroTotalbyCat = 0;

                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clCant.Border = 1;
                                    cltotalEsp.Border = 1;



                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);


                                    /////////////////////////////////////////////
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clCant.Border = 0;
                                    cltotalEsp.Border = 0;

                                    cltotalEsp.FixedHeight = 30f;
                                    clFechaEsp.FixedHeight = 30f;
                                    clidprod.FixedHeight = 30f;
                                    clnombreprod.FixedHeight = 30f;
                                    clCant.FixedHeight = 30f;
                                    cltotalEsp.FixedHeight = 30f;

                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);
                                    //}
                                    // catActual = prodFac.getIdCatbyidProd(v.idProducto);
                                    //MessageBox.Show(catActual);


                                }



                                //#######################################################
                                //###### PRODUCTOS QUE SE HALLAN ELIMINADO ############
                                //#######################################################
                                //Tabla para tabla otros que no tengan una categoria asociada pero que igualmente deben estar en reportes de ventas.
                                bool otros = false;
                                PdfPTable tblOtros = new PdfPTable(5);
                                int totalOtros = 0;
                                int subtotalCosto_otros = 0;
                                int totalCosto_otros = 0;
                                int diferenciaVenta_otros = 0;
                                int totalDiferencia_otros = 0;
                                if (listProductosinCatniInfoProd.Count > 0)
                                {
                                    otros = true;
                                    tblOtros.WidthPercentage = 100;
                                    tblOtros.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat_otros = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clnombreCat_otros.Colspan = 2;
                                    PdfPCell h11 = new PdfPCell(new Phrase(""));
                                    PdfPCell h22 = new PdfPCell(new Phrase(""));
                                    PdfPCell h33 = new PdfPCell(new Phrase(""));
                                    h11.Border = 0;
                                    h22.Border = 0;
                                    h33.Border = 0;
                                    tblOtros.AddCell(clnombreCat_otros);
                                    tblOtros.AddCell(h11);
                                    tblOtros.AddCell(h22);
                                    tblOtros.AddCell(h33);

                                    PdfPCell clFechaEsp_otros = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    PdfPCell clidprod_otros = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    PdfPCell clnombreprod_otros = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    PdfPCell clCant_otros = new PdfPCell(new Phrase("Cantidad", _standardFont));

                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    PdfPCell cltotalEsp_otros = new PdfPCell(new Phrase("Total", _standardFont));

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);


                                    foreach (var item in listProductosinCatniInfoProd)
                                    {
                                        clFechaEsp_otros = new PdfPCell(new Phrase(item.fecha.ToString("d"), _standardFont));
                                        clidprod_otros = new PdfPCell(new Phrase(item.idProducto, _standardFont));

                                        clnombreprod_otros = new PdfPCell(new Phrase(item.nombreProducto, _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant_otros = new PdfPCell(new Phrase(item.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp_otros = new PdfPCell(new Phrase(item.total.ToString(), _standardFont));

                                        tblOtros.AddCell(clFechaEsp_otros);
                                        tblOtros.AddCell(clidprod_otros);
                                        tblOtros.AddCell(clnombreprod_otros);
                                        tblOtros.AddCell(clCant_otros);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblOtros.AddCell(cltotalEsp_otros);
                                        totalOtros = totalOtros + Convert.ToInt32(item.total);

                                        if (string.IsNullOrEmpty(prodFacEsp.getPrecioCompraProducto(item.idProducto).ToString()))
                                        {
                                            subtotalCosto_otros = 0;
                                        }
                                        else
                                        {
                                            subtotalCosto_otros = item.cantidad * prodFacEsp.getPrecioCompraProducto(item.idProducto);
                                        }
                                        //MessageBox.Show("idProducto=" + item.idProducto);
                                        totalCosto_otros = totalCosto_otros + subtotalCosto_otros;
                                        diferenciaVenta_otros = totalOtros - subtotalCosto_otros; //subtotalCosto_otros - totalCosto_otros;
                                        totalDiferencia_otros = totalDiferencia_otros + diferenciaVenta_otros;


                                    }


                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clCant_otros.Border = 1;
                                    cltotalEsp_otros.Border = 1;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);
                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clCant_otros.Border = 0;
                                    cltotalEsp_otros.Border = 0;

                                    cltotalEsp_otros.FixedHeight = 50f;
                                    clFechaEsp_otros.FixedHeight = 50f;
                                    clidprod_otros.FixedHeight = 50f;
                                    clnombreprod_otros.FixedHeight = 50f;
                                    clCant_otros.FixedHeight = 50f;
                                    cltotalEsp_otros.FixedHeight = 50f;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);

                                }

                                //#######################################################
                                //######            DETALLES GENERALES       ############
                                //#######################################################

                                PdfPTable tblGenerales = new PdfPTable(5);
                                //doc.SetMargins(0f, 0f, 0f, 0f);
                                tblGenerales.HorizontalAlignment = Element.ALIGN_LEFT;
                                // Configuramos el título de las columnas de la tabla
                                PdfPCell clFecha = new PdfPCell(new Phrase("Dia", _standardFont));
                                PdfPCell clCat = new PdfPCell(new Phrase("Categoria", _standardFont));
                                PdfPCell clSubtotal = new PdfPCell(new Phrase("Subtotal ventas", _standardFont));
                                PdfPCell clSubtotalCosto = new PdfPCell(new Phrase("Subtotal costo", _standardFont));
                                PdfPCell clDiferencia = new PdfPCell(new Phrase("Diferencia", _standardFont));
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);
                                tblGenerales.AddCell(clSubtotalCosto);
                                tblGenerales.AddCell(clDiferencia);

                                categoriaFacade catFac = new categoriaFacade();
                                ProductoFacade prodFac = new ProductoFacade();

                                int subtotalventas = 0;
                                int totalsubtotalventas = 0;
                                int posListG = 0;
                                int subtotalCosto = 0;
                                int totalsubtotalcosto = 0;
                                int diferenciaVenta = 0;
                                int totalDiferencia = 0;
                                int totalCostoVenta = 0;
                                //Agrupar por categoria los idproducto
                                List<MVentas> ListcatGeneral = new List<MVentas>();
                                foreach (var x in q)
                                {
                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posListG];


                                        subtotalventas = subtotalventas + Convert.ToInt32(v.total);
                                        subtotalCosto = v.cantidad * prodFac.getPrecioCompraProducto(v.idProducto);
                                        posListG = posListG + 1;
                                        totalsubtotalcosto = totalsubtotalcosto + subtotalCosto;
                                        //MessageBox.Show(subtotalventas.ToString() + " + " + totalsubtotalcosto.ToString() + "=" + diferenciaVenta.ToString());
                                        diferenciaVenta = subtotalventas - totalsubtotalcosto;
                                        // MessageBox.Show(subtotalventas.ToString() + " - " + totalsubtotalcosto.ToString() + "=" + diferenciaVenta.ToString());
                                        totalDiferencia = totalDiferencia + diferenciaVenta;
                                        //gananciaReal = gananciaReal + totalDiferencia;

                                    }
                                    totalsubtotalventas = totalsubtotalventas + subtotalventas;
                                    clFecha = new PdfPCell(new Phrase(date.ToString("D"), _standardFont));
                                    clCat = new PdfPCell(new Phrase(catFac.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(subtotalventas.ToString(), _standardFont));
                                    clSubtotalCosto = new PdfPCell(new Phrase(subtotalCosto.ToString(), _standardFont));
                                    clDiferencia = new PdfPCell(new Phrase(diferenciaVenta.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    tblGenerales.AddCell(clSubtotalCosto);
                                    tblGenerales.AddCell(clDiferencia);
                                    subtotalventas = 0;
                                    subtotalCosto = 0;
                                    diferenciaVenta = 0;
                                    totalCostoVenta = totalCostoVenta + totalsubtotalcosto;
                                    totalsubtotalcosto = 0;

                                }
                                if (otros)
                                {
                                    clFecha = new PdfPCell(new Phrase(date.ToString("D"), _standardFont));
                                    clCat = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    clSubtotalCosto = new PdfPCell(new Phrase(subtotalCosto_otros.ToString(), _standardFont));
                                    clDiferencia = new PdfPCell(new Phrase(diferenciaVenta_otros.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    tblGenerales.AddCell(clSubtotalCosto);
                                    tblGenerales.AddCell(clDiferencia);
                                    totalsubtotalventas = totalsubtotalventas + totalOtros;

                                    subtotalCosto = subtotalCosto + totalCosto_otros;
                                    totalsubtotalcosto = totalsubtotalcosto + subtotalCosto;
                                    totalDiferencia = totalDiferencia + diferenciaVenta_otros;
                                    totalCostoVenta = totalCostoVenta + totalCosto_otros;
                                    //diferenciaVenta_otros = diferenciaVenta + diferenciaVenta_otros;
                                    //totalDiferencia_otros = totalDiferencia + diferenciaVenta_otros;
                                }

                                clFecha = new PdfPCell(new Phrase("", _standardFont));
                                clCat = new PdfPCell(new Phrase("Total ", _standardFont));
                                clSubtotal = new PdfPCell(new Phrase(totalsubtotalventas.ToString(), _standardFont));
                                clSubtotalCosto = new PdfPCell(new Phrase(totalCostoVenta.ToString(), _standardFont));
                                clDiferencia = new PdfPCell(new Phrase("", _standardFont));

                                clFecha.Border = 0;
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);
                                tblGenerales.AddCell(clSubtotalCosto);
                                tblGenerales.AddCell(clDiferencia);

                                //Ganancia Real

                                clFecha = new PdfPCell(new Phrase("", _standardFont));
                                clCat = new PdfPCell(new Phrase("", _standardFont));
                                clSubtotal = new PdfPCell(new Phrase("", _standardFont));
                                clSubtotalCosto = new PdfPCell(new Phrase("Ganancia Real", _standardFont));
                                clDiferencia = new PdfPCell(new Phrase(totalDiferencia.ToString(), _standardFont));

                                clFecha.Border = 0;
                                clCat.Border = 0;
                                clSubtotal.Border = 0;

                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);
                                tblGenerales.AddCell(clSubtotalCosto);
                                tblGenerales.AddCell(clDiferencia);

                                int vtotales = listVentasDia.Count;
                                iTextSharp.text.Font _fontDe = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                                iTextSharp.text.Paragraph ventasTotal = new iTextSharp.text.Paragraph("Total Ventas:" + vtotales.ToString(), _fontDe);
                                int efectivo = ventasFacEsp.getVentasByFechaDiaPagoEfectivo(date);
                                int cuenta = ventasFacEsp.getVentasByFechaDiaPagoCuenta(date);
                                int debito = ventasFacEsp.getVentasByFechaDiaPagoDebito(date);
                                int cheque = ventasFacEsp.getVentasByFechaDiaPagoCheque(date);
                                iTextSharp.text.Paragraph pago = new iTextSharp.text.Paragraph("Pago: Efectivo:" + efectivo.ToString() + "  Cuenta:" + cuenta.ToString() + "  Debito:" + debito.ToString() + "  Cheque:" + cheque.ToString(), _fontDe);
                                ventasTotal.Alignment = Element.ALIGN_LEFT;
                                if (otros)
                                {


                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblOtros);

                                }
                                else
                                {

                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                }


                                int page = writer.PageNumber;
                                //iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(page.ToString(), _standardFont);
                                //doc.Add(p);

                                doc.Close();
                                writer.Close();




                                //MessageBox.Show("Pdf Creado!");
                            }
                            // }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            finally
                            {
                                doc.Close();
                                writer.Close();
                            }
                            System.Diagnostics.Process.Start(exportSaveFileDialog.FileName);
                        }


                    }
                    else
                    {
                        MessageBox.Show("Dia no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                else
                {
                    MessageBox.Show("Dia no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccionar fecha", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);

            }


        }
        private void btnReporteMes_Click(object sender, RoutedEventArgs e)
        {
            if (!lfechareporte.Content.Equals(""))
            {
                if (!lventasMes.Content.Equals("0"))
                {
                    DateTime date = Convert.ToDateTime(lfechareporte.Content);
                    //MessageBox.Show(date.Month.ToString() + "/" + date.Year.ToString());
                    ventasFacade vfac = new ventasFacade();
                    List<MVentas> listVentasMes = vfac.getVentasByFechaMes(date);

                    if (listVentasMes.Count > 0)
                    {
                        SaveFileDialog exportSaveFileDialog = new SaveFileDialog();

                        exportSaveFileDialog.Title = "Guardar reporte ventas mensuales";
                        exportSaveFileDialog.Filter = "PDF(*.pdf)|*.pdf";
                        exportSaveFileDialog.FileName = "ReporteVentasMensuales";
                        exportSaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        if (exportSaveFileDialog.ShowDialog() == true)
                        {
                            // Creamos el documento con el tamaño de página tradicional
                            Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);
                            PdfWriter writer = null;
                            // Indicamos donde vamos a guardar el documento
                            try
                            {
                                writer = PdfWriter.GetInstance(doc, new FileStream(exportSaveFileDialog.FileName, FileMode.Create));
                                doc.AddCreator("Magnolia");
                                doc.Open();
                                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlLogo, System.Drawing.Imaging.ImageFormat.Png);
                                imagen.Alignment = Element.ALIGN_CENTER;
                                imagen.ScaleToFit(120f, 120f);
                                doc.Add(imagen);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Font _fontTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 25, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                iTextSharp.text.Paragraph titulo = new iTextSharp.text.Paragraph("Reporte de ventas mensual");
                                titulo.Alignment = Element.ALIGN_CENTER;
                                titulo.Font = _fontTitulo;
                                doc.Add(titulo);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Paragraph dgeneral = new iTextSharp.text.Paragraph("Detalles general");
                                dgeneral.Alignment = Element.ALIGN_LEFT;

                                //doc.AddTitle("Reporte de MVentas");
                                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                //////////////////////////////////////
                                ///////Tabla especificos    //////////
                                /////////////////////////////////////
                                iTextSharp.text.Paragraph despecifico = new iTextSharp.text.Paragraph("Detalles Especificos");
                                despecifico.Alignment = Element.ALIGN_LEFT;

                                // doc.Add(Chunk.NEWLINE);

                                categoriaFacade catFacEsp = new categoriaFacade();
                                ProductoFacade prodFacEsp = new ProductoFacade();
                                ventasFacade ventasFacEsp = new ventasFacade();
                                PdfPTable tblEspecifico = new PdfPTable(5);

                                string tempIdpro = "";

                                int dineroTotalbyCat = 0;
                                List<MVentas> listProductosinCatniInfoProd = new List<MVentas>();
                                List<MVentas> listProductoConCategoria = new List<MVentas>();

                                foreach (var item in listVentasMes)
                                {
                                    if (!tempIdpro.Equals(item.idProducto))
                                    {
                                        string IDCategoria = prodFacEsp.getIdCatbyidProd(item.idProducto);

                                        if (string.IsNullOrEmpty(IDCategoria))
                                        {
                                            //MessageBox.Show(item.idProducto + ":Sin categoria");
                                            //Agregar en celda distinta para obtener el total, no tienen categoria porque posiblemente se borra una categoria durante el mes o tiempo de uso.
                                            //Sin nombre de producto  ni categoria
                                            List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getVentasbyIdProdSinNombreGroupByFecha(item.idProducto, date);
                                            foreach (var i in listventaAgrupadabyFecha)
                                            {
                                                MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.cantidad, i.total, i.idCategoria);
                                                bool exists = listProductosinCatniInfoProd.Any(x => x.fecha == sinInfo.fecha && x.idProducto == sinInfo.idProducto);
                                                if (!exists)
                                                {
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }

                                            }

                                            // existeSinCategoria = true;

                                        }
                                        else
                                        {
                                            //Con nombre de producto pero sin categoria
                                            string NombreCat = catFacEsp.getNombreCategoriaById(IDCategoria);
                                            if (string.IsNullOrEmpty(NombreCat))
                                            {

                                                List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getVentasbyIdProdGroupByFecha(item.idProducto, date);
                                                foreach (var i in listventaAgrupadabyFecha)
                                                {
                                                    MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.cantidad, i.total, i.idCategoria);
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }

                                            }
                                            else
                                            {

                                                /*Agrupar por categoria los idproducto
                                                  Obtener todas las id categoria para obtener idprodudcto y buscar por idproducto en ventas 
                                                 * si no encuentra idproducto en ventas categoria no ha tenido ventas
                                                 * */
                                                //Obtiene el cantidadTotal , dineroTotal recaudado para producto agrupado por fecha
                                                List<MVentas> listVentaPorIdprod = ventasFacEsp.getVentasbyIdProdGroupByFecha(item.idProducto, date);
                                                foreach (var v in listVentaPorIdprod)
                                                {
                                                    //idcategoria que sera igual para distinto idproducto
                                                    MVentas ConInfo = new MVentas(v.fecha, v.idProducto, v.nombreProducto, v.cantidad, v.total, v.idCategoria);
                                                    bool exists = listProductoConCategoria.Any(x => x.fecha == ConInfo.fecha && x.idProducto == ConInfo.idProducto && x.idCategoria == ConInfo.idCategoria);
                                                    if (!exists)
                                                    {
                                                        listProductoConCategoria.Add(ConInfo);


                                                    }

                                                }

                                            }
                                        }
                                    }

                                }
                                //LLenar tabla especifico 
                                PdfPCell clFechaEsp = new PdfPCell();
                                PdfPCell clidprod = new PdfPCell();
                                PdfPCell clnombreprod = new PdfPCell();
                                PdfPCell clCant = new PdfPCell();
                                PdfPCell cltotalEsp = new PdfPCell();

                                listProductoConCategoria = listProductoConCategoria.OrderByDescending(i => i.idCategoria).ToList();

                                var q = from x in listProductoConCategoria
                                        group x.idCategoria by x.idCategoria into g
                                        let count = g.Count()
                                        /*orderby count descending*/
                                        select new { Value = g.Key, Count = count };


                                int posList = 0;
                                foreach (var x in q)
                                {
                                    tblEspecifico.WidthPercentage = 100;
                                    tblEspecifico.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat = new PdfPCell(new Phrase(catFacEsp.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clnombreCat.Colspan = 2;
                                    PdfPCell h1 = new PdfPCell(new Phrase(""));
                                    PdfPCell h2 = new PdfPCell(new Phrase(""));
                                    PdfPCell h3 = new PdfPCell(new Phrase(""));
                                    h1.Border = 0;
                                    h2.Border = 0;
                                    h3.Border = 0;
                                    tblEspecifico.AddCell(clnombreCat);
                                    tblEspecifico.AddCell(h1);
                                    tblEspecifico.AddCell(h2);
                                    tblEspecifico.AddCell(h3);

                                    clFechaEsp = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Cantidad", _standardFont));
                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("Total", _standardFont));

                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);

                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posList];

                                        clFechaEsp = new PdfPCell(new Phrase(v.fecha.ToString("d"), _standardFont));
                                        //clidprod = new PdfPCell(new Phrase(x.Value.ToString(), _standardFont));
                                        clidprod = new PdfPCell(new Phrase(v.idProducto.ToString(), _standardFont));

                                        //clnombreprod = new PdfPCell(new Phrase(v.idCategoria.ToString() + ":" + v.nombreProducto, _standardFont));
                                        clnombreprod = new PdfPCell(new Phrase(v.nombreProducto.ToString(), _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant = new PdfPCell(new Phrase(v.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp = new PdfPCell(new Phrase(v.total.ToString(), _standardFont));

                                        tblEspecifico.AddCell(clFechaEsp);
                                        tblEspecifico.AddCell(clidprod);
                                        tblEspecifico.AddCell(clnombreprod);
                                        tblEspecifico.AddCell(clCant);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblEspecifico.AddCell(cltotalEsp);
                                        dineroTotalbyCat = dineroTotalbyCat + Convert.ToInt32(v.total);
                                        tempIdpro = v.idProducto;
                                        posList = posList + 1;
                                    }
                                    //#################################
                                    //en cada categoria 
                                    //#################################
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase(dineroTotalbyCat.ToString(), _standardFont));
                                    dineroTotalbyCat = 0;

                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clCant.Border = 1;
                                    cltotalEsp.Border = 1;



                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);


                                    /////////////////////////////////////////////
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clCant.Border = 0;
                                    cltotalEsp.Border = 0;

                                    cltotalEsp.FixedHeight = 30f;
                                    clFechaEsp.FixedHeight = 30f;
                                    clidprod.FixedHeight = 30f;
                                    clnombreprod.FixedHeight = 30f;
                                    clCant.FixedHeight = 30f;
                                    cltotalEsp.FixedHeight = 30f;

                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);
                                    //}
                                    // catActual = prodFac.getIdCatbyidProd(v.idProducto);
                                    //MessageBox.Show(catActual);


                                }


                                //#######################################################
                                //###### PRODUCTOS QUE SE HALLAN ELIMINADO ############
                                //#######################################################
                                //Tabla para tabla otros que no tengan una categoria asociada pero que igualmente deben estar en reportes de ventas.
                                bool otros = false;
                                PdfPTable tblOtros = new PdfPTable(5);
                                int totalOtros = 0;
                                int subtotalCosto_otros = 0;
                                int totalCosto_otros = 0;
                                int diferenciaVenta_otros = 0;
                                int totalDiferencia_otros = 0;
                                if (listProductosinCatniInfoProd.Count > 0)
                                {
                                    otros = true;
                                    tblOtros.WidthPercentage = 100;
                                    tblOtros.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat_otros = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clnombreCat_otros.Colspan = 2;
                                    PdfPCell h11 = new PdfPCell(new Phrase(""));
                                    PdfPCell h22 = new PdfPCell(new Phrase(""));
                                    PdfPCell h33 = new PdfPCell(new Phrase(""));
                                    h11.Border = 0;
                                    h22.Border = 0;
                                    h33.Border = 0;
                                    tblOtros.AddCell(clnombreCat_otros);
                                    tblOtros.AddCell(h11);
                                    tblOtros.AddCell(h22);
                                    tblOtros.AddCell(h33);

                                    PdfPCell clFechaEsp_otros = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    PdfPCell clidprod_otros = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    PdfPCell clnombreprod_otros = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    PdfPCell clCant_otros = new PdfPCell(new Phrase("Cantidad", _standardFont));

                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    PdfPCell cltotalEsp_otros = new PdfPCell(new Phrase("Total", _standardFont));

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);


                                    foreach (var item in listProductosinCatniInfoProd)
                                    {
                                        clFechaEsp_otros = new PdfPCell(new Phrase(item.fecha.ToString("d"), _standardFont));
                                        clidprod_otros = new PdfPCell(new Phrase(item.idProducto, _standardFont));

                                        clnombreprod_otros = new PdfPCell(new Phrase(item.nombreProducto, _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant_otros = new PdfPCell(new Phrase(item.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp_otros = new PdfPCell(new Phrase(item.total.ToString(), _standardFont));

                                        tblOtros.AddCell(clFechaEsp_otros);
                                        tblOtros.AddCell(clidprod_otros);
                                        tblOtros.AddCell(clnombreprod_otros);
                                        tblOtros.AddCell(clCant_otros);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblOtros.AddCell(cltotalEsp_otros);
                                        totalOtros = totalOtros + Convert.ToInt32(item.total);

                                        if (string.IsNullOrEmpty(prodFacEsp.getPrecioCompraProducto(item.idProducto).ToString()))
                                        {
                                            subtotalCosto_otros = 0;
                                        }
                                        else
                                        {
                                            subtotalCosto_otros = item.cantidad * prodFacEsp.getPrecioCompraProducto(item.idProducto);
                                        }
                                        //MessageBox.Show("idProducto=" + item.idProducto);
                                        totalCosto_otros = totalCosto_otros + subtotalCosto_otros;
                                        diferenciaVenta_otros = totalOtros - subtotalCosto_otros; //subtotalCosto_otros - totalCosto_otros;
                                        totalDiferencia_otros = totalDiferencia_otros + diferenciaVenta_otros;


                                    }


                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clCant_otros.Border = 1;
                                    cltotalEsp_otros.Border = 1;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);
                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clCant_otros.Border = 0;
                                    cltotalEsp_otros.Border = 0;

                                    cltotalEsp_otros.FixedHeight = 50f;
                                    clFechaEsp_otros.FixedHeight = 50f;
                                    clidprod_otros.FixedHeight = 50f;
                                    clnombreprod_otros.FixedHeight = 50f;
                                    clCant_otros.FixedHeight = 50f;
                                    cltotalEsp_otros.FixedHeight = 50f;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);

                                }

                                //#######################################################
                                //######            DETALLES GENERALES       ############
                                //#######################################################

                                PdfPTable tblGenerales = new PdfPTable(5);
                                //doc.SetMargins(0f, 0f, 0f, 0f);
                                tblGenerales.HorizontalAlignment = Element.ALIGN_LEFT;
                                // Configuramos el título de las columnas de la tabla
                                PdfPCell clFecha = new PdfPCell(new Phrase("Mes", _standardFont));
                                PdfPCell clCat = new PdfPCell(new Phrase("Categoria", _standardFont));
                                PdfPCell clSubtotal = new PdfPCell(new Phrase("Subtotal ventas", _standardFont));
                                PdfPCell clSubtotalCosto = new PdfPCell(new Phrase("Subtotal costo", _standardFont));
                                PdfPCell clDiferencia = new PdfPCell(new Phrase("Diferencia", _standardFont));
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);
                                tblGenerales.AddCell(clSubtotalCosto);
                                tblGenerales.AddCell(clDiferencia);

                                categoriaFacade catFac = new categoriaFacade();
                                ProductoFacade prodFac = new ProductoFacade();

                                int subtotal = 0;
                                int total = 0;
                                int posListG = 0;
                                int subtotalCosto = 0;
                                int totalCosto = 0;
                                int totalCostoVenta = 0;
                                int diferenciaVenta = 0;
                                int totalDiferencia = 0;

                                //Agrupar por categoria los idproducto
                                List<MVentas> ListcatGeneral = new List<MVentas>();
                                foreach (var x in q)
                                {
                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posListG];


                                        subtotal = subtotal + Convert.ToInt32(v.total);
                                        subtotalCosto = v.cantidad * prodFac.getPrecioCompraProducto(v.idProducto);
                                        posListG = posListG + 1;
                                        totalCosto = totalCosto + subtotalCosto;
                                        diferenciaVenta = subtotal - totalCosto;
                                        totalDiferencia = totalDiferencia + diferenciaVenta;
                                        //gananciaReal = gananciaReal + totalDiferencia;

                                    }
                                    total = total + subtotal;
                                    clFecha = new PdfPCell(new Phrase(date.ToString("y"), _standardFont));
                                    clCat = new PdfPCell(new Phrase(catFac.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(subtotal.ToString(), _standardFont));
                                    clSubtotalCosto = new PdfPCell(new Phrase(subtotalCosto.ToString(), _standardFont));
                                    clDiferencia = new PdfPCell(new Phrase(diferenciaVenta.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    tblGenerales.AddCell(clSubtotalCosto);
                                    tblGenerales.AddCell(clDiferencia);
                                    subtotal = 0;
                                    subtotalCosto = 0;
                                    diferenciaVenta = 0;
                                    totalCostoVenta = totalCostoVenta + totalCosto;
                                    totalCosto = 0;

                                }
                                if (otros)
                                {
                                    clFecha = new PdfPCell(new Phrase(date.ToString("y"), _standardFont));
                                    clCat = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    clSubtotalCosto = new PdfPCell(new Phrase(subtotalCosto_otros.ToString(), _standardFont));
                                    clDiferencia = new PdfPCell(new Phrase(diferenciaVenta_otros.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    tblGenerales.AddCell(clSubtotalCosto);
                                    tblGenerales.AddCell(clDiferencia);
                                    total = total + totalOtros;

                                    subtotalCosto = subtotalCosto + totalCosto_otros;
                                    totalCosto = totalCosto + subtotalCosto;
                                    totalDiferencia = totalDiferencia + diferenciaVenta_otros;
                                    totalCostoVenta = totalCostoVenta + totalCosto_otros;
                                    //diferenciaVenta_otros = diferenciaVenta + diferenciaVenta_otros;
                                    //totalDiferencia_otros = totalDiferencia + diferenciaVenta_otros;
                                }

                                clFecha = new PdfPCell(new Phrase("", _standardFont));
                                clCat = new PdfPCell(new Phrase("Total ", _standardFont));
                                clSubtotal = new PdfPCell(new Phrase(total.ToString(), _standardFont));
                                clSubtotalCosto = new PdfPCell(new Phrase(totalCostoVenta.ToString(), _standardFont));
                                clDiferencia = new PdfPCell(new Phrase("", _standardFont));

                                clFecha.Border = 0;
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);
                                tblGenerales.AddCell(clSubtotalCosto);
                                tblGenerales.AddCell(clDiferencia);

                                //Ganancia Real

                                clFecha = new PdfPCell(new Phrase("", _standardFont));
                                clCat = new PdfPCell(new Phrase("", _standardFont));
                                clSubtotal = new PdfPCell(new Phrase("", _standardFont));
                                clSubtotalCosto = new PdfPCell(new Phrase("Ganancia Real", _standardFont));
                                clDiferencia = new PdfPCell(new Phrase(totalDiferencia.ToString(), _standardFont));

                                clFecha.Border = 0;
                                clCat.Border = 0;
                                clSubtotal.Border = 0;

                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);
                                tblGenerales.AddCell(clSubtotalCosto);
                                tblGenerales.AddCell(clDiferencia);


                                int vtotales = listVentasMes.Count;
                                iTextSharp.text.Font _fontDe = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                                iTextSharp.text.Paragraph ventasTotal = new iTextSharp.text.Paragraph("Total Ventas:" + vtotales.ToString(), _fontDe);
                                int efectivo = ventasFacEsp.getVentasByFechaMesPagoEfectivo(date);
                                int cuenta = ventasFacEsp.getVentasByFechaMesPagocuenta(date);
                                int debito = ventasFacEsp.getVentasByFechaMesPagodebito(date);
                                int cheque = ventasFacEsp.getVentasByFechaMesPagoCheque(date);
                                iTextSharp.text.Paragraph pago = new iTextSharp.text.Paragraph("Pago: Efectivo:" + efectivo.ToString() + "  Cuenta:" + cuenta.ToString() + "  Debito:" + debito.ToString() + "  Cheque:" + cheque.ToString(), _fontDe);
                                ventasTotal.Alignment = Element.ALIGN_LEFT;
                                if (otros)
                                {


                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblOtros);

                                }
                                else
                                {

                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                }

                                int page = writer.PageNumber;
                                //iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(page.ToString(), _standardFont);
                                //doc.Add(p);

                                doc.Close();
                                writer.Close();

                                //MessageBox.Show("Pdf Creado!");
                            }
                            // }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            finally
                            {
                                doc.Close();
                                writer.Close();
                            }
                            System.Diagnostics.Process.Start(exportSaveFileDialog.FileName);
                        }


                    }
                    else
                    {
                        MessageBox.Show("Mes no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                else
                {
                    MessageBox.Show("Mes no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccionar fecha", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }



        private void btnReporteAño_Click(object sender, RoutedEventArgs e)
        {
            if (!lfechareporte.Content.Equals(""))
            {
                if (!lventasAño.Content.Equals("0"))
                {
                    DateTime date = Convert.ToDateTime(lfechareporte.Content);
                    ventasFacade vfac = new ventasFacade();
                    List<MVentas> listVentasAño = vfac.getVentasByFechaAño(date);

                    if (listVentasAño.Count > 0)
                    {
                        SaveFileDialog exportSaveFileDialog = new SaveFileDialog();

                        exportSaveFileDialog.Title = "Guardar reporte ventas anuales";
                        exportSaveFileDialog.Filter = "PDF(*.pdf)|*.pdf";
                        exportSaveFileDialog.FileName = "ReporteVentasAnuales";
                        exportSaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        if (exportSaveFileDialog.ShowDialog() == true)
                        {
                            // Creamos el documento con el tamaño de página tradicional
                            Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);
                            PdfWriter writer = null;
                            // Indicamos donde vamos a guardar el documento
                            try
                            {
                                writer = PdfWriter.GetInstance(doc, new FileStream(exportSaveFileDialog.FileName, FileMode.Create));
                                doc.AddCreator("Magnolia");
                                doc.Open();
                                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlLogo, System.Drawing.Imaging.ImageFormat.Png);
                                imagen.Alignment = Element.ALIGN_CENTER;
                                imagen.ScaleToFit(120f, 120f);
                                doc.Add(imagen);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Font _fontTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 25, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                iTextSharp.text.Paragraph titulo = new iTextSharp.text.Paragraph("Reporte de ventas anual");
                                titulo.Alignment = Element.ALIGN_CENTER;
                                titulo.Font = _fontTitulo;
                                doc.Add(titulo);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Paragraph dgeneral = new iTextSharp.text.Paragraph("Detalles general");
                                dgeneral.Alignment = Element.ALIGN_LEFT;

                                //doc.AddTitle("Reporte de MVentas");
                                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                //////////////////////////////////////
                                ///////Tabla especificos    //////////
                                /////////////////////////////////////
                                iTextSharp.text.Paragraph despecifico = new iTextSharp.text.Paragraph("Detalles Especificos");
                                despecifico.Alignment = Element.ALIGN_LEFT;

                                // doc.Add(Chunk.NEWLINE);

                                categoriaFacade catFacEsp = new categoriaFacade();
                                ProductoFacade prodFacEsp = new ProductoFacade();
                                ventasFacade ventasFacEsp = new ventasFacade();
                                PdfPTable tblEspecifico = new PdfPTable(5);

                                string tempIdpro = "";

                                int dineroTotalbyCat = 0;
                                List<MVentas> listProductosinCatniInfoProd = new List<MVentas>();
                                List<MVentas> listProductoConCategoria = new List<MVentas>();

                                foreach (var item in listVentasAño)
                                {
                                    if (!tempIdpro.Equals(item.idProducto))
                                    {
                                        string IDCategoria = prodFacEsp.getIdCatbyidProd(item.idProducto);

                                        if (string.IsNullOrEmpty(IDCategoria))
                                        {
                                            //MessageBox.Show(item.idProducto + ":Sin categoria");
                                            //Agregar en celda distinta para obtener el total, no tienen categoria porque posiblemente se borra una categoria durante el mes o tiempo de uso.
                                            //Sin nombre de producto  ni categoria
                                            List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getVentasbyIdProdSinNombreGroupByFechaAño(item.idProducto, date);
                                            foreach (var i in listventaAgrupadabyFecha)
                                            {
                                                MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.cantidad, i.total, i.idCategoria);
                                                bool exists = listProductosinCatniInfoProd.Any(x => x.fecha == sinInfo.fecha && x.idProducto == sinInfo.idProducto);
                                                if (!exists)
                                                {
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }

                                            }

                                            // existeSinCategoria = true;

                                        }
                                        else
                                        {
                                            //Con nombre de producto pero sin categoria
                                            string NombreCat = catFacEsp.getNombreCategoriaById(IDCategoria);
                                            if (string.IsNullOrEmpty(NombreCat))
                                            {

                                                List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getVentasbyIdProdGroupByFechaAño(item.idProducto, date);
                                                foreach (var i in listventaAgrupadabyFecha)
                                                {
                                                    MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.cantidad, i.total, i.idCategoria);
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }

                                            }
                                            else
                                            {

                                                /*Agrupar por categoria los idproducto
                                                  Obtener todas las id categoria para obtener idprodudcto y buscar por idproducto en ventas 
                                                 * si no encuentra idproducto en ventas categoria no ha tenido ventas
                                                 * */
                                                //Obtiene el cantidadTotal , dineroTotal recaudado para producto agrupado por fecha
                                                List<MVentas> listVentaPorIdprod = ventasFacEsp.getVentasbyIdProdGroupByFechaAño(item.idProducto, date);
                                                foreach (var v in listVentaPorIdprod)
                                                {
                                                    //idcategoria que sera igual para distinto idproducto
                                                    MVentas ConInfo = new MVentas(v.fecha, v.idProducto, v.nombreProducto, v.cantidad, v.total, v.idCategoria);
                                                    bool exists = listProductoConCategoria.Any(x => x.fecha == ConInfo.fecha && x.idProducto == ConInfo.idProducto && x.idCategoria == ConInfo.idCategoria);
                                                    if (!exists)
                                                    {
                                                        listProductoConCategoria.Add(ConInfo);


                                                    }

                                                }

                                            }
                                        }
                                    }

                                }
                                //LLenar tabla especifico 
                                PdfPCell clFechaEsp = new PdfPCell();
                                PdfPCell clidprod = new PdfPCell();
                                PdfPCell clnombreprod = new PdfPCell();
                                PdfPCell clCant = new PdfPCell();
                                PdfPCell cltotalEsp = new PdfPCell();

                                listProductoConCategoria = listProductoConCategoria.OrderByDescending(i => i.idCategoria).ToList();

                                var q = from x in listProductoConCategoria
                                        group x.idCategoria by x.idCategoria into g
                                        let count = g.Count()
                                        /*orderby count descending*/
                                        select new { Value = g.Key, Count = count };


                                int posList = 0;
                                foreach (var x in q)
                                {
                                    tblEspecifico.WidthPercentage = 100;
                                    tblEspecifico.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat = new PdfPCell(new Phrase(catFacEsp.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clnombreCat.Colspan = 2;
                                    PdfPCell h1 = new PdfPCell(new Phrase(""));
                                    PdfPCell h2 = new PdfPCell(new Phrase(""));
                                    PdfPCell h3 = new PdfPCell(new Phrase(""));
                                    h1.Border = 0;
                                    h2.Border = 0;
                                    h3.Border = 0;
                                    tblEspecifico.AddCell(clnombreCat);
                                    tblEspecifico.AddCell(h1);
                                    tblEspecifico.AddCell(h2);
                                    tblEspecifico.AddCell(h3);

                                    clFechaEsp = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Cantidad", _standardFont));
                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("Total", _standardFont));

                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);

                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posList];

                                        clFechaEsp = new PdfPCell(new Phrase(v.fecha.ToString("d"), _standardFont));
                                        //clidprod = new PdfPCell(new Phrase(x.Value.ToString(), _standardFont));
                                        clidprod = new PdfPCell(new Phrase(v.idProducto.ToString(), _standardFont));

                                        //clnombreprod = new PdfPCell(new Phrase(v.idCategoria.ToString() + ":" + v.nombreProducto, _standardFont));
                                        clnombreprod = new PdfPCell(new Phrase(v.nombreProducto.ToString(), _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant = new PdfPCell(new Phrase(v.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp = new PdfPCell(new Phrase(v.total.ToString(), _standardFont));

                                        tblEspecifico.AddCell(clFechaEsp);
                                        tblEspecifico.AddCell(clidprod);
                                        tblEspecifico.AddCell(clnombreprod);
                                        tblEspecifico.AddCell(clCant);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblEspecifico.AddCell(cltotalEsp);
                                        dineroTotalbyCat = dineroTotalbyCat + Convert.ToInt32(v.total);
                                        tempIdpro = v.idProducto;
                                        posList = posList + 1;
                                    }
                                    //#################################
                                    //en cada categoria 
                                    //#################################
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase(dineroTotalbyCat.ToString(), _standardFont));
                                    dineroTotalbyCat = 0;

                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clCant.Border = 1;
                                    cltotalEsp.Border = 1;



                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);


                                    /////////////////////////////////////////////
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clCant.Border = 0;
                                    cltotalEsp.Border = 0;

                                    cltotalEsp.FixedHeight = 30f;
                                    clFechaEsp.FixedHeight = 30f;
                                    clidprod.FixedHeight = 30f;
                                    clnombreprod.FixedHeight = 30f;
                                    clCant.FixedHeight = 30f;
                                    cltotalEsp.FixedHeight = 30f;

                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);
                                    //}
                                    // catActual = prodFac.getIdCatbyidProd(v.idProducto);
                                    //MessageBox.Show(catActual);


                                }





                                //#######################################################
                                //###### PRODUCTOS QUE SE HALLAN ELIMINADO ############
                                //#######################################################
                                //Tabla para tabla otros que no tengan una categoria asociada pero que igualmente deben estar en reportes de ventas.
                                bool otros = false;
                                PdfPTable tblOtros = new PdfPTable(5);
                                int totalOtros = 0;
                                int subtotalCosto_otros = 0;
                                int totalCosto_otros = 0;
                                int diferenciaVenta_otros = 0;
                                int totalDiferencia_otros = 0;
                                if (listProductosinCatniInfoProd.Count > 0)
                                {
                                    otros = true;
                                    tblOtros.WidthPercentage = 100;
                                    tblOtros.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat_otros = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clnombreCat_otros.Colspan = 2;
                                    PdfPCell h11 = new PdfPCell(new Phrase(""));
                                    PdfPCell h22 = new PdfPCell(new Phrase(""));
                                    PdfPCell h33 = new PdfPCell(new Phrase(""));
                                    h11.Border = 0;
                                    h22.Border = 0;
                                    h33.Border = 0;
                                    tblOtros.AddCell(clnombreCat_otros);
                                    tblOtros.AddCell(h11);
                                    tblOtros.AddCell(h22);
                                    tblOtros.AddCell(h33);

                                    PdfPCell clFechaEsp_otros = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    PdfPCell clidprod_otros = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    PdfPCell clnombreprod_otros = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    PdfPCell clCant_otros = new PdfPCell(new Phrase("Cantidad", _standardFont));

                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    PdfPCell cltotalEsp_otros = new PdfPCell(new Phrase("Total", _standardFont));

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);


                                    foreach (var item in listProductosinCatniInfoProd)
                                    {
                                        clFechaEsp_otros = new PdfPCell(new Phrase(item.fecha.ToString("d"), _standardFont));
                                        clidprod_otros = new PdfPCell(new Phrase(item.idProducto, _standardFont));

                                        clnombreprod_otros = new PdfPCell(new Phrase(item.nombreProducto, _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant_otros = new PdfPCell(new Phrase(item.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp_otros = new PdfPCell(new Phrase(item.total.ToString(), _standardFont));

                                        tblOtros.AddCell(clFechaEsp_otros);
                                        tblOtros.AddCell(clidprod_otros);
                                        tblOtros.AddCell(clnombreprod_otros);
                                        tblOtros.AddCell(clCant_otros);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblOtros.AddCell(cltotalEsp_otros);
                                        totalOtros = totalOtros + Convert.ToInt32(item.total);

                                        if (string.IsNullOrEmpty(prodFacEsp.getPrecioCompraProducto(item.idProducto).ToString()))
                                        {
                                            subtotalCosto_otros = 0;
                                        }
                                        else
                                        {
                                            subtotalCosto_otros = item.cantidad * prodFacEsp.getPrecioCompraProducto(item.idProducto);
                                        }
                                        //MessageBox.Show("idProducto=" + item.idProducto);
                                        totalCosto_otros = totalCosto_otros + subtotalCosto_otros;
                                        diferenciaVenta_otros = totalOtros - subtotalCosto_otros; //subtotalCosto_otros - totalCosto_otros;
                                        totalDiferencia_otros = totalDiferencia_otros + diferenciaVenta_otros;


                                    }


                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clCant_otros.Border = 1;
                                    cltotalEsp_otros.Border = 1;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);
                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clCant_otros.Border = 0;
                                    cltotalEsp_otros.Border = 0;

                                    cltotalEsp_otros.FixedHeight = 50f;
                                    clFechaEsp_otros.FixedHeight = 50f;
                                    clidprod_otros.FixedHeight = 50f;
                                    clnombreprod_otros.FixedHeight = 50f;
                                    clCant_otros.FixedHeight = 50f;
                                    cltotalEsp_otros.FixedHeight = 50f;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);

                                }

                                //#######################################################
                                //######            DETALLES GENERALES       ############
                                //#######################################################

                                PdfPTable tblGenerales = new PdfPTable(5);
                                //doc.SetMargins(0f, 0f, 0f, 0f);
                                tblGenerales.HorizontalAlignment = Element.ALIGN_LEFT;
                                // Configuramos el título de las columnas de la tabla
                                PdfPCell clFecha = new PdfPCell(new Phrase("Año", _standardFont));
                                PdfPCell clCat = new PdfPCell(new Phrase("Categoria", _standardFont));
                                PdfPCell clSubtotal = new PdfPCell(new Phrase("Subtotal ventas", _standardFont));
                                PdfPCell clSubtotalCosto = new PdfPCell(new Phrase("Subtotal costo", _standardFont));
                                PdfPCell clDiferencia = new PdfPCell(new Phrase("Diferencia", _standardFont));
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);
                                tblGenerales.AddCell(clSubtotalCosto);
                                tblGenerales.AddCell(clDiferencia);

                                categoriaFacade catFac = new categoriaFacade();
                                ProductoFacade prodFac = new ProductoFacade();

                                int subtotal = 0;
                                int total = 0;
                                int posListG = 0;
                                int subtotalCosto = 0;
                                int totalCosto = 0;
                                int totalCostoVenta = 0;
                                int diferenciaVenta = 0;
                                int totalDiferencia = 0;

                                //Agrupar por categoria los idproducto
                                List<MVentas> ListcatGeneral = new List<MVentas>();
                                foreach (var x in q)
                                {
                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posListG];


                                        subtotal = subtotal + Convert.ToInt32(v.total);
                                        subtotalCosto = v.cantidad * prodFac.getPrecioCompraProducto(v.idProducto);
                                        posListG = posListG + 1;
                                        totalCosto = totalCosto + subtotalCosto;
                                        diferenciaVenta = subtotal - totalCosto;
                                        totalDiferencia = totalDiferencia + diferenciaVenta;
                                        //gananciaReal = gananciaReal + totalDiferencia;

                                    }
                                    total = total + subtotal;
                                    clFecha = new PdfPCell(new Phrase(date.ToString("yyyy"), _standardFont));
                                    clCat = new PdfPCell(new Phrase(catFac.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(subtotal.ToString(), _standardFont));
                                    clSubtotalCosto = new PdfPCell(new Phrase(subtotalCosto.ToString(), _standardFont));
                                    clDiferencia = new PdfPCell(new Phrase(diferenciaVenta.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    tblGenerales.AddCell(clSubtotalCosto);
                                    tblGenerales.AddCell(clDiferencia);
                                    subtotal = 0;
                                    subtotalCosto = 0;
                                    diferenciaVenta = 0;
                                    totalCostoVenta = totalCostoVenta + totalCosto;
                                    totalCosto = 0;

                                }
                                if (otros)
                                {
                                    clFecha = new PdfPCell(new Phrase(date.ToString("yyyy"), _standardFont));
                                    clCat = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    clSubtotalCosto = new PdfPCell(new Phrase(subtotalCosto_otros.ToString(), _standardFont));
                                    clDiferencia = new PdfPCell(new Phrase(diferenciaVenta_otros.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    tblGenerales.AddCell(clSubtotalCosto);
                                    tblGenerales.AddCell(clDiferencia);
                                    total = total + totalOtros;

                                    subtotalCosto = subtotalCosto + totalCosto_otros;
                                    totalCosto = totalCosto + subtotalCosto;
                                    totalDiferencia = totalDiferencia + diferenciaVenta_otros;
                                    //diferenciaVenta_otros = diferenciaVenta + diferenciaVenta_otros;
                                    //totalDiferencia_otros = totalDiferencia + diferenciaVenta_otros;
                                    totalCostoVenta = totalCostoVenta + totalCosto_otros;
                                }

                                clFecha = new PdfPCell(new Phrase("", _standardFont));
                                clCat = new PdfPCell(new Phrase("Total ", _standardFont));
                                clSubtotal = new PdfPCell(new Phrase(total.ToString(), _standardFont));
                                clSubtotalCosto = new PdfPCell(new Phrase(totalCostoVenta.ToString(), _standardFont));
                                clDiferencia = new PdfPCell(new Phrase("", _standardFont));

                                clFecha.Border = 0;
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);
                                tblGenerales.AddCell(clSubtotalCosto);
                                tblGenerales.AddCell(clDiferencia);

                                //Ganancia Real

                                clFecha = new PdfPCell(new Phrase("", _standardFont));
                                clCat = new PdfPCell(new Phrase("", _standardFont));
                                clSubtotal = new PdfPCell(new Phrase("", _standardFont));
                                clSubtotalCosto = new PdfPCell(new Phrase("Ganancia Real", _standardFont));
                                clDiferencia = new PdfPCell(new Phrase(totalDiferencia.ToString(), _standardFont));

                                clFecha.Border = 0;
                                clCat.Border = 0;
                                clSubtotal.Border = 0;

                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);
                                tblGenerales.AddCell(clSubtotalCosto);
                                tblGenerales.AddCell(clDiferencia);


                                int vtotales = listVentasAño.Count;
                                iTextSharp.text.Font _fontDe = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                                iTextSharp.text.Paragraph ventasTotal = new iTextSharp.text.Paragraph("Total Ventas:" + vtotales.ToString(), _fontDe);
                                int efectivo = ventasFacEsp.getVentasByFechaAñoPagoEfectivo(date);
                                int cuenta = ventasFacEsp.getVentasByFechaAñoPagocuenta(date);
                                int debito = ventasFacEsp.getVentasByFechasAñoPagodebito(date);
                                int cheque = ventasFacEsp.getVentasByFechaAñoPagoCheque(date);
                                iTextSharp.text.Paragraph pago = new iTextSharp.text.Paragraph("Pago: Efectivo:" + efectivo.ToString() + "  Cuenta:" + cuenta.ToString() + "  Debito:" + debito.ToString() + "  Cheque:" + cheque.ToString(), _fontDe);
                                ventasTotal.Alignment = Element.ALIGN_LEFT;
                                if (otros)
                                {


                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblOtros);

                                }
                                else
                                {

                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                }

                                int page = writer.PageNumber;
                                //iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(page.ToString(), _standardFont);
                                //doc.Add(p);

                                doc.Close();
                                writer.Close();

                                //MessageBox.Show("Pdf Creado!");
                            }
                            // }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            finally
                            {
                                doc.Close();
                                writer.Close();
                            }
                            System.Diagnostics.Process.Start(exportSaveFileDialog.FileName);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Año no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                else
                {
                    MessageBox.Show("Año no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccionar fecha", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }




        }

        private void btnCostoDia_Click(object sender, RoutedEventArgs e)
        {

            if (!lfechareporte.Content.Equals(""))
            {
                if (!lventasDia.Content.Equals("0"))
                {
                    DateTime date = Convert.ToDateTime(lfechareporte.Content);
                    ventasFacade vfac = new ventasFacade();
                    List<MVentas> listVentasDia = vfac.getVentasByFechaDia(date);

                    if (listVentasDia.Count > 0)
                    {
                        SaveFileDialog exportSaveFileDialog = new SaveFileDialog();

                        exportSaveFileDialog.Title = "Guardar reporte de costos productos vendidos en el dia";
                        exportSaveFileDialog.Filter = "PDF(*.pdf)|*.pdf";
                        exportSaveFileDialog.FileName = "ReportecostoProdvendidosDia";
                        exportSaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        if (exportSaveFileDialog.ShowDialog() == true)
                        {
                            // Creamos el documento con el tamaño de página tradicional
                            Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);
                            PdfWriter writer = null;
                            // Indicamos donde vamos a guardar el documento
                            try
                            {
                                writer = PdfWriter.GetInstance(doc, new FileStream(exportSaveFileDialog.FileName, FileMode.Create));
                                doc.AddCreator("Magnolia");
                                doc.Open();
                                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlLogo, System.Drawing.Imaging.ImageFormat.Png);
                                imagen.Alignment = Element.ALIGN_CENTER;
                                imagen.ScaleToFit(120f, 120f);
                                doc.Add(imagen);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Font _fontTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 25, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                iTextSharp.text.Paragraph titulo = new iTextSharp.text.Paragraph("Reporte de costo ventas diaria");
                                titulo.Alignment = Element.ALIGN_CENTER;
                                titulo.Font = _fontTitulo;
                                doc.Add(titulo);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Paragraph dgeneral = new iTextSharp.text.Paragraph("Detalles general");
                                dgeneral.Alignment = Element.ALIGN_LEFT;

                                //doc.AddTitle("Reporte de MVentas");
                                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                //////////////////////////////////////
                                ///////Tabla especificos    //////////
                                /////////////////////////////////////
                                iTextSharp.text.Paragraph despecifico = new iTextSharp.text.Paragraph("Detalles Especificos");
                                despecifico.Alignment = Element.ALIGN_LEFT;

                                // doc.Add(Chunk.NEWLINE);

                                categoriaFacade catFacEsp = new categoriaFacade();
                                ProductoFacade prodFacEsp = new ProductoFacade();
                                ventasFacade ventasFacEsp = new ventasFacade();
                                PdfPTable tblEspecifico = new PdfPTable(5);

                                string tempIdpro = "";

                                int dineroTotalbyCat = 0;
                                List<MVentas> listProductosinCatniInfoProd = new List<MVentas>();
                                List<MVentas> listProductoConCategoria = new List<MVentas>();

                                foreach (var item in listVentasDia)
                                {
                                    if (!tempIdpro.Equals(item.idProducto))
                                    {
                                        string IDCategoria = prodFacEsp.getIdCatbyidProd(item.idProducto);

                                        if (string.IsNullOrEmpty(IDCategoria))
                                        {
                                            //MessageBox.Show(item.idProducto + ":Sin categoria");
                                            //Agregar en celda distinta para obtener el total, no tienen categoria porque posiblemente se borra una categoria durante el mes o tiempo de uso.
                                            //Sin nombre de producto  ni categoria
                                            List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getCostoVentasbyIdProdSinNombreGroupByFechaDia(item.idProducto, date);
                                            foreach (var i in listventaAgrupadabyFecha)
                                            {
                                                MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.precioReal, i.cantidad, i.total, i.idCategoria);
                                                bool exists = listProductosinCatniInfoProd.Any(x => x.fecha == sinInfo.fecha && x.idProducto == sinInfo.idProducto);
                                                if (!exists)
                                                {
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            //Con nombre de producto pero sin categoria
                                            string NombreCat = catFacEsp.getNombreCategoriaById(IDCategoria);
                                            if (string.IsNullOrEmpty(NombreCat))
                                            {

                                                List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getCostosVentasbyIdProdGroupByFechaDia(item.idProducto, date);
                                                foreach (var i in listventaAgrupadabyFecha)
                                                {
                                                    MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.precioReal, i.cantidad, i.total, i.idCategoria);
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }

                                            }
                                            else
                                            {

                                                /*Agrupar por categoria los idproducto
                                                  Obtener todas las id categoria para obtener idprodudcto y buscar por idproducto en ventas 
                                                 * si no encuentra idproducto en ventas categoria no ha tenido ventas
                                                 * */
                                                //Obtiene el cantidadTotal , dineroTotal recaudado para producto agrupado por fecha
                                                List<MVentas> listVentaPorIdprod = ventasFacEsp.getCostosVentasbyIdProdGroupByFechaDia(item.idProducto, date);
                                                foreach (var v in listVentaPorIdprod)
                                                {
                                                    //idcategoria que sera igual para distinto idproducto
                                                    MVentas ConInfo = new MVentas(v.fecha, v.idProducto, v.nombreProducto, v.precioReal, v.cantidad, v.total, v.idCategoria);
                                                    bool exists = listProductoConCategoria.Any(x => x.fecha == ConInfo.fecha && x.idProducto == ConInfo.idProducto && x.idCategoria == ConInfo.idCategoria);
                                                    if (!exists)
                                                    {
                                                        listProductoConCategoria.Add(ConInfo);


                                                    }

                                                }

                                            }
                                        }
                                    }

                                }
                                //LLenar tabla especifico 
                                PdfPCell clFechaEsp = new PdfPCell();
                                PdfPCell clidprod = new PdfPCell();
                                PdfPCell clnombreprod = new PdfPCell();
                                PdfPCell clPrecioReal = new PdfPCell();
                                PdfPCell clCant = new PdfPCell();
                                PdfPCell cltotalEsp = new PdfPCell();

                                listProductoConCategoria = listProductoConCategoria.OrderByDescending(i => i.idCategoria).ToList();

                                var q = from x in listProductoConCategoria
                                        group x.idCategoria by x.idCategoria into g
                                        let count = g.Count()
                                        /*orderby count descending*/
                                        select new { Value = g.Key, Count = count };


                                int posList = 0;
                                foreach (var x in q)
                                {
                                    tblEspecifico.WidthPercentage = 100;
                                    tblEspecifico.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat = new PdfPCell(new Phrase(catFacEsp.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clnombreCat.Colspan = 2;
                                    PdfPCell h1 = new PdfPCell(new Phrase(""));
                                    PdfPCell h2 = new PdfPCell(new Phrase(""));
                                    PdfPCell h3 = new PdfPCell(new Phrase(""));
                                    h1.Border = 0;
                                    h2.Border = 0;
                                    h3.Border = 0;
                                    tblEspecifico.AddCell(clnombreCat);
                                    tblEspecifico.AddCell(h1);
                                    tblEspecifico.AddCell(h2);
                                    tblEspecifico.AddCell(h3);

                                    clFechaEsp = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    clPrecioReal = new PdfPCell(new Phrase("Precio Compra", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Cantidad", _standardFont));
                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("Total", _standardFont));



                                    tblEspecifico.AddCell(clFechaEsp);
                                    //tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clPrecioReal);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);

                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posList];

                                        clFechaEsp = new PdfPCell(new Phrase(v.fecha.ToString("d"), _standardFont));
                                        //clidprod = new PdfPCell(new Phrase(x.Value.ToString(), _standardFont));
                                        clidprod = new PdfPCell(new Phrase(v.idProducto.ToString(), _standardFont));

                                        //clnombreprod = new PdfPCell(new Phrase(v.idCategoria.ToString() + ":" + v.nombreProducto, _standardFont));
                                        clnombreprod = new PdfPCell(new Phrase(v.nombreProducto.ToString(), _standardFont));
                                        clPrecioReal = new PdfPCell(new Phrase(v.precioReal.ToString(), _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant = new PdfPCell(new Phrase(v.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp = new PdfPCell(new Phrase(v.total.ToString(), _standardFont));

                                        tblEspecifico.AddCell(clFechaEsp);
                                        //tblEspecifico.AddCell(clidprod);
                                        tblEspecifico.AddCell(clnombreprod);
                                        tblEspecifico.AddCell(clPrecioReal);
                                        tblEspecifico.AddCell(clCant);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblEspecifico.AddCell(cltotalEsp);
                                        dineroTotalbyCat = dineroTotalbyCat + Convert.ToInt32(v.total);
                                        tempIdpro = v.idProducto;
                                        posList = posList + 1;
                                    }
                                    //#################################
                                    //en cada categoria 
                                    //#################################
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clPrecioReal = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase(dineroTotalbyCat.ToString(), _standardFont));
                                    dineroTotalbyCat = 0;

                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clPrecioReal.Border = 0;
                                    clCant.Border = 1;
                                    cltotalEsp.Border = 1;



                                    tblEspecifico.AddCell(clFechaEsp);
                                    //tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clPrecioReal);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);


                                    /////////////////////////////////////////////
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clPrecioReal = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clPrecioReal.Border = 0;
                                    clCant.Border = 0;
                                    cltotalEsp.Border = 0;

                                    cltotalEsp.FixedHeight = 30f;
                                    clFechaEsp.FixedHeight = 30f;
                                    clidprod.FixedHeight = 30f;
                                    clnombreprod.FixedHeight = 30f;
                                    clPrecioReal.FixedHeight = 30f;
                                    clCant.FixedHeight = 30f;
                                    cltotalEsp.FixedHeight = 30f;

                                    tblEspecifico.AddCell(clFechaEsp);
                                    //tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clPrecioReal);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);
                                    //}
                                    // catActual = prodFac.getIdCatbyidProd(v.idProducto);
                                    //MessageBox.Show(catActual);


                                }




                                //#######################################################
                                //###### PRODUCTOS QUE SE HALLAN ELIMINADO ############
                                //#######################################################
                                //Tabla para tabla otros que no tengan una categoria asociada pero que igualmente deben estar en reportes de ventas.
                                bool otros = false;
                                PdfPTable tblOtros = new PdfPTable(5);
                                int totalOtros = 0;
                                if (listProductosinCatniInfoProd.Count > 0)
                                {
                                    otros = true;
                                    tblOtros.WidthPercentage = 100;
                                    tblOtros.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat_otros = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clnombreCat_otros.Colspan = 2;
                                    PdfPCell h11 = new PdfPCell(new Phrase(""));
                                    PdfPCell h22 = new PdfPCell(new Phrase(""));
                                    PdfPCell h33 = new PdfPCell(new Phrase(""));
                                    h11.Border = 0;
                                    h22.Border = 0;
                                    h33.Border = 0;
                                    tblOtros.AddCell(clnombreCat_otros);
                                    tblOtros.AddCell(h11);
                                    tblOtros.AddCell(h22);
                                    tblOtros.AddCell(h33);

                                    PdfPCell clFechaEsp_otros = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    PdfPCell clidprod_otros = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    PdfPCell clnombreprod_otros = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    PdfPCell clPrecioReal_otros = new PdfPCell(new Phrase("Precio Compra", _standardFont));
                                    PdfPCell clCant_otros = new PdfPCell(new Phrase("Cantidad", _standardFont));
                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    PdfPCell cltotalEsp_otros = new PdfPCell(new Phrase("Total", _standardFont));

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    //tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clPrecioReal_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);


                                    foreach (var item in listProductosinCatniInfoProd)
                                    {
                                        clFechaEsp_otros = new PdfPCell(new Phrase(item.fecha.ToString("d"), _standardFont));
                                        clidprod_otros = new PdfPCell(new Phrase(item.idProducto, _standardFont));

                                        clnombreprod_otros = new PdfPCell(new Phrase(item.nombreProducto, _standardFont));
                                        clPrecioReal_otros = new PdfPCell(new Phrase(item.precioReal, _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant_otros = new PdfPCell(new Phrase(item.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp_otros = new PdfPCell(new Phrase(item.total.ToString(), _standardFont));

                                        tblOtros.AddCell(clFechaEsp_otros);
                                        //tblOtros.AddCell(clidprod_otros);
                                        tblOtros.AddCell(clnombreprod_otros);
                                        tblOtros.AddCell(clPrecioReal_otros);
                                        tblOtros.AddCell(clCant_otros);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblOtros.AddCell(cltotalEsp_otros);
                                        totalOtros = totalOtros + Convert.ToInt32(item.total);

                                    }


                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clPrecioReal_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clPrecioReal_otros.Border = 0;
                                    clCant_otros.Border = 1;
                                    cltotalEsp_otros.Border = 1;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    //tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clPrecioReal_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);
                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clPrecioReal_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clPrecioReal_otros.Border = 0;
                                    clCant_otros.Border = 0;
                                    cltotalEsp_otros.Border = 0;

                                    cltotalEsp_otros.FixedHeight = 50f;
                                    clFechaEsp_otros.FixedHeight = 50f;
                                    clidprod_otros.FixedHeight = 50f;
                                    clnombreprod_otros.FixedHeight = 50f;
                                    clPrecioReal_otros.FixedHeight = 50f;
                                    clCant_otros.FixedHeight = 50f;
                                    cltotalEsp_otros.FixedHeight = 50f;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    //tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clPrecioReal_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);

                                }

                                //#######################################################
                                //######            DETALLES GENERALES       ############
                                //#######################################################

                                PdfPTable tblGenerales = new PdfPTable(3);
                                //doc.SetMargins(0f, 0f, 0f, 0f);
                                tblGenerales.HorizontalAlignment = Element.ALIGN_LEFT;
                                // Configuramos el título de las columnas de la tabla
                                PdfPCell clFecha = new PdfPCell(new Phrase("Dia", _standardFont));
                                PdfPCell clCat = new PdfPCell(new Phrase("Categoria", _standardFont));
                                PdfPCell clSubtotal = new PdfPCell(new Phrase("Subtotal", _standardFont));
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);

                                categoriaFacade catFac = new categoriaFacade();
                                ProductoFacade prodFac = new ProductoFacade();

                                int subtotal = 0;
                                int total = 0;
                                int posListG = 0;
                                //Agrupar por categoria los idproducto
                                List<MVentas> ListcatGeneral = new List<MVentas>();
                                foreach (var x in q)
                                {
                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posListG];


                                        subtotal = subtotal + Convert.ToInt32(v.total);
                                        posListG = posListG + 1;
                                    }
                                    total = total + subtotal;
                                    clFecha = new PdfPCell(new Phrase(date.ToString("D"), _standardFont));
                                    clCat = new PdfPCell(new Phrase(catFac.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(subtotal.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    subtotal = 0;
                                }
                                if (otros)
                                {
                                    clFecha = new PdfPCell(new Phrase(date.ToString("D"), _standardFont));
                                    clCat = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    total = total + totalOtros;
                                }

                                clFecha = new PdfPCell(new Phrase("", _standardFont));
                                clCat = new PdfPCell(new Phrase("Total Costo Ventas Producto", _standardFont));
                                clSubtotal = new PdfPCell(new Phrase(total.ToString(), _standardFont));

                                clFecha.Border = 0;
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);


                                int vtotales = listVentasDia.Count;
                                iTextSharp.text.Font _fontDe = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                                iTextSharp.text.Paragraph ventasTotal = new iTextSharp.text.Paragraph("Total Ventas:" + vtotales.ToString(), _fontDe);
                                int efectivo = ventasFacEsp.getVentasByFechaDiaPagoEfectivo(date);
                                int cuenta = ventasFacEsp.getVentasByFechaDiaPagoCuenta(date);
                                int debito = ventasFacEsp.getVentasByFechaDiaPagoDebito(date);
                                int cheque = ventasFacEsp.getVentasByFechaDiaPagoCheque(date);
                                iTextSharp.text.Paragraph pago = new iTextSharp.text.Paragraph("Pago: Efectivo:" + efectivo.ToString() + "  Cuenta:" + cuenta.ToString() + "  Debito:" + debito.ToString() + "  Cheque:" + cheque.ToString(), _fontDe);
                                ventasTotal.Alignment = Element.ALIGN_LEFT;
                                if (otros)
                                {


                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblOtros);

                                }
                                else
                                {

                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                }
                                int page = writer.PageNumber;
                                //iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(page.ToString(), _standardFont);
                                //doc.Add(p);

                                doc.Close();
                                writer.Close();

                                //MessageBox.Show("Pdf Creado!");
                            }
                            // }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            finally
                            {
                                doc.Close();
                                writer.Close();
                            }
                            System.Diagnostics.Process.Start(exportSaveFileDialog.FileName);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Dia no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                else
                {
                    MessageBox.Show("Dia no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccionar fecha", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }



        }

        private void btnCostoMes_Click(object sender, RoutedEventArgs e)
        {
            if (!lfechareporte.Content.Equals(""))
            {
                if (!lventasMes.Content.Equals("0"))
                {
                    DateTime date = Convert.ToDateTime(lfechareporte.Content);
                    ventasFacade vfac = new ventasFacade();
                    List<MVentas> listVentasMes = vfac.getVentasByFechaMes(date);

                    if (listVentasMes.Count > 0)
                    {
                        SaveFileDialog exportSaveFileDialog = new SaveFileDialog();

                        exportSaveFileDialog.Title = "Guardar reporte productos vendidos en el mes";
                        exportSaveFileDialog.Filter = "PDF(*.pdf)|*.pdf";
                        exportSaveFileDialog.FileName = "ReportecostoProdvendidosMes";
                        exportSaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        if (exportSaveFileDialog.ShowDialog() == true)
                        {
                            // Creamos el documento con el tamaño de página tradicional
                            Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);
                            PdfWriter writer = null;
                            // Indicamos donde vamos a guardar el documento
                            try
                            {
                                writer = PdfWriter.GetInstance(doc, new FileStream(exportSaveFileDialog.FileName, FileMode.Create));
                                doc.AddCreator("Magnolia");
                                doc.Open();
                                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlLogo, System.Drawing.Imaging.ImageFormat.Png);
                                imagen.Alignment = Element.ALIGN_CENTER;
                                imagen.ScaleToFit(120f, 120f);
                                doc.Add(imagen);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Font _fontTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 25, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                iTextSharp.text.Paragraph titulo = new iTextSharp.text.Paragraph("Reporte de costo ventas mensuales");
                                titulo.Alignment = Element.ALIGN_CENTER;
                                titulo.Font = _fontTitulo;
                                doc.Add(titulo);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Paragraph dgeneral = new iTextSharp.text.Paragraph("Detalles general");
                                dgeneral.Alignment = Element.ALIGN_LEFT;

                                //doc.AddTitle("Reporte de MVentas");
                                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                //////////////////////////////////////
                                ///////Tabla especificos    //////////
                                /////////////////////////////////////
                                iTextSharp.text.Paragraph despecifico = new iTextSharp.text.Paragraph("Detalles Especificos");
                                despecifico.Alignment = Element.ALIGN_LEFT;

                                // doc.Add(Chunk.NEWLINE);

                                categoriaFacade catFacEsp = new categoriaFacade();
                                ProductoFacade prodFacEsp = new ProductoFacade();
                                ventasFacade ventasFacEsp = new ventasFacade();
                                PdfPTable tblEspecifico = new PdfPTable(5);

                                string tempIdpro = "";

                                int dineroTotalbyCat = 0;
                                List<MVentas> listProductosinCatniInfoProd = new List<MVentas>();
                                List<MVentas> listProductoConCategoria = new List<MVentas>();

                                foreach (var item in listVentasMes)
                                {
                                    if (!tempIdpro.Equals(item.idProducto))
                                    {
                                        string IDCategoria = prodFacEsp.getIdCatbyidProd(item.idProducto);

                                        if (string.IsNullOrEmpty(IDCategoria))
                                        {
                                            //MessageBox.Show(item.idProducto + ":Sin categoria");
                                            //Agregar en celda distinta para obtener el total, no tienen categoria porque posiblemente se borra una categoria durante el mes o tiempo de uso.
                                            //Sin nombre de producto  ni categoria
                                            List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getCostoVentasbyIdProdSinNombreGroupByFecha(item.idProducto, date);
                                            foreach (var i in listventaAgrupadabyFecha)
                                            {
                                                MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.precioReal, i.cantidad, i.total, i.idCategoria);
                                                bool exists = listProductosinCatniInfoProd.Any(x => x.fecha == sinInfo.fecha && x.idProducto == sinInfo.idProducto);
                                                if (!exists)
                                                {
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            //Con nombre de producto pero sin categoria
                                            string NombreCat = catFacEsp.getNombreCategoriaById(IDCategoria);
                                            if (string.IsNullOrEmpty(NombreCat))
                                            {

                                                List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getCostoVentasbyIdProdGroupByFecha(item.idProducto, date);
                                                foreach (var i in listventaAgrupadabyFecha)
                                                {
                                                    MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.precioReal, i.cantidad, i.total, i.idCategoria);
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }

                                            }
                                            else
                                            {

                                                /*Agrupar por categoria los idproducto
                                                  Obtener todas las id categoria para obtener idprodudcto y buscar por idproducto en ventas 
                                                 * si no encuentra idproducto en ventas categoria no ha tenido ventas
                                                 * */
                                                //Obtiene el cantidadTotal , dineroTotal recaudado para producto agrupado por fecha
                                                List<MVentas> listVentaPorIdprod = ventasFacEsp.getCostoVentasbyIdProdGroupByFecha(item.idProducto, date);
                                                foreach (var v in listVentaPorIdprod)
                                                {
                                                    //idcategoria que sera igual para distinto idproducto
                                                    MVentas ConInfo = new MVentas(v.fecha, v.idProducto, v.nombreProducto, v.precioReal, v.cantidad, v.total, v.idCategoria);
                                                    bool exists = listProductoConCategoria.Any(x => x.fecha == ConInfo.fecha && x.idProducto == ConInfo.idProducto && x.idCategoria == ConInfo.idCategoria);
                                                    if (!exists)
                                                    {
                                                        listProductoConCategoria.Add(ConInfo);


                                                    }

                                                }

                                            }
                                        }
                                    }

                                }
                                //LLenar tabla especifico 
                                PdfPCell clFechaEsp = new PdfPCell();
                                PdfPCell clidprod = new PdfPCell();
                                PdfPCell clnombreprod = new PdfPCell();
                                PdfPCell clPrecioReal = new PdfPCell();
                                PdfPCell clCant = new PdfPCell();
                                PdfPCell cltotalEsp = new PdfPCell();

                                listProductoConCategoria = listProductoConCategoria.OrderByDescending(i => i.idCategoria).ToList();

                                var q = from x in listProductoConCategoria
                                        group x.idCategoria by x.idCategoria into g
                                        let count = g.Count()
                                        /*orderby count descending*/
                                        select new { Value = g.Key, Count = count };


                                int posList = 0;
                                foreach (var x in q)
                                {
                                    tblEspecifico.WidthPercentage = 100;
                                    tblEspecifico.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat = new PdfPCell(new Phrase(catFacEsp.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clnombreCat.Colspan = 2;
                                    PdfPCell h1 = new PdfPCell(new Phrase(""));
                                    PdfPCell h2 = new PdfPCell(new Phrase(""));
                                    PdfPCell h3 = new PdfPCell(new Phrase(""));
                                    h1.Border = 0;
                                    h2.Border = 0;
                                    h3.Border = 0;
                                    tblEspecifico.AddCell(clnombreCat);
                                    tblEspecifico.AddCell(h1);
                                    tblEspecifico.AddCell(h2);
                                    tblEspecifico.AddCell(h3);

                                    clFechaEsp = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    clPrecioReal = new PdfPCell(new Phrase("Precio Compra", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Cantidad", _standardFont));
                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("Total", _standardFont));



                                    tblEspecifico.AddCell(clFechaEsp);
                                    //tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clPrecioReal);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);

                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posList];

                                        clFechaEsp = new PdfPCell(new Phrase(v.fecha.ToString("d"), _standardFont));
                                        //clidprod = new PdfPCell(new Phrase(x.Value.ToString(), _standardFont));
                                        clidprod = new PdfPCell(new Phrase(v.idProducto.ToString(), _standardFont));

                                        //clnombreprod = new PdfPCell(new Phrase(v.idCategoria.ToString() + ":" + v.nombreProducto, _standardFont));
                                        clnombreprod = new PdfPCell(new Phrase(v.nombreProducto.ToString(), _standardFont));
                                        clPrecioReal = new PdfPCell(new Phrase(v.precioReal.ToString(), _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant = new PdfPCell(new Phrase(v.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp = new PdfPCell(new Phrase(v.total.ToString(), _standardFont));

                                        tblEspecifico.AddCell(clFechaEsp);
                                        //tblEspecifico.AddCell(clidprod);
                                        tblEspecifico.AddCell(clnombreprod);
                                        tblEspecifico.AddCell(clPrecioReal);
                                        tblEspecifico.AddCell(clCant);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblEspecifico.AddCell(cltotalEsp);
                                        dineroTotalbyCat = dineroTotalbyCat + Convert.ToInt32(v.total);
                                        tempIdpro = v.idProducto;
                                        posList = posList + 1;
                                    }
                                    //#################################
                                    //en cada categoria 
                                    //#################################
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clPrecioReal = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase(dineroTotalbyCat.ToString(), _standardFont));
                                    dineroTotalbyCat = 0;

                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clPrecioReal.Border = 0;
                                    clCant.Border = 1;
                                    cltotalEsp.Border = 1;



                                    tblEspecifico.AddCell(clFechaEsp);
                                    //tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clPrecioReal);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);


                                    /////////////////////////////////////////////
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clPrecioReal = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clPrecioReal.Border = 0;
                                    clCant.Border = 0;
                                    cltotalEsp.Border = 0;

                                    cltotalEsp.FixedHeight = 30f;
                                    clFechaEsp.FixedHeight = 30f;
                                    clidprod.FixedHeight = 30f;
                                    clnombreprod.FixedHeight = 30f;
                                    clPrecioReal.FixedHeight = 30f;
                                    clCant.FixedHeight = 30f;
                                    cltotalEsp.FixedHeight = 30f;

                                    tblEspecifico.AddCell(clFechaEsp);
                                    //tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clPrecioReal);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);
                                    //}
                                    // catActual = prodFac.getIdCatbyidProd(v.idProducto);
                                    //MessageBox.Show(catActual);


                                }




                                //#######################################################
                                //###### PRODUCTOS QUE SE HALLAN ELIMINADO ############
                                //#######################################################
                                //Tabla para tabla otros que no tengan una categoria asociada pero que igualmente deben estar en reportes de ventas.
                                bool otros = false;
                                PdfPTable tblOtros = new PdfPTable(5);
                                int totalOtros = 0;
                                if (listProductosinCatniInfoProd.Count > 0)
                                {
                                    otros = true;
                                    tblOtros.WidthPercentage = 100;
                                    tblOtros.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat_otros = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clnombreCat_otros.Colspan = 2;
                                    PdfPCell h11 = new PdfPCell(new Phrase(""));
                                    PdfPCell h22 = new PdfPCell(new Phrase(""));
                                    PdfPCell h33 = new PdfPCell(new Phrase(""));
                                    h11.Border = 0;
                                    h22.Border = 0;
                                    h33.Border = 0;
                                    tblOtros.AddCell(clnombreCat_otros);
                                    tblOtros.AddCell(h11);
                                    tblOtros.AddCell(h22);
                                    tblOtros.AddCell(h33);

                                    PdfPCell clFechaEsp_otros = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    PdfPCell clidprod_otros = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    PdfPCell clnombreprod_otros = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    PdfPCell clPrecioReal_otros = new PdfPCell(new Phrase("Precio Compra", _standardFont));
                                    PdfPCell clCant_otros = new PdfPCell(new Phrase("Cantidad", _standardFont));
                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    PdfPCell cltotalEsp_otros = new PdfPCell(new Phrase("Total", _standardFont));

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    //tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clPrecioReal_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);


                                    foreach (var item in listProductosinCatniInfoProd)
                                    {
                                        clFechaEsp_otros = new PdfPCell(new Phrase(item.fecha.ToString("d"), _standardFont));
                                        clidprod_otros = new PdfPCell(new Phrase(item.idProducto, _standardFont));

                                        clnombreprod_otros = new PdfPCell(new Phrase(item.nombreProducto, _standardFont));
                                        clPrecioReal_otros = new PdfPCell(new Phrase(item.precioReal, _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant_otros = new PdfPCell(new Phrase(item.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp_otros = new PdfPCell(new Phrase(item.total.ToString(), _standardFont));

                                        tblOtros.AddCell(clFechaEsp_otros);
                                        //tblOtros.AddCell(clidprod_otros);
                                        tblOtros.AddCell(clnombreprod_otros);
                                        tblOtros.AddCell(clPrecioReal_otros);
                                        tblOtros.AddCell(clCant_otros);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblOtros.AddCell(cltotalEsp_otros);
                                        totalOtros = totalOtros + Convert.ToInt32(item.total);

                                    }


                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clPrecioReal_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clPrecioReal_otros.Border = 0;
                                    clCant_otros.Border = 1;
                                    cltotalEsp_otros.Border = 1;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    //tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clPrecioReal_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);
                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clPrecioReal_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clPrecioReal_otros.Border = 0;
                                    clCant_otros.Border = 0;
                                    cltotalEsp_otros.Border = 0;

                                    cltotalEsp_otros.FixedHeight = 50f;
                                    clFechaEsp_otros.FixedHeight = 50f;
                                    clidprod_otros.FixedHeight = 50f;
                                    clnombreprod_otros.FixedHeight = 50f;
                                    clPrecioReal_otros.FixedHeight = 50f;
                                    clCant_otros.FixedHeight = 50f;
                                    cltotalEsp_otros.FixedHeight = 50f;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    //tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clPrecioReal_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);

                                }

                                //#######################################################
                                //######            DETALLES GENERALES       ############
                                //#######################################################

                                PdfPTable tblGenerales = new PdfPTable(3);
                                //doc.SetMargins(0f, 0f, 0f, 0f);
                                tblGenerales.HorizontalAlignment = Element.ALIGN_LEFT;
                                // Configuramos el título de las columnas de la tabla
                                PdfPCell clFecha = new PdfPCell(new Phrase("Mes", _standardFont));
                                PdfPCell clCat = new PdfPCell(new Phrase("Categoria", _standardFont));
                                PdfPCell clSubtotal = new PdfPCell(new Phrase("Subtotal", _standardFont));
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);

                                categoriaFacade catFac = new categoriaFacade();
                                ProductoFacade prodFac = new ProductoFacade();

                                int subtotal = 0;
                                int total = 0;
                                int posListG = 0;
                                //Agrupar por categoria los idproducto
                                List<MVentas> ListcatGeneral = new List<MVentas>();
                                foreach (var x in q)
                                {
                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posListG];


                                        subtotal = subtotal + Convert.ToInt32(v.total);
                                        posListG = posListG + 1;
                                    }
                                    total = total + subtotal;
                                    clFecha = new PdfPCell(new Phrase(date.ToString("y"), _standardFont));
                                    clCat = new PdfPCell(new Phrase(catFac.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(subtotal.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    subtotal = 0;
                                }
                                if (otros)
                                {
                                    clFecha = new PdfPCell(new Phrase(date.ToString("y"), _standardFont));
                                    clCat = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    total = total + totalOtros;
                                }

                                clFecha = new PdfPCell(new Phrase("", _standardFont));
                                clCat = new PdfPCell(new Phrase("Total Costo Ventas Producto", _standardFont));
                                clSubtotal = new PdfPCell(new Phrase(total.ToString(), _standardFont));

                                clFecha.Border = 0;
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);

                                int vtotales = listVentasMes.Count;
                                iTextSharp.text.Font _fontDe = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                                iTextSharp.text.Paragraph ventasTotal = new iTextSharp.text.Paragraph("Total Ventas:" + vtotales.ToString(), _fontDe);
                                int efectivo = ventasFacEsp.getVentasByFechaMesPagoEfectivo(date);
                                int cuenta = ventasFacEsp.getVentasByFechaMesPagocuenta(date);
                                int debito = ventasFacEsp.getVentasByFechaMesPagodebito(date);
                                int cheque = ventasFacEsp.getVentasByFechaMesPagoCheque(date);
                                iTextSharp.text.Paragraph pago = new iTextSharp.text.Paragraph("Pago: Efectivo:" + efectivo.ToString() + "  Cuenta:" + cuenta.ToString() + "  Debito:" + debito.ToString() + "  Cheque:" + cheque.ToString(), _fontDe);
                                ventasTotal.Alignment = Element.ALIGN_LEFT;
                                if (otros)
                                {


                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblOtros);

                                }
                                else
                                {

                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                }

                                int page = writer.PageNumber;
                                //iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(page.ToString(), _standardFont);
                                //doc.Add(p);

                                doc.Close();
                                writer.Close();
                                //System.Diagnostics.Process.Start(urlpdf + "ReporteCostoMes.pdf");
                                //MessageBox.Show("Pdf Creado!");
                            }
                            // }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            finally
                            {
                                doc.Close();
                                writer.Close();
                            }
                            System.Diagnostics.Process.Start(exportSaveFileDialog.FileName);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Mes no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                else
                {
                    MessageBox.Show("Mes no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccionar fecha", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void btnCostoAño_Click(object sender, RoutedEventArgs e)
        {

            if (!lfechareporte.Content.Equals(""))
            {
                if (!lventasAño.Content.Equals("0"))
                {
                    DateTime date = Convert.ToDateTime(lfechareporte.Content);
                    ventasFacade vfac = new ventasFacade();
                    List<MVentas> listVentasAño = vfac.getVentasByFechaAño(date);

                    if (listVentasAño.Count > 0)
                    {
                        SaveFileDialog exportSaveFileDialog = new SaveFileDialog();

                        exportSaveFileDialog.Title = "Guardar reporte de costos ventas anuales";
                        exportSaveFileDialog.Filter = "PDF(*.pdf)|*.pdf";
                        exportSaveFileDialog.FileName = "ReporteCostoProdVendidosAño";
                        exportSaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        if (exportSaveFileDialog.ShowDialog() == true)
                        {
                            // Creamos el documento con el tamaño de página tradicional
                            Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);
                            PdfWriter writer = null;
                            // Indicamos donde vamos a guardar el documento
                            try
                            {
                                writer = PdfWriter.GetInstance(doc, new FileStream(exportSaveFileDialog.FileName, FileMode.Create));
                                doc.AddCreator("Magnolia");
                                doc.Open();
                                iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlLogo, System.Drawing.Imaging.ImageFormat.Png);
                                imagen.Alignment = Element.ALIGN_CENTER;
                                imagen.ScaleToFit(120f, 120f);
                                doc.Add(imagen);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Font _fontTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 25, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                iTextSharp.text.Paragraph titulo = new iTextSharp.text.Paragraph("Reporte de costo ventas anuales");
                                titulo.Alignment = Element.ALIGN_CENTER;
                                titulo.Font = _fontTitulo;
                                doc.Add(titulo);
                                doc.Add(Chunk.NEWLINE);

                                iTextSharp.text.Paragraph dgeneral = new iTextSharp.text.Paragraph("Detalles general");
                                dgeneral.Alignment = Element.ALIGN_LEFT;

                                //doc.AddTitle("Reporte de MVentas");
                                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                //////////////////////////////////////
                                ///////Tabla especificos    //////////
                                /////////////////////////////////////
                                iTextSharp.text.Paragraph despecifico = new iTextSharp.text.Paragraph("Detalles Especificos");
                                despecifico.Alignment = Element.ALIGN_LEFT;

                                // doc.Add(Chunk.NEWLINE);

                                categoriaFacade catFacEsp = new categoriaFacade();
                                ProductoFacade prodFacEsp = new ProductoFacade();
                                ventasFacade ventasFacEsp = new ventasFacade();
                                PdfPTable tblEspecifico = new PdfPTable(5);

                                string tempIdpro = "";

                                int dineroTotalbyCat = 0;
                                List<MVentas> listProductosinCatniInfoProd = new List<MVentas>();
                                List<MVentas> listProductoConCategoria = new List<MVentas>();

                                foreach (var item in listVentasAño)
                                {
                                    if (!tempIdpro.Equals(item.idProducto))
                                    {
                                        string IDCategoria = prodFacEsp.getIdCatbyidProd(item.idProducto);

                                        if (string.IsNullOrEmpty(IDCategoria))
                                        {
                                            //MessageBox.Show(item.idProducto + ":Sin categoria");
                                            //Agregar en celda distinta para obtener el total, no tienen categoria porque posiblemente se borra una categoria durante el mes o tiempo de uso.
                                            //Sin nombre de producto  ni categoria
                                            List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getCostoVentasbyIdProdSinNombreGroupByFechaAño(item.idProducto, date);
                                            foreach (var i in listventaAgrupadabyFecha)
                                            {
                                                MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.precioReal, i.cantidad, i.total, i.idCategoria);
                                                bool exists = listProductosinCatniInfoProd.Any(x => x.fecha == sinInfo.fecha && x.idProducto == sinInfo.idProducto);
                                                if (!exists)
                                                {
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }

                                            }

                                            // existeSinCategoria = true;

                                        }
                                        else
                                        {
                                            //Con nombre de producto pero sin categoria
                                            string NombreCat = catFacEsp.getNombreCategoriaById(IDCategoria);
                                            if (string.IsNullOrEmpty(NombreCat))
                                            {

                                                List<MVentas> listventaAgrupadabyFecha = ventasFacEsp.getCostoVentasbyIdProdGroupByFechaAño(item.idProducto, date);
                                                foreach (var i in listventaAgrupadabyFecha)
                                                {
                                                    MVentas sinInfo = new MVentas(i.fecha, i.idProducto, i.nombreProducto, i.precioReal, i.cantidad, i.total, i.idCategoria);
                                                    listProductosinCatniInfoProd.Add(sinInfo);
                                                }

                                            }
                                            else
                                            {

                                                /*Agrupar por categoria los idproducto
                                                  Obtener todas las id categoria para obtener idprodudcto y buscar por idproducto en ventas 
                                                 * si no encuentra idproducto en ventas categoria no ha tenido ventas
                                                 * */
                                                //Obtiene el cantidadTotal , dineroTotal recaudado para producto agrupado por fecha
                                                List<MVentas> listVentaPorIdprod = ventasFacEsp.getCostoVentasbyIdProdGroupByFechaAño(item.idProducto, date);
                                                foreach (var v in listVentaPorIdprod)
                                                {
                                                    //idcategoria que sera igual para distinto idproducto
                                                    MVentas ConInfo = new MVentas(v.fecha, v.idProducto, v.nombreProducto, v.precioReal, v.cantidad, v.total, v.idCategoria);
                                                    bool exists = listProductoConCategoria.Any(x => x.fecha == ConInfo.fecha && x.idProducto == ConInfo.idProducto && x.idCategoria == ConInfo.idCategoria);
                                                    if (!exists)
                                                    {
                                                        listProductoConCategoria.Add(ConInfo);


                                                    }

                                                }

                                            }
                                        }
                                    }

                                }
                                //LLenar tabla especifico 
                                PdfPCell clFechaEsp = new PdfPCell();
                                PdfPCell clidprod = new PdfPCell();
                                PdfPCell clnombreprod = new PdfPCell();
                                PdfPCell clCant = new PdfPCell();
                                PdfPCell cltotalEsp = new PdfPCell();

                                listProductoConCategoria = listProductoConCategoria.OrderByDescending(i => i.idCategoria).ToList();

                                var q = from x in listProductoConCategoria
                                        group x.idCategoria by x.idCategoria into g
                                        let count = g.Count()
                                        /*orderby count descending*/
                                        select new { Value = g.Key, Count = count };


                                int posList = 0;
                                foreach (var x in q)
                                {
                                    tblEspecifico.WidthPercentage = 100;
                                    tblEspecifico.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat = new PdfPCell(new Phrase(catFacEsp.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clnombreCat.Colspan = 2;
                                    PdfPCell h1 = new PdfPCell(new Phrase(""));
                                    PdfPCell h2 = new PdfPCell(new Phrase(""));
                                    PdfPCell h3 = new PdfPCell(new Phrase(""));
                                    h1.Border = 0;
                                    h2.Border = 0;
                                    h3.Border = 0;
                                    tblEspecifico.AddCell(clnombreCat);
                                    tblEspecifico.AddCell(h1);
                                    tblEspecifico.AddCell(h2);
                                    tblEspecifico.AddCell(h3);

                                    clFechaEsp = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Cantidad", _standardFont));
                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("Total", _standardFont));

                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);

                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posList];

                                        clFechaEsp = new PdfPCell(new Phrase(v.fecha.ToString("d"), _standardFont));
                                        //clidprod = new PdfPCell(new Phrase(x.Value.ToString(), _standardFont));
                                        clidprod = new PdfPCell(new Phrase(v.idProducto.ToString(), _standardFont));

                                        //clnombreprod = new PdfPCell(new Phrase(v.idCategoria.ToString() + ":" + v.nombreProducto, _standardFont));
                                        clnombreprod = new PdfPCell(new Phrase(v.nombreProducto.ToString(), _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant = new PdfPCell(new Phrase(v.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp = new PdfPCell(new Phrase(v.total.ToString(), _standardFont));

                                        tblEspecifico.AddCell(clFechaEsp);
                                        tblEspecifico.AddCell(clidprod);
                                        tblEspecifico.AddCell(clnombreprod);
                                        tblEspecifico.AddCell(clCant);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblEspecifico.AddCell(cltotalEsp);
                                        dineroTotalbyCat = dineroTotalbyCat + Convert.ToInt32(v.total);
                                        tempIdpro = v.idProducto;
                                        posList = posList + 1;
                                    }
                                    //#################################
                                    //en cada categoria 
                                    //#################################
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase(dineroTotalbyCat.ToString(), _standardFont));
                                    dineroTotalbyCat = 0;

                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clCant.Border = 1;
                                    cltotalEsp.Border = 1;



                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);


                                    /////////////////////////////////////////////
                                    clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                    clCant = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp.Border = 0;
                                    clidprod.Border = 0;
                                    clnombreprod.Border = 0;
                                    clCant.Border = 0;
                                    cltotalEsp.Border = 0;

                                    cltotalEsp.FixedHeight = 30f;
                                    clFechaEsp.FixedHeight = 30f;
                                    clidprod.FixedHeight = 30f;
                                    clnombreprod.FixedHeight = 30f;
                                    clCant.FixedHeight = 30f;
                                    cltotalEsp.FixedHeight = 30f;

                                    tblEspecifico.AddCell(clFechaEsp);
                                    tblEspecifico.AddCell(clidprod);
                                    tblEspecifico.AddCell(clnombreprod);
                                    tblEspecifico.AddCell(clCant);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblEspecifico.AddCell(cltotalEsp);
                                    //}
                                    // catActual = prodFac.getIdCatbyidProd(v.idProducto);
                                    //MessageBox.Show(catActual);


                                }


                                //#######################################################
                                //###### PRODUCTOS QUE SE HALLAN ELIMINADO ############
                                //#######################################################
                                //Tabla para tabla otros que no tengan una categoria asociada pero que igualmente deben estar en reportes de ventas.
                                bool otros = false;
                                PdfPTable tblOtros = new PdfPTable(5);
                                int totalOtros = 0;
                                if (listProductosinCatniInfoProd.Count > 0)
                                {
                                    otros = true;
                                    tblOtros.WidthPercentage = 100;
                                    tblOtros.HorizontalAlignment = Element.ALIGN_LEFT;
                                    PdfPCell clnombreCat_otros = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clnombreCat_otros.Colspan = 2;
                                    PdfPCell h11 = new PdfPCell(new Phrase(""));
                                    PdfPCell h22 = new PdfPCell(new Phrase(""));
                                    PdfPCell h33 = new PdfPCell(new Phrase(""));
                                    h11.Border = 0;
                                    h22.Border = 0;
                                    h33.Border = 0;
                                    tblOtros.AddCell(clnombreCat_otros);
                                    tblOtros.AddCell(h11);
                                    tblOtros.AddCell(h22);
                                    tblOtros.AddCell(h33);

                                    PdfPCell clFechaEsp_otros = new PdfPCell(new Phrase("Fecha", _standardFont));
                                    PdfPCell clidprod_otros = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                    PdfPCell clnombreprod_otros = new PdfPCell(new Phrase("Nombre", _standardFont));
                                    PdfPCell clPrecioReal_otros = new PdfPCell(new Phrase("Precio Compra", _standardFont));
                                    PdfPCell clCant_otros = new PdfPCell(new Phrase("Cantidad", _standardFont));
                                    //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                    PdfPCell cltotalEsp_otros = new PdfPCell(new Phrase("Total", _standardFont));

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    //tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clPrecioReal_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);


                                    foreach (var item in listProductosinCatniInfoProd)
                                    {
                                        clFechaEsp_otros = new PdfPCell(new Phrase(item.fecha.ToString("d"), _standardFont));
                                        clidprod_otros = new PdfPCell(new Phrase(item.idProducto, _standardFont));

                                        clnombreprod_otros = new PdfPCell(new Phrase(item.nombreProducto, _standardFont));
                                        clPrecioReal_otros = new PdfPCell(new Phrase(item.precioReal, _standardFont));
                                        //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                        clCant_otros = new PdfPCell(new Phrase(item.cantidad.ToString(), _standardFont));
                                        //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                        cltotalEsp_otros = new PdfPCell(new Phrase(item.total.ToString(), _standardFont));

                                        tblOtros.AddCell(clFechaEsp_otros);
                                        //tblOtros.AddCell(clidprod_otros);
                                        tblOtros.AddCell(clnombreprod_otros);
                                        tblOtros.AddCell(clPrecioReal_otros);
                                        tblOtros.AddCell(clCant_otros);
                                        //tblEspecifico.AddCell(clTipoPag);
                                        tblOtros.AddCell(cltotalEsp_otros);
                                        totalOtros = totalOtros + Convert.ToInt32(item.total);

                                    }


                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clPrecioReal_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clPrecioReal_otros.Border = 0;
                                    clCant_otros.Border = 1;
                                    cltotalEsp_otros.Border = 1;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    //tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clPrecioReal_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);
                                    clFechaEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clidprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clnombreprod_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clPrecioReal_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clCant_otros = new PdfPCell(new Phrase("", _standardFont));
                                    //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                    cltotalEsp_otros = new PdfPCell(new Phrase("", _standardFont));
                                    clFechaEsp_otros.Border = 0;
                                    clidprod_otros.Border = 0;
                                    clnombreprod_otros.Border = 0;
                                    clPrecioReal_otros.Border = 0;
                                    clCant_otros.Border = 0;
                                    cltotalEsp_otros.Border = 0;

                                    cltotalEsp_otros.FixedHeight = 50f;
                                    clFechaEsp_otros.FixedHeight = 50f;
                                    clidprod_otros.FixedHeight = 50f;
                                    clnombreprod_otros.FixedHeight = 50f;
                                    clPrecioReal_otros.FixedHeight = 50f;
                                    clCant_otros.FixedHeight = 50f;
                                    cltotalEsp_otros.FixedHeight = 50f;

                                    tblOtros.AddCell(clFechaEsp_otros);
                                    //tblOtros.AddCell(clidprod_otros);
                                    tblOtros.AddCell(clnombreprod_otros);
                                    tblOtros.AddCell(clPrecioReal_otros);
                                    tblOtros.AddCell(clCant_otros);
                                    //tblEspecifico.AddCell(clTipoPag);
                                    tblOtros.AddCell(cltotalEsp_otros);

                                }

                                //#######################################################
                                //######            DETALLES GENERALES       ############
                                //#######################################################

                                PdfPTable tblGenerales = new PdfPTable(3);
                                //doc.SetMargins(0f, 0f, 0f, 0f);
                                tblGenerales.HorizontalAlignment = Element.ALIGN_LEFT;
                                // Configuramos el título de las columnas de la tabla
                                PdfPCell clFecha = new PdfPCell(new Phrase("Año", _standardFont));
                                PdfPCell clCat = new PdfPCell(new Phrase("Categoria", _standardFont));
                                PdfPCell clSubtotal = new PdfPCell(new Phrase("Subtotal", _standardFont));
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);

                                categoriaFacade catFac = new categoriaFacade();
                                ProductoFacade prodFac = new ProductoFacade();

                                int subtotal = 0;
                                int total = 0;
                                int posListG = 0;
                                //Agrupar por categoria los idproducto
                                List<MVentas> ListcatGeneral = new List<MVentas>();
                                foreach (var x in q)
                                {
                                    for (int i = 0; i < x.Count; i++)
                                    {
                                        var v = listProductoConCategoria[posListG];


                                        subtotal = subtotal + Convert.ToInt32(v.total);
                                        posListG = posListG + 1;
                                    }
                                    total = total + subtotal;
                                    clFecha = new PdfPCell(new Phrase(date.ToString("yyyy"), _standardFont));
                                    clCat = new PdfPCell(new Phrase(catFac.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(subtotal.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    subtotal = 0;
                                }
                                if (otros)
                                {
                                    clFecha = new PdfPCell(new Phrase(date.ToString("yyyy"), _standardFont));
                                    clCat = new PdfPCell(new Phrase("Otros", _standardFont));
                                    clSubtotal = new PdfPCell(new Phrase(totalOtros.ToString(), _standardFont));
                                    tblGenerales.AddCell(clFecha);
                                    tblGenerales.AddCell(clCat);
                                    tblGenerales.AddCell(clSubtotal);
                                    total = total + totalOtros;
                                }

                                clFecha = new PdfPCell(new Phrase("", _standardFont));
                                clCat = new PdfPCell(new Phrase("Total Costo Ventas Producto", _standardFont));
                                clSubtotal = new PdfPCell(new Phrase(total.ToString(), _standardFont));

                                clFecha.Border = 0;
                                tblGenerales.AddCell(clFecha);
                                tblGenerales.AddCell(clCat);
                                tblGenerales.AddCell(clSubtotal);


                                int vtotales = listVentasAño.Count;
                                iTextSharp.text.Font _fontDe = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);
                                iTextSharp.text.Paragraph ventasTotal = new iTextSharp.text.Paragraph("Total Ventas:" + vtotales.ToString(), _fontDe);
                                int efectivo = ventasFacEsp.getVentasByFechaAñoPagoEfectivo(date);
                                int cuenta = ventasFacEsp.getVentasByFechaAñoPagocuenta(date);
                                int debito = ventasFacEsp.getVentasByFechasAñoPagodebito(date);
                                int cheque = ventasFacEsp.getVentasByFechaAñoPagoCheque(date);
                                iTextSharp.text.Paragraph pago = new iTextSharp.text.Paragraph("Pago: Efectivo:" + efectivo.ToString() + "  Cuenta:" + cuenta.ToString() + "  Debito:" + debito.ToString() + "  Cheque:" + cheque.ToString(), _fontDe);
                                ventasTotal.Alignment = Element.ALIGN_LEFT;
                                if (otros)
                                {


                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblOtros);

                                }
                                else
                                {

                                    doc.Add(dgeneral);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(tblGenerales);
                                    doc.Add(ventasTotal);
                                    doc.Add(pago);
                                    doc.Add(Chunk.NEWLINE);
                                    doc.Add(despecifico);
                                    doc.Add(new Chunk("\n"));
                                    doc.Add(tblEspecifico);
                                    doc.Add(new Chunk("\n"));
                                }
                                int page = writer.PageNumber;
                                //iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(page.ToString(), _standardFont);
                                //doc.Add(p);

                                doc.Close();
                                writer.Close();

                                //MessageBox.Show("Pdf Creado!");
                            }
                            // }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                            finally
                            {
                                doc.Close();
                                writer.Close();
                            }
                            System.Diagnostics.Process.Start(exportSaveFileDialog.FileName);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Año no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
                else
                {
                    MessageBox.Show("Año no ha tenido ventas", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccionar fecha", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void CantidadTotalRubrosyProductos(DateTime fecha)
        {
            ProductoFacade prodFac = new ProductoFacade();
            categoriaFacade catFac = new categoriaFacade();
            ltotalproduc.Content = prodFac.getTotalProductosbyMes(fecha).ToString();
            ltotalrubros.Content = catFac.getTotalCategoriabyMes(fecha).ToString();


        }

        private void btnproductosXrubro_Click(object sender, RoutedEventArgs e)
        {

            categoriaFacade catFac1 = new categoriaFacade();

            int hay = catFac1.getTotalCategoriabyMes(MesRubroProd);
            if (!string.IsNullOrEmpty(month))
            {
                if (hay > 0)
                {

                    //buscar productos que fueron ingresados o modificados dentro de la fecha indicada
                    ProductoFacade prodFac = new ProductoFacade();
                    List<Producto> listProductoMes = prodFac.getALLProductosbyFechaMes(Convert.ToDateTime(month));
                    //MessageBox.Show(listProductoMes.Count.ToString());
                    //categorizar productos y obtener total de costo de compra en reporte

                    if (listProductoMes.Count > 0)
                    {
                        SaveFileDialog exportSaveFileDialog = new SaveFileDialog();
                        exportSaveFileDialog.Title = "Guardar reporte de costo para productos adquiridos";
                        exportSaveFileDialog.Filter = "PDF(*.pdf)|*.pdf";
                        exportSaveFileDialog.FileName = "ReporteCostoProdAdquiridos";
                        exportSaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        if (exportSaveFileDialog.ShowDialog() == true)
                        {

                            string tempIdpro = "";

                            int dineroTotalbyCat = 0;
                            List<Producto> listProductoConCategoria = listProductoMes;

                            foreach (var item in listProductoMes)
                            {
                                if (!tempIdpro.Equals(item.idProducto))
                                {

                                    Document doc = new Document(PageSize.LETTER, 50, 50, 50, 50);
                                    PdfWriter writer = null;
                                    // Indicamos donde vamos a guardar el documento
                                    try
                                    {
                                        writer = PdfWriter.GetInstance(doc, new FileStream(exportSaveFileDialog.FileName, FileMode.Create));
                                        doc.AddCreator("Magnolia");
                                        doc.Open();
                                        iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(urlLogo, System.Drawing.Imaging.ImageFormat.Png);
                                        imagen.Alignment = Element.ALIGN_CENTER;
                                        imagen.ScaleToFit(120f, 120f);
                                        doc.Add(imagen);
                                        doc.Add(Chunk.NEWLINE);

                                        iTextSharp.text.Font _fontTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 25, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                        iTextSharp.text.Paragraph titulo = new iTextSharp.text.Paragraph("Reporte de costo ventas mensuales");
                                        titulo.Alignment = Element.ALIGN_CENTER;
                                        titulo.Font = _fontTitulo;
                                        doc.Add(titulo);
                                        doc.Add(Chunk.NEWLINE);

                                        iTextSharp.text.Paragraph dgeneral = new iTextSharp.text.Paragraph("Detalles general");
                                        dgeneral.Alignment = Element.ALIGN_LEFT;

                                        //doc.AddTitle("Reporte de MVentas");
                                        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK);


                                        //////////////////////////////////////
                                        ///////Tabla especificos    //////////
                                        /////////////////////////////////////

                                        categoriaFacade catFacEsp = new categoriaFacade();
                                        ProductoFacade prodFacEsp = new ProductoFacade();
                                        ventasFacade ventasFacEsp = new ventasFacade();
                                        PdfPTable tblEspecifico = new PdfPTable(5);

                                        iTextSharp.text.Paragraph despecifico = new iTextSharp.text.Paragraph("Detalles Especificos");
                                        despecifico.Alignment = Element.ALIGN_LEFT;
                                        //LLenar tabla especifico 
                                        PdfPCell clFechaEsp = new PdfPCell();
                                        //PdfPCell clidprod = new PdfPCell();
                                        PdfPCell clnombreprod = new PdfPCell();
                                        PdfPCell clPrecioReal = new PdfPCell();
                                        PdfPCell clCant = new PdfPCell();
                                        PdfPCell cltotalEsp = new PdfPCell();

                                        listProductoConCategoria = listProductoConCategoria.OrderByDescending(i => i.idCategoria).ToList();
                                        //MessageBox.Show("TotalProductos:" + listProductoConCategoria.Count().ToString());
                                        var q = from x in listProductoConCategoria
                                                group x.idCategoria by x.idCategoria into g
                                                let count = g.Count()
                                                /*orderby count descending*/
                                                select new { Value = g.Key, Count = count };


                                        int posList = 0;
                                        foreach (var x in q)
                                        {
                                            tblEspecifico.WidthPercentage = 100;
                                            tblEspecifico.HorizontalAlignment = Element.ALIGN_LEFT;
                                            PdfPCell clnombreCat = new PdfPCell(new Phrase(catFacEsp.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                            clnombreCat.Colspan = 2;
                                            PdfPCell h1 = new PdfPCell(new Phrase(""));
                                            PdfPCell h2 = new PdfPCell(new Phrase(""));
                                            PdfPCell h3 = new PdfPCell(new Phrase(""));
                                            h1.Border = 0;
                                            h2.Border = 0;
                                            h3.Border = 0;
                                            tblEspecifico.AddCell(clnombreCat);
                                            tblEspecifico.AddCell(h1);
                                            tblEspecifico.AddCell(h2);
                                            tblEspecifico.AddCell(h3);

                                            clFechaEsp = new PdfPCell(new Phrase("Fecha", _standardFont));
                                            //clidprod = new PdfPCell(new Phrase("IDProducto", _standardFont));
                                            clnombreprod = new PdfPCell(new Phrase("Nombre", _standardFont));
                                            clPrecioReal = new PdfPCell(new Phrase("Precio Compra", _standardFont));
                                            clCant = new PdfPCell(new Phrase("Stock", _standardFont));
                                            //PdfPCell clTipoPag = new PdfPCell(new Phrase("Tipo Pago", _standardFont));
                                            cltotalEsp = new PdfPCell(new Phrase("Total", _standardFont));

                                            tblEspecifico.AddCell(clFechaEsp);
                                            // tblEspecifico.AddCell(clidprod);
                                            tblEspecifico.AddCell(clnombreprod);
                                            tblEspecifico.AddCell(clPrecioReal);
                                            tblEspecifico.AddCell(clCant);
                                            //tblEspecifico.AddCell(clTipoPag);
                                            tblEspecifico.AddCell(cltotalEsp);

                                            for (int i = 0; i < x.Count; i++)
                                            {
                                                var v = listProductoConCategoria[posList];

                                                clFechaEsp = new PdfPCell(new Phrase(v.fecha.ToString("y"), _standardFont));
                                                //clidprod = new PdfPCell(new Phrase(x.Value.ToString(), _standardFont));
                                                // clidprod = new PdfPCell(new Phrase(v.idProducto.ToString(), _standardFont));

                                                //clnombreprod = new PdfPCell(new Phrase(v.idCategoria.ToString() + ":" + v.nombreProducto, _standardFont));
                                                clnombreprod = new PdfPCell(new Phrase(v.nombre.ToString(), _standardFont));
                                                //clnombreprod= new PdfPCell(new Phrase(prodFacEsp.getnombreProdbyidProd(v.idProducto), _standardFont));
                                                clPrecioReal = new PdfPCell(new Phrase(v.precioReal.ToString(), _standardFont));
                                                clCant = new PdfPCell(new Phrase(v.stock.ToString(), _standardFont));
                                                //clTipoPag = new PdfPCell(new Phrase("", _standardFont));
                                                cltotalEsp = new PdfPCell(new Phrase(v.total.ToString(), _standardFont));

                                                tblEspecifico.AddCell(clFechaEsp);
                                                //tblEspecifico.AddCell(clidprod);
                                                tblEspecifico.AddCell(clnombreprod);
                                                tblEspecifico.AddCell(clPrecioReal);
                                                tblEspecifico.AddCell(clCant);
                                                //tblEspecifico.AddCell(clTipoPag);
                                                tblEspecifico.AddCell(cltotalEsp);
                                                dineroTotalbyCat = dineroTotalbyCat + v.total;
                                                tempIdpro = v.idProducto;
                                                posList = posList + 1;
                                            }
                                            //#################################
                                            //en cada categoria 
                                            //#################################
                                            clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                            ////clidprod = new PdfPCell(new Phrase("", _standardFont));
                                            clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                            clPrecioReal = new PdfPCell(new Phrase("", _standardFont));
                                            clCant = new PdfPCell(new Phrase("Subtotal($)", _standardFont));
                                            //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                            cltotalEsp = new PdfPCell(new Phrase(dineroTotalbyCat.ToString(), _standardFont));
                                            dineroTotalbyCat = 0;

                                            clFechaEsp.Border = 0;
                                            // clidprod.Border = 0;
                                            clnombreprod.Border = 0;
                                            clPrecioReal.Border = 0;
                                            clCant.Border = 1;
                                            cltotalEsp.Border = 1;



                                            tblEspecifico.AddCell(clFechaEsp);
                                            //tblEspecifico.AddCell(clidprod);
                                            tblEspecifico.AddCell(clnombreprod);
                                            tblEspecifico.AddCell(clPrecioReal);
                                            tblEspecifico.AddCell(clCant);
                                            //tblEspecifico.AddCell(clTipoPag);
                                            tblEspecifico.AddCell(cltotalEsp);


                                            /////////////////////////////////////////////
                                            clFechaEsp = new PdfPCell(new Phrase("", _standardFont));
                                            //clidprod = new PdfPCell(new Phrase("", _standardFont));
                                            clnombreprod = new PdfPCell(new Phrase("", _standardFont));
                                            clCant = new PdfPCell(new Phrase("", _standardFont));
                                            clPrecioReal = new PdfPCell(new Phrase("", _standardFont));
                                            //clTipoPag = new PdfPCell(new Phrase("-----------", _standardFont));
                                            cltotalEsp = new PdfPCell(new Phrase("", _standardFont));
                                            clFechaEsp.Border = 0;
                                            //clidprod.Border = 0;
                                            clnombreprod.Border = 0;
                                            clPrecioReal.Border = 0;
                                            clCant.Border = 0;
                                            cltotalEsp.Border = 0;

                                            cltotalEsp.FixedHeight = 30f;
                                            clFechaEsp.FixedHeight = 30f;
                                            // clidprod.FixedHeight = 30f;
                                            clnombreprod.FixedHeight = 30f;
                                            clPrecioReal.FixedHeight = 30f;
                                            clCant.FixedHeight = 30f;
                                            cltotalEsp.FixedHeight = 30f;

                                            tblEspecifico.AddCell(clFechaEsp);
                                            // tblEspecifico.AddCell(clidprod);
                                            tblEspecifico.AddCell(clnombreprod);
                                            tblEspecifico.AddCell(clPrecioReal);
                                            tblEspecifico.AddCell(clCant);
                                            //tblEspecifico.AddCell(clTipoPag);
                                            tblEspecifico.AddCell(cltotalEsp);

                                        }
                                        //#######################################################
                                        //######            DETALLES GENERALES       ############
                                        //#######################################################

                                        PdfPTable tblGenerales = new PdfPTable(3);
                                        //doc.SetMargins(0f, 0f, 0f, 0f);
                                        tblGenerales.HorizontalAlignment = Element.ALIGN_LEFT;
                                        // Configuramos el título de las columnas de la tabla
                                        PdfPCell clFecha = new PdfPCell(new Phrase("Mes", _standardFont));
                                        PdfPCell clCat = new PdfPCell(new Phrase("Categoria", _standardFont));
                                        PdfPCell clSubtotal = new PdfPCell(new Phrase("Subtotal", _standardFont));
                                        tblGenerales.AddCell(clFecha);
                                        tblGenerales.AddCell(clCat);
                                        tblGenerales.AddCell(clSubtotal);

                                        categoriaFacade catFac = new categoriaFacade();


                                        int subtotal = 0;
                                        int total = 0;
                                        int posListG = 0;
                                        //Agrupar por categoria los idproducto

                                        foreach (var x in q)
                                        {
                                            for (int i = 0; i < x.Count; i++)
                                            {
                                                var v = listProductoConCategoria[posListG];


                                                subtotal = subtotal + v.total;
                                                posListG = posListG + 1;
                                            }
                                            total = total + subtotal;
                                            clFecha = new PdfPCell(new Phrase(Convert.ToDateTime(month).ToString("y"), _standardFont));
                                            clCat = new PdfPCell(new Phrase(catFac.getNombreCategoriaById(x.Value.ToString()), _standardFont));
                                            clSubtotal = new PdfPCell(new Phrase(subtotal.ToString(), _standardFont));
                                            tblGenerales.AddCell(clFecha);
                                            tblGenerales.AddCell(clCat);
                                            tblGenerales.AddCell(clSubtotal);
                                            subtotal = 0;
                                        }


                                        clFecha = new PdfPCell(new Phrase("", _standardFont));
                                        clCat = new PdfPCell(new Phrase("Total Costo  Producto Adquiridos", _standardFont));
                                        clSubtotal = new PdfPCell(new Phrase(total.ToString(), _standardFont));

                                        clFecha.Border = 0;
                                        tblGenerales.AddCell(clFecha);
                                        tblGenerales.AddCell(clCat);
                                        tblGenerales.AddCell(clSubtotal);



                                        doc.Add(dgeneral);
                                        doc.Add(Chunk.NEWLINE);
                                        doc.Add(tblGenerales);
                                        doc.Add(Chunk.NEWLINE);
                                        doc.Add(despecifico);
                                        doc.Add(new Chunk("\n"));
                                        doc.Add(tblEspecifico);
                                        doc.Add(new Chunk("\n"));






                                        doc.Close();
                                        writer.Close();




                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.ToString());
                                    }
                                    finally
                                    {
                                        doc.Close();
                                        writer.Close();
                                    }

                                }

                            }
                            System.Diagnostics.Process.Start(exportSaveFileDialog.FileName);
                        }




                    }
                    else
                    {
                        MessageBox.Show("No hay Productos ingresados", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccionar mes para generar reporte", "Magnolia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void calendarReportesPagoXRubros_SelectedDatesChanged(object sender,
       SelectionChangedEventArgs e)
        {

            var calendar = sender as Calendar;
            if (calendarReportesPagoRubro.SelectedDate.HasValue)
            {
                CantidadTotalRubrosyProductos(MesRubroProd);

                DateTime date = calendar.SelectedDate.Value;
                //txtFechaReporte.Text = string.Format("{0}-{1}", year, month);

                //calendarReportesPagoRubro.DisplayMode =  CalendarMode.Year;

            }

        }

        private void calendar1_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
        {
            if (calendarReportesPagoRubro.DisplayMode != CalendarMode.Year)
            {
                calendarReportesPagoRubro.DisplayMode = CalendarMode.Year;
            }
            /*else 
            { 
                calendarReportesPagoRubro.DisplayMode = CalendarMode.Decade;
            }*/
        }

        private void calendar1_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {


            if (txtFechaReporte != null) //Because the calendar may render prior to the textbox
            {
                //year = calendarReportesPagoRubro.DisplayDate.Year.ToString();
                month = calendarReportesPagoRubro.DisplayDate.ToString("y");
                MesRubroProd = calendarReportesPagoRubro.DisplayDate;
                //CantidadTotalRubrosyProductos(MesRubroProd);
                ProductoFacade prodFac = new ProductoFacade();
                categoriaFacade catFac = new categoriaFacade();
                int p = prodFac.getTotalProductosbyMes(MesRubroProd);
                int r = catFac.getTotalCategoriabyMes(MesRubroProd);
                CantidadTotalRubrosyProductos(MesRubroProd);
                //MessageBox.Show(MesRubroProd.Month.ToString()+":rubro:"+r+"-producto:"+p);
                /*if (calendarReportesPagoRubro.DisplayDate.Month < 10)
                {
                    month = "0" + month;
                }*/
                txtFechaReporte.Text = month;
                //txtFechaReporte.Text = string.Format("{0}-{1}", year, month);
                //txtFechaReporte.Text = string.Format("{0}-{1}", year, month);
            }
        }
        private void _Calendar_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        public void setContent(TransLoginToReporte t)
        {
            translogin = t;

        }
        public TransLoginToReporte getContent()
        {
            return translogin;
        }

        private void btnSalirReporte_Click(object sender, RoutedEventArgs e)
        {
            LoginAdmin login = new LoginAdmin();
            login.setLogin(translogin);
            translogin.pageTransitionControl.ShowPage(login);
        }





    }
}
