using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using FirstFloor.ModernUI.App.Modelo;


namespace FirstFloor.ModernUI.App.Control
{
    class ProductoFacade
    {
        ConexionBD getconexion = new ConexionBD();


        public string GuardarProducto(string idProducto, string nombre, string stock, string precioReal, string precio, string nombreCategoria, DateTime fecha, int idGenerado)
        {
            //Boolean guardar = false;
            string i = "";

            try
            {

                categoriaFacade ctf = new categoriaFacade();
                string idCategoria = ctf.getCategoriaByNombre(nombreCategoria);

                string consultinsert = "INSERT INTO producto(idProducto,nombre,stock,precioReal,precio,idCategoria,fecha,idGenerado) VALUES(@idProducto, @nombre,@stock,@precioReal,@precio,@idCategoria,@fecha,@idGenerado)";

                //  string consultinsert  = "INSERT INTO producto(idProducto,nombre,stock,precio,idCategoria) VALUES("+idProducto+","+nombre+","+stock+","+precio+","+idCategoria+")";
                MySqlCommand comm = new MySqlCommand(consultinsert, getconexion.getConexion());
                comm.Parameters.AddWithValue("@idProducto", idProducto);
                comm.Parameters.AddWithValue("@nombre", nombre);
                comm.Parameters.AddWithValue("@stock", stock);
                comm.Parameters.AddWithValue("@precioReal", precioReal);
                comm.Parameters.AddWithValue("@precio", precio);
                comm.Parameters.AddWithValue("@idCategoria", idCategoria);
                comm.Parameters.AddWithValue("@fecha", fecha);
                comm.Parameters.AddWithValue("@idGenerado", idGenerado);

                comm.ExecuteNonQuery();
                getconexion.CerrarConexion();

            }
            catch (Exception e)
            {
                //guardar = false;
                i = e.ToString();

            }
            return i;

        }


        public List<Producto> getProductobyNombre(string nombre)
        {

            MySqlDataReader rdr = null;
            List<Producto> ListProducto = new List<Producto>();

            string consulta = "SELECT*FROM producto WHERE nombre LIKE \'%" + nombre + "%\'";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {
                ListProducto.Add(new Producto(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDateTime(6)));

            }
            getconexion.CerrarConexion();

            return ListProducto;


        }
        public List<Producto> getProductobyCodigo(string idcodigo)
        {

            MySqlDataReader rdr = null;
            List<Producto> ListProducto = new List<Producto>();

            string consulta = "SELECT*FROM producto WHERE idProducto LIKE \'%" + idcodigo + "%\'";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {
                ListProducto.Add(new Producto(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDateTime(6)));

            }
            getconexion.CerrarConexion();

            return ListProducto;


        }
        public bool getExisteProductoBynombreYidCat(string nombreProducto, string nombreCategoria)
        {
            categoriaFacade catF = new categoriaFacade();
            string idCategoria = catF.getCategoriaByNombre(nombreCategoria);

            string consulta = "SELECT*FROM producto WHERE nombre=\"" + nombreProducto + "\"AND idCategoria=\"" + idCategoria + "\";";
            bool res = false;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    if (!read2.Equals(""))
                    {
                        res = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);

            }
            finally
            {
                getconexion.CerrarConexion();
            }

            return res;

        }
        public bool getExisteProductoByidProd(string idProd)
        {
            categoriaFacade catF = new categoriaFacade();

            string consulta = "SELECT*FROM producto WHERE idProducto=\"" + idProd + "\"";
            bool res = false;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    if (!read2.Equals(""))
                    {
                        res = true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);

            }
            finally
            {
                getconexion.CerrarConexion();
            }

