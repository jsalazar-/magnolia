using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using FirstFloor.ModernUI.App.Modelo;
using System.Data;
namespace FirstFloor.ModernUI.App.Control
{
    class clienteFacade
    {
        ConexionBD getconexion = new ConexionBD();


        public string GuardarClientes(string rut, string nombre, string cantidadDescuento, string deuda, DateTime fechaUltimaCompra, int totalCompras)
        {
            //Boolean guardar = false;
            string i = "";

            try
            {

                string consultinsert = "INSERT INTO cliente(rut,nombre,cantidadDescuento,deuda,fechaUltimaCompra,totalCompras) VALUES(@rut, @nombre,@cantidadDescuento,@deuda,@fechaUltimaCompra,@totalCompras)";

                MySqlCommand comm = new MySqlCommand(consultinsert, getconexion.getConexion());
                comm.Parameters.AddWithValue("@rut", rut);
                comm.Parameters.AddWithValue("@nombre", nombre);
                comm.Parameters.AddWithValue("@cantidadDescuento", cantidadDescuento);
                comm.Parameters.AddWithValue("@deuda", "0");
                comm.Parameters.AddWithValue("@fechaUltimaCompra", fechaUltimaCompra);
                comm.Parameters.AddWithValue("@totalCompras", totalCompras);

                comm.ExecuteNonQuery();
                getconexion.CerrarConexion();

                //guardar = true;
            }
            catch (Exception e)
            {
                //guardar = false;
                i = e.ToString();

            }
            finally {
                getconexion.CerrarConexion();

            }
            return i;

        }
        public List<Cliente> getClientes()
        {
            DataTable dtDatos = new DataTable();
            MySqlDataReader rdr = null;
            List<Cliente> Listcliente = new List<Cliente>();
            
            string consulta = "SELECT*FROM cliente";
            
            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();
            dtDatos.Columns.Add("rut");
            dtDatos.Columns.Add("nombre");
            dtDatos.Columns.Add("cantidadDescuento");
            dtDatos.Columns.Add("deuda");
            dtDatos.Columns.Add("fechaUltimaCompra");
            dtDatos.Columns.Add("totalCompras");

            while (rdr.Read())
            {
                Listcliente.Add(new Cliente(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetDouble(3), rdr.GetDateTime(4),rdr.GetInt32(5)));

            }
            getconexion.CerrarConexion();

            return Listcliente;


        }
        public bool getExisteCliente(string rut)
        {


            string consulta = "SELECT*FROM cliente WHERE rut=\"" + rut + "\"";
            bool res = false;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    if (!read2.GetString(0).Equals(""))
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
        public List<Cliente> getClientesbyRut(string rut)
        {
            DataTable dtDatos = new DataTable();
            MySqlDataReader rdr = null;
            List<Cliente> Listcliente = new List<Cliente>();

            string consulta = "SELECT*FROM cliente WHERE rut LIKE \'%"+rut+"%\'";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();
            dtDatos.Columns.Add("rut");
            dtDatos.Columns.Add("nombre");
            dtDatos.Columns.Add("cantidadDescuento");
            dtDatos.Columns.Add("deuda");
            dtDatos.Columns.Add("fechaUltimaCompra");
            dtDatos.Columns.Add("totalCompras");

            while (rdr.Read())
            {
                Listcliente.Add(new Cliente(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetDouble(3), rdr.GetDateTime(4), rdr.GetInt32(5)));

            }
            getconexion.CerrarConexion();

            return Listcliente;


        }
        public List<Cliente> getClientesbyNombre(string nombre)
        {
            DataTable dtDatos = new DataTable();
            MySqlDataReader rdr = null;
            List<Cliente> Listcliente = new List<Cliente>();

            string consulta = "SELECT*FROM cliente WHERE nombre LIKE \'%"+nombre+"%\'";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();
            dtDatos.Columns.Add("rut");
            dtDatos.Columns.Add("nombre");
            dtDatos.Columns.Add("cantidadDescuento");
            dtDatos.Columns.Add("deuda");
            dtDatos.Columns.Add("fechaUltimaCompra");
            dtDatos.Columns.Add("totalCompras");

            while (rdr.Read())
            {
                Listcliente.Add(new Cliente(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetDouble(3), rdr.GetDateTime(4), rdr.GetInt32(5)));

            }
            getconexion.CerrarConexion();

            return Listcliente;


        }
        public string borrarClienteByRut(string rut)
        {
            string borrar = "";
            try
            {
                 

                MySqlCommand cmdCategoria = new MySqlCommand("DELETE FROM cliente WHERE rut =\"" + rut + "\"", getconexion.getConexion());
                cmdCategoria.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                borrar = e.ToString();
            }
            return borrar;
        
        

        }
        public string borrarAllCliente()
        {
            string borrar = "";
            try
            {
                categoriaFacade ctf = new categoriaFacade();


                MySqlCommand cmdCategoria = new MySqlCommand("TRUNCATE TABLE cliente ", getconexion.getConexion());
                cmdCategoria.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                borrar = e.ToString();
            }
            return borrar;


        }
        public string actualizarCliente(string rut, string nombre, string cantidadDescuento, double deuda) 
        {
            string res = "";
            try
            {
                MySqlCommand cmdCliente = new MySqlCommand("UPDATE cliente set nombre=\"" + nombre + "\", cantidadDescuento=\"" + cantidadDescuento + "\", deuda=\"" + deuda + "\" WHERE rut =\"" + rut + "\";", getconexion.getConexion());

                MySqlDataReader MyReader2 = cmdCliente.ExecuteReader();
                
            }
            catch (Exception e)
            {
                res = e.ToString();
                getconexion.CerrarConexion();
            }
            finally {
                getconexion.CerrarConexion();
            }


            return res;
        }
        public Cliente getAllClientesbyRut(string rut)
        {
             
            MySqlDataReader rdr = null;
             Cliente Listcliente=new Cliente();

            string consulta = "SELECT*FROM cliente  WHERE rut=\"" + rut+"\"" ;

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();
            

            while (rdr.Read())
            {
                Listcliente=new Cliente(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetDouble(3), rdr.GetDateTime(4), rdr.GetInt32(5));
                

            }
            getconexion.CerrarConexion();

            return Listcliente;


        }
        public string actualizar_DFT_Cliente(string rut, double deuda, DateTime fechaUltimaCompra)
        {
            string res = "";
            try
            {
                Cliente cl = getAllClientesbyRut(rut);
                double adeuda =Convert.ToDouble(cl.deuda)+Convert.ToDouble(deuda);
                int atotalcompra = Convert.ToInt32(cl.totalCompras) + 1;

                MySqlCommand cmdCliente = new MySqlCommand("UPDATE cliente set deuda=@deuda, fechaUltimaCompra   =@fecha, totalCompras=\""+atotalcompra+"\" WHERE rut =\"" + rut + "\";", getconexion.getConexion());
                cmdCliente.Parameters.AddWithValue("@deuda", deuda);
                cmdCliente.Parameters.AddWithValue("@fecha", fechaUltimaCompra);
                MySqlDataReader MyReader2 = cmdCliente.ExecuteReader();
                
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

        public int getTotalClientes()
        {
            

            string consulta = "SELECT*FROM cliente";
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
        public double getDeudaCliente(string rut)
        {


            string consulta = "SELECT deuda FROM cliente WHERE rut=\"" + rut + "\"";
            int res = 0;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    res = Convert.ToInt32(read2.GetDouble(0));

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

    }
}
