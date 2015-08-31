using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using FirstFloor.ModernUI.App.Modelo;

namespace FirstFloor.ModernUI.App.Control
{
    class categoriaFacade
    {
        ConexionBD getconexion = new ConexionBD();

        public string GuardarCategoria(string nombreCategoria)
        {
            string res = "";
            try
            {
                DateTime fechaactual = DateTime.Now.Date;
                MySqlCommand comm = getconexion.getConexion().CreateCommand();
                comm.CommandText = "INSERT INTO categoria(idCategoria,nombreCategoria,fecha) VALUES(@idCategoria, @nombreCategoria,@fecha)";

                long id = comm.LastInsertedId;
                comm.Parameters.AddWithValue("@idCategoria", id);
                comm.Parameters.AddWithValue("@nombreCategoria", nombreCategoria);
                comm.Parameters.AddWithValue("@fecha", fechaactual);
                comm.ExecuteNonQuery();
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
        public List<Categoria> getCategoria()
        {
            string consulta = "SELECT*FROM categoria";
            List<Categoria> listaCategoria = new List<Categoria>();

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            MySqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                listaCategoria.Add(new Categoria(read.GetInt32(0), read.GetString(1),read.GetDateTime(2)));
            }
            getconexion.CerrarConexion();

            return listaCategoria;

        }

        public List<Categoria> getCategoriaConProductosParaImprimirCodigos()
        {

            int totalCat = getTotalCategoria();
            ProductoFacade prodFac = new ProductoFacade();
            
            string consulta = "SELECT*FROM categoria";
            List<Categoria> listaCategoria = new List<Categoria>();

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            MySqlDataReader read = cmd.ExecuteReader();

            while (read.Read())
            {

                if (prodFac.getProductosBynombreCategoria(read.GetString(1)).Count > 0)
                {
                    listaCategoria.Add(new Categoria(read.GetInt32(0), read.GetString(1), read.GetDateTime(2)));
                }
            }
            getconexion.CerrarConexion();

            return listaCategoria;

        }
        public int getTotalCategoria()
        {
            categoriaFacade catF = new categoriaFacade();

            string consulta = "SELECT*FROM categoria";
            int res = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                        res = res + 1;
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


        public int getTotalCategoriabyMes(DateTime fecha )
        {
            categoriaFacade catF = new categoriaFacade();

            string consulta = "SELECT*FROM categoria where month(fecha)="+fecha.Month;
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
        public bool getExisteCategoria(string nombre)
        {
            string consulta = "SELECT*FROM categoria WHERE nombreCategoria=\"" + nombre + "\";";
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
        
        public string getCategoriaById(string id)
        {
            string consulta = "SELECT nombreCategoria FROM categoria WHERE idCategoria=" + id;
            string nombreCat = "";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            MySqlDataReader read1 = cmd.ExecuteReader();
            while (read1.Read())
            {
                nombreCat = read1.GetString(0);
            }
            getconexion.CerrarConexion();

            return nombreCat;

        }
        
            public string getNombreCategoriaById(string id)
        {
            string consulta = "SELECT nombreCategoria FROM categoria WHERE idCategoria=" + id;
            string nombreCat = "";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            MySqlDataReader read1 = cmd.ExecuteReader();
            while (read1.Read())
            {
                nombreCat = read1.GetString(0);
            }
            getconexion.CerrarConexion();

            return nombreCat;

        }
        public string getCategoriaByNombre(string nombre)
        {
            string consulta = "SELECT idCategoria FROM categoria WHERE nombreCategoria=\"" + nombre + "\";";
            string idCat = "";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            MySqlDataReader read2 = cmd.ExecuteReader();
            while (read2.Read())
            {
                idCat = read2.GetString(0);
            }
            getconexion.CerrarConexion();

            return idCat;

        }

        public Boolean borrarCategoriaBynombre(string nombreCategoria)
        {
            Boolean borrar = false;
            try
            {
                //Borrar datos asociados a categoria 


                ProductoFacade prodf = new ProductoFacade();
                prodf.borrarProductoBynombreCategoria(nombreCategoria);

                categoriaFacade ctf = new categoriaFacade();
                string idCategoria = ctf.getCategoriaByNombre(nombreCategoria);




                MySqlCommand cmdCategoria = new MySqlCommand("DELETE FROM categoria WHERE idCategoria =@idcategoria", getconexion.getConexion());
                cmdCategoria.Parameters.AddWithValue("@idCategoria", idCategoria);
                cmdCategoria.ExecuteNonQuery();
                borrar = true;

            }
            catch (Exception e)
            {
                borrar = false;

            }
            return borrar;

        }
        public string actualizarCategoria(string nombreCategoriaN, string nombreCategoria)
        {
            string res = "";
            string consulta = "";
            try
            {
                DateTime fechaactual = DateTime.Now.Date;
                categoriaFacade ctf = new categoriaFacade();
                string idCategoria = ctf.getCategoriaByNombre(nombreCategoria);
                consulta = "UPDATE categoria set nombreCategoria=\"" + nombreCategoriaN + "\"" + ",fecha=@fecha WHERE idCategoria =\"" + idCategoria + "\";";
                MySqlCommand cmdCategoria = new MySqlCommand(consulta, getconexion.getConexion());
                cmdCategoria.Parameters.AddWithValue("@fecha", fechaactual);
                MySqlDataReader MyReader2 = cmdCategoria.ExecuteReader();

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


    }

}
