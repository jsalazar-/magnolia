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
    class vendedorFacade
    {
        ConexionBD getconexion = new ConexionBD();




        public string GuardarVendedor(string rut, string nombre, string fechaIngresoTrabajar, string fechaUltimoAcceso, int totalVentas, int tipo, string contrasena)
        {
            //Boolean guardar = false;
            string i = "";

            try
            {

                string consultinsert = "INSERT INTO vendedor(rut, nombre, fechaIngresoTrabajar,fechaUltimoAcceso, totalVentas,tipo,contrasena) VALUES(@rut, @nombre, @fechaIngresoTrabajar,@fechaUltimoAcceso, @totalVentas,@tipo,SHA(@contrasena))";

                MySqlCommand comm = new MySqlCommand(consultinsert, getconexion.getConexion());
                comm.Parameters.AddWithValue("@rut", rut);
                comm.Parameters.AddWithValue("@nombre", nombre);
                comm.Parameters.AddWithValue("@fechaIngresoTrabajar", fechaIngresoTrabajar);
                comm.Parameters.AddWithValue("@fechaUltimoAcceso", fechaUltimoAcceso);
                comm.Parameters.AddWithValue("@totalVentas", totalVentas);
                comm.Parameters.AddWithValue("@tipo", tipo);
                comm.Parameters.AddWithValue("@contrasena", contrasena);


                comm.ExecuteNonQuery();
                getconexion.CerrarConexion();

                //guardar = true;
            }
            catch (Exception e)
            {
                //guardar = false;
                i = e.ToString();

            }
            finally
            {
                getconexion.CerrarConexion();

            }
            return i;

        }
        public List<Vendedor> getVendedor()
        {

            MySqlDataReader rdr = null;
            List<Vendedor> Listcliente = new List<Vendedor>();

            string consulta = "SELECT*FROM vendedor";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();



            while (rdr.Read())
            {
                Listcliente.Add(new Vendedor(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetString(6)));

            }
            getconexion.CerrarConexion();

            return Listcliente;


        }
        public Vendedor getVendedorbyRut(string rut)
        {

            MySqlDataReader rdr = null;
            Vendedor cliente = null;

            string consulta = "SELECT*FROM vendedor where rut=\"" + rut + "\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {
                cliente = new Vendedor(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetString(6));

            }
            getconexion.CerrarConexion();

            return cliente;


        }
        public string getpassbyRut(string rut)
        {

            MySqlDataReader rdr = null;
            string pass = "";

            string consulta = "SELECT contrasena FROM vendedor where rut=\"" + rut + "\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();


            while (rdr.Read())
            {
                pass = rdr.GetString(0);

            }
            getconexion.CerrarConexion();

            return pass;


        }
        public string borrarVendedorByRut(string rut)
        {
            string borrar = "";
            try
            {


                MySqlCommand cmdCategoria = new MySqlCommand("DELETE FROM vendedor WHERE rut =\"" + rut + "\"", getconexion.getConexion());
                cmdCategoria.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                borrar = e.ToString();
            }
            return borrar;



        }
        public string borrarAllVendedor()
        {
            string borrar = "";
            try
            {
                categoriaFacade ctf = new categoriaFacade();


                MySqlCommand cmdCategoria = new MySqlCommand("TRUNCATE TABLE vendedor ", getconexion.getConexion());
                cmdCategoria.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                borrar = e.ToString();
            }
            return borrar;


        }
        public string actualizarVendedor(string rut, string nombre, string fechaIngresoTrabajar)
        {
            string res = "";
            try
            {
                MySqlCommand cmdCliente = new MySqlCommand("UPDATE vendedor set nombre=\"" + nombre + "\", fechaIngresoTrabajar=\"" + fechaIngresoTrabajar + "\" WHERE rut =\"" + rut + "\";", getconexion.getConexion());

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
        public string actualizarAdmin(string rut, string nombre, string contrasena)
        {
            string res = "";
            try
            {
                MySqlCommand cmdCliente = new MySqlCommand("UPDATE vendedor set nombre=\"" + nombre + "\", contrasena=SHA(@contrasena) WHERE rut =\"" + rut + "\";", getconexion.getConexion());
                cmdCliente.Parameters.AddWithValue("@contrasena", contrasena);
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
        public int getTotalVendedor()
        {


            string consulta = "SELECT*FROM vendedor";
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

        public bool getExistAdmin()
        {


            string consulta = "SELECT tipo FROM vendedor";
            bool res = false;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    if (read2.GetInt32(0) == 1)
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
        public bool getExistAdminByRut(string rut)
        {


            string consulta = "SELECT tipo FROM vendedor where rut=\"" + rut + "\"";
            bool res = false;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    if (read2.GetInt32(0) == 1)
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
        public string getNombreAdminByRut(string rut)
        {


            string consulta = "SELECT nombre FROM vendedor where rut=\"" + rut + "\"";
            string res = "";

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {

                    res = read2.GetString(0);

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
        public bool getExisteVendedor(string rut)
        {


            string consulta = "SELECT*FROM vendedor WHERE rut=\"" + rut + "\"";
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
        public string getRutAdmin()
        {


            string consulta = "SELECT rut, tipo FROM vendedor";
            string res = "";

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    if (read2.GetInt32(1) == 1)
                    {
                        res = read2.GetString(0);
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
        public bool getVerificarsiesAdmin(string rut)
        {


            string consulta = "SELECT tipo FROM vendedor WHERE rut=\"" + rut + "\"";
            bool res = false;

            try
            {
                MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
                MySqlDataReader read2 = cmd.ExecuteReader();
                while (read2.Read())
                {
                    if (read2.GetInt32(0) == 1)
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

        public string actualizarUltimaFechaIngrVend(string rut, string Ultimafecha)
        {
            string res = "";
            try
            {
                MySqlCommand cmdCliente = new MySqlCommand("UPDATE vendedor set  fechaUltimoAcceso=\"" + Ultimafecha + "\" WHERE rut =\"" + rut + "\";", getconexion.getConexion());

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

        public int getTotalVentasVendedor(string rut)
        {


            string consulta = "SELECT totalVentas FROM vendedor WHERE rut=\"" + rut + "\"";
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
        public string actualizarVentasVend(string rut)
        {
            string res = "";
            try
            {
                int v = getTotalVentasVendedor(rut);
                int tventasAct = v + 1;
                MySqlCommand cmdCliente = new MySqlCommand("UPDATE vendedor set totalVentas=" + tventasAct + " WHERE rut =\"" + rut + "\";", getconexion.getConexion());

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


    }

}
