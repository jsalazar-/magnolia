using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
namespace FirstFloor.ModernUI.App.Control
{
    class ConexionBD
    {
        static string connectionString = "server=localhost; user id=root; password=1234; database=db";
        MySqlConnection conexion = new MySqlConnection(connectionString);



        public  MySqlConnection getConexion()
        {
            try
            {
                conexion.Open();
                

            }
            catch (Exception e)
            {


            }
            return conexion;
        }
        public Boolean conectado()
        {
            Boolean exito=false;
            try
            {
                conexion.Open();
                exito = true;


            }
            catch (Exception e)
            {
                Console.Write(e.ToString());

            }

            return exito;
        }
        public MySqlConnection CerrarConexion() {
            try
            {
                conexion.Close();
            }
            catch (Exception e) {
                Console.Write(e);
            
            }
            return conexion;
        
        }
    }
}