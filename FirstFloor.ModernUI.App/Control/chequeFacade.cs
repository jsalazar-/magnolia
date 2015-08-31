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
    class chequeFacade
    {
        ConexionBD getconexion = new ConexionBD();

        public string GuardarCheque(Cheque ch)
        {
            //Boolean guardar = false;
            string i = "";

            try
            {

                string consultinsert = "INSERT INTO cheque(rut, nombre, nombreBanco,fechaEmision, fechaVenta,fechaExpiracion,monto) VALUES(@rut, @nombre, @nombreBanco,@fechaEmision, @fechaExpiracion,@fechaVenta,@monto)";

                MySqlCommand comm = new MySqlCommand(consultinsert, getconexion.getConexion());
                comm.Parameters.AddWithValue("@rut", ch.rut);
                comm.Parameters.AddWithValue("@nombre", ch.nombre);
                comm.Parameters.AddWithValue("@nombreBanco", ch.nombreBanco);
                comm.Parameters.AddWithValue("@fechaEmision", ch.fechaemision);
                comm.Parameters.AddWithValue("@fechaExpiracion", ch.fechaexpiracion);
                 comm.Parameters.AddWithValue("@fechaVenta", ch.fechaVenta);
                comm.Parameters.AddWithValue("@monto", ch.monto);
                



                comm.ExecuteNonQuery();
                getconexion.CerrarConexion();

                //guardar = true;
            }
            catch (Exception e)
            {
                //guardar = false;
                i = e.Message.ToString();

            }
            finally
            {
                getconexion.CerrarConexion();

            }
            return i;

        }
    }
}