            return res;

        }
        public string getStockProductoByidProd(string idProd)
        {
            categoriaFacade catF = new categoriaFacade();

            string consulta = "SELECT stock FROM producto WHERE idProducto=\"" + idProd + "\"";
            string res = "";

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {

                    res = read2.GetInt32(0).ToString();

                }
            }
            catch (Exception e)
            {
                res = e.ToString();

            }
            finally
            {
                getconexion.CerrarConexion();
            }

            return res;

        }
        public int getTotalProductosBynombreCat(string nombreCategoria)
        {
            categoriaFacade catF = new categoriaFacade();
            string idCategoria = catF.getCategoriaByNombre(nombreCategoria);

            string consulta = "SELECT*FROM producto WHERE idCategoria=\"" + idCategoria + "\";";
            int res = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    if (!read2.Equals(""))
                    {
                        res = res + 1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);

            }
            finally
            {
                getconexion.CerrarConexion();
            }

            return res;

        }

        public int getIdGenerado(string idProducto)
        {

            string consulta = "SELECT idGenerado FROM producto WHERE idProducto=\"" + idProducto + "\";";
            int res = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    res = read2.GetInt32(0);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);

            }
            finally
            {
                getconexion.CerrarConexion();
            }

            return res;

        }

        public int getPrecioCompraProducto(string idProducto)
        {
            categoriaFacade catF = new categoriaFacade();

            string consulta = "SELECT precioReal FROM producto WHERE idProducto=\"" + idProducto + "\"";
            int res = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    res = Convert.ToInt32(read2.GetString(0));
                }
            }
            catch (Exception e)
            {
                Console.Write(e);

            }
            finally
            {
                getconexion.CerrarConexion();
            }

            return res;

        }
        public int getTotalProductos()
        {
            categoriaFacade catF = new categoriaFacade();

            string consulta = "SELECT*FROM producto";
            int res = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    if (!read2.Equals(""))
                    {
                        res = res + 1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);

            }
            finally
            {
                getconexion.CerrarConexion();
            }

            return res;

        }
        public int getTotalProductosbyMes(DateTime mes)
        {
            categoriaFacade catF = new categoriaFacade();

            string consulta = "SELECT*FROM producto where month(fecha)=" + mes.Month;
            int res = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    if (!read2.Equals(""))
                    {
                        res = res + 1;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e);

            }
            finally
            {
                getconexion.CerrarConexion();
            }

            return res;

        }

        public List<Producto> getProductos()
        {
            MySqlDataReader rdr = null;
            List<Producto> producto = new List<Producto>();

            string consulta = "SELECT*FROM producto";
            //MySqlDataAdapter mdaDatos = new MySqlDataAdapter("SELECT*FROM producto", conexion.getConexion());
            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                producto.Add(new Producto(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDateTime(6)));

            }
            getconexion.CerrarConexion();

            return producto;


        }
        public Producto getProductosByID(string idProducto)
        {

            MySqlDataReader rdr = null;
            Producto producto = new Producto();


            string consulta = "SELECT*FROM producto WHERE idProducto=\"" + idProducto + "\"";
            //MySqlDataAdapter mdaDatos = new MySqlDataAdapter("SELECT*FROM producto", conexion.getConexion());
            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                // producto.Add(new Producto(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4)));
                producto.idProducto = rdr.GetString(0);
                producto.nombre = rdr.GetString(1);
                producto.stock = rdr.GetString(2);
                producto.precio = rdr.GetString(4);
                producto.idCategoria = rdr.GetInt32(5);

            }
            getconexion.CerrarConexion();

            return producto;


        }
        public string getIdProductosBynombreCategoria(string nombreCategoria)
        {

            MySqlDataReader rdr = null;
            string producto = "";
            categoriaFacade ctf = new categoriaFacade();
            string idCategoria = ctf.getCategoriaByNombre(nombreCategoria);

            string consulta = "SELECT idProducto FROM producto WHERE idCategoria=\"" + idCategoria + "\"";
            //MySqlDataAdapter mdaDatos = new MySqlDataAdapter("SELECT*FROM producto", conexion.getConexion());
            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {
                producto = rdr.GetString(0);


            }
            getconexion.CerrarConexion();

            return producto;
        }
        public List<Producto> getProductosBynombreCategoria(string nombreCategoria)
        {
            DataTable dtDatos = new DataTable();
            MySqlDataReader rdr = null;
            List<Producto> producto = new List<Producto>();
            categoriaFacade ctf = new categoriaFacade();
            string idCategoria = ctf.getCategoriaByNombre(nombreCategoria);

            string consulta = "SELECT*FROM producto WHERE idCategoria=\"" + idCategoria + "\"";
            //MySqlDataAdapter mdaDatos = new MySqlDataAdapter("SELECT*FROM producto", conexion.getConexion());
            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();
            dtDatos.Columns.Add("Nombre");
            dtDatos.Columns.Add("Stock");
            dtDatos.Columns.Add("precioReal");
            dtDatos.Columns.Add("Precio");
            dtDatos.Columns.Add("Categoria");

            while (rdr.Read())
            {
                producto.Add(new Producto(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDateTime(6)));

            }
            getconexion.CerrarConexion();

            return producto;
        }
        public List<Producto> getProductosBynombre(string nombre, string nombreCategoria)
        {
            DataTable dtDatos = new DataTable();
            MySqlDataReader rdr = null;
            List<Producto> producto = new List<Producto>();
            categoriaFacade ctf = new categoriaFacade();
            string idCategoria = ctf.getCategoriaByNombre(nombreCategoria);
            string consulta = "SELECT*FROM producto WHERE nombre=\"" + nombre + "\" AND idCategoria=\"" + idCategoria + "\"";
            //MySqlDataAdapter mdaDatos = new MySqlDataAdapter("SELECT*FROM producto", conexion.getConexion());
            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();
            dtDatos.Columns.Add("Nombre");
            dtDatos.Columns.Add("Stock");
            dtDatos.Columns.Add("precioReal");
            dtDatos.Columns.Add("Precio");
            dtDatos.Columns.Add("Categoria");

            while (rdr.Read())
            {
                producto.Add(new Producto(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4), rdr.GetInt32(5), rdr.GetDateTime(6)));

            }
            getconexion.CerrarConexion();

            return producto;
        }

        public Boolean borrarProductoBynombreCategoria(string nombreCategoria)
        {
            Boolean borrar = false;
            try
            {
                categoriaFacade ctf = new categoriaFacade();
                string idCategoria = ctf.getCategoriaByNombre(nombreCategoria);


                MySqlDataAdapter adapter = new MySqlDataAdapter();



                MySqlCommand cmdCategoria = new MySqlCommand("DELETE FROM producto WHERE idCategoria =\"" + idCategoria + "\"", getconexion.getConexion());
                cmdCategoria.ExecuteNonQuery();

                borrar = true;
            }
            catch (Exception e)
            {
                borrar = false;
            }
            return borrar;


        }

        public Boolean borrarProductoYnombreCategoria(string nombreCategoria)
        {
            Boolean borrar = false;
            try
            {
                categoriaFacade ctf = new categoriaFacade();
                string idCategoria = ctf.getCategoriaByNombre(nombreCategoria);
                ctf.borrarCategoriaBynombre(nombreCategoria);

                MySqlDataAdapter adapter = new MySqlDataAdapter();



                MySqlCommand cmdCategoria = new MySqlCommand("DELETE FROM producto WHERE idCategoria =\"" + idCategoria + "\"", getconexion.getConexion());
                cmdCategoria.ExecuteNonQuery();

                borrar = true;
            }
            catch (Exception e)
            {
                borrar = false;
            }
            return borrar;


        }
        public string borrarProductoByid(string idproducto)
        {
            string borrar = "";
            try
            {
                categoriaFacade ctf = new categoriaFacade();


                MySqlCommand cmdCategoria = new MySqlCommand("DELETE FROM producto WHERE idProducto =\"" + idproducto + "\"", getconexion.getConexion());
                cmdCategoria.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                borrar = e.ToString();
            }
            return borrar;


        }
        public string borrarAllProducto()
        {
            string borrar = "";
            try
            {
                categoriaFacade ctf = new categoriaFacade();


                MySqlCommand cmdCategoria = new MySqlCommand("TRUNCATE TABLE producto ", getconexion.getConexion());
                cmdCategoria.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                borrar = e.ToString();
            }
            return borrar;


        }
        public string getnombreProdbyidProd(string idProducto)
        {
            categoriaFacade ctf = new categoriaFacade();

            string consulta = "SELECT nombre FROM producto WHERE idProducto=\"" + idProducto + "\"";


            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            MySqlDataReader read2 = cmd.ExecuteReader();
            string res = "";
            while (read2.Read())
            {
                res = read2.GetString(0);
            }
            getconexion.CerrarConexion();

            return res;
        }
        public string getIdCatbyidProd(string idProducto)
        {
            categoriaFacade ctf = new categoriaFacade();

            string consulta = "SELECT idCategoria FROM producto WHERE idProducto=\"" + idProducto + "\"";


            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            MySqlDataReader read2 = cmd.ExecuteReader();
            string res = "";
            while (read2.Read())
            {
                res = read2.GetInt32(0).ToString();
            }
            getconexion.CerrarConexion();

            //return ctf.getNombreCategoriaById(res);
            return res;
        }
        //Devuelve ultimo registro de producto asociado a categoria
        public string getMayorIdProdbyNombCat(string nombreCategoria)
        {
            categoriaFacade ctf = new categoriaFacade();
            string idCategoria = ctf.getCategoriaByNombre(nombreCategoria);
            string consulta = "SELECT idProducto FROM producto WHERE idCategoria=\"" + idCategoria + "\"";


            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            MySqlDataReader read2 = cmd.ExecuteReader();
            long mayorid = 0;
            while (read2.Read())
            {
                long idCat = read2.GetInt64(0);
                if (idCat > mayorid)
                {
                    mayorid = idCat;
                }
            }
            getconexion.CerrarConexion();

            return mayorid.ToString();
        }


        public string ActualizarProducto(string idProdAnt, string idProdNuevo, string nombre, string stock, string precioReal, string precio, string nombreCategoria, DateTime fecha)
        {
            //Boolean guardar = false;
            string res = "";

            try
            {

                categoriaFacade ctf = new categoriaFacade();
                string idCategoria = ctf.getCategoriaByNombre(nombreCategoria);

                string consultinsert = "UPDATE  producto SET idProducto=@idProdNuevo,nombre=@nombre,stock=@stock,precioReal=@precioReal,precio=@precio,idCategoria=@idCategoria,fecha=@fecha WHERE idProducto=@idProdAnt";


                MySqlCommand comm = new MySqlCommand(consultinsert, getconexion.getConexion());
                comm.Parameters.AddWithValue("@idProdNuevo", idProdNuevo);
                comm.Parameters.AddWithValue("@nombre", nombre);
                comm.Parameters.AddWithValue("@stock", stock);
                comm.Parameters.AddWithValue("@PrecioReal", precioReal);
                comm.Parameters.AddWithValue("@precio", precio);
                comm.Parameters.AddWithValue("@idCategoria", idCategoria);
                comm.Parameters.AddWithValue("@fecha", fecha);
                comm.Parameters.AddWithValue("@idProdAnt", idProdAnt);

                MySqlDataReader MyReader2 = comm.ExecuteReader();

            }
            catch (Exception e)
            {
                res = e.ToString();
                getconexion.CerrarConexion();
            }
            finally
            {
                getconexion.CerrarConexion();
            }


            return res;

        }
        public string actualizarStockProductoDevolucion(string idProducto, int stock)
        {
            //Boolean guardar = false;
            string res = "";

            try
            {

                int stockAnterior = Convert.ToInt32(getStockProductoByidProd(idProducto));
                int stockActualizado = stockAnterior + Convert.ToInt32(stock);
                string consultinsert = "UPDATE  producto SET stock=@stock WHERE idProducto=@idProducto";


                MySqlCommand comm = new MySqlCommand(consultinsert, getconexion.getConexion());
                comm.Parameters.AddWithValue("@idProducto", idProducto);
                comm.Parameters.AddWithValue("@stock", stockActualizado);


                MySqlDataReader MyReader2 = comm.ExecuteReader();

            }
            catch (Exception e)
            {
                res = e.ToString();
                getconexion.CerrarConexion();
            }
            finally
            {
                getconexion.CerrarConexion();
            }


            return res;

        }
        public string actualizarStockProducto(string idProducto, string stock)
        {
            //Boolean guardar = false;
            string res = "";

            try
            {

                int stockAnterior = Convert.ToInt32(getStockProductoByidProd(idProducto));
                int stockActualizado = stockAnterior - Convert.ToInt32(stock);
                string consultinsert = "UPDATE  producto SET stock=@stock WHERE idProducto=@idProducto";


                MySqlCommand comm = new MySqlCommand(consultinsert, getconexion.getConexion());
                comm.Parameters.AddWithValue("@idProducto", idProducto);
                comm.Parameters.AddWithValue("@stock", stockActualizado);


                MySqlDataReader MyReader2 = comm.ExecuteReader();

            }
            catch (Exception e)
            {
                res = e.ToString();
                getconexion.CerrarConexion();
            }
            finally
            {
                getconexion.CerrarConexion();
            }


            return res;

        }

        //##############################################
        //#########         MES       ##################
        //##############################################
        public List<Producto> getALLProductosbyFechaMes(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<Producto> ListProductos = new List<Producto>();

            string consulta = "SELECT fecha,idProducto,nombre,precioReal,stock,(precioReal*stock) as total,idCategoria FROM db.producto WHERE MONTH(fecha) =" + fecha.Month + " AND year(fecha)=" + fecha.Year;

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                ListProductos.Add(new Producto(rdr.GetDateTime(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3),
                    rdr.GetInt32(4).ToString(), rdr.GetInt32(5), rdr.GetInt32(6)));

            }
            getconexion.CerrarConexion();

            return ListProductos;

        }




    }
}
