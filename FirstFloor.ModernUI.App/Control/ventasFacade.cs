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
    class ventasFacade
    {
        ConexionBD getconexion = new ConexionBD();

        public string GuardarVentas(List<MVentas> listVentas)
        {
            //Boolean guardar = false;
            string i = "";

            try
            {
                /*string c="Select*from venta";
                MySqlCommand comm1 = new MySqlCommand(c, getconexion.getConexion());
                long id = comm1.LastInsertedId;
                id = id + 1;*/

                //verificar si es primera venta para luego agregar un id igual para venta en temp
                ventasFacade v = new ventasFacade();
                if (v.getVentastotales() == 0)
                {//primer venta 

                    foreach (var vts in listVentas)
                    {
                        string consultinsert = "INSERT INTO venta(idVenta,idProducto, rutCliente, rutVendedor,cantidad, total,fecha,tipoVenta) VALUES (@idVenta,@idProducto, @rutCliente, @rutVendedor,@cantidad, @total,@fecha,@tipoVenta)";
                        MySqlCommand comm = new MySqlCommand(consultinsert, getconexion.getConexion());
                        comm.Parameters.AddWithValue("@idVenta", 0);
                        comm.Parameters.AddWithValue("@idProducto", vts.idProducto);
                        comm.Parameters.AddWithValue("@rutCliente", vts.rutCliente);
                        comm.Parameters.AddWithValue("@rutVendedor", vts.rutVendedor);
                        comm.Parameters.AddWithValue("@cantidad", vts.cantidad);
                        comm.Parameters.AddWithValue("@total", vts.total);
                        comm.Parameters.AddWithValue("@fecha", vts.fecha);
                        comm.Parameters.AddWithValue("@tipoVenta", vts.tipoVenta);
                        comm.ExecuteNonQuery();


                    }
                }
                else
                {//+1 en ventas obtener el ultimo id y sumar +1 para un unico id de ventas en temp
                    ventasFacade vt = new ventasFacade();
                    int idultimo = vt.getUltimoIngresadoenVentas();
                    idultimo = idultimo + 1;
                    foreach (var vts in listVentas)
                    {
                        string consultinsert = "INSERT INTO venta(idVenta,idProducto, rutCliente, rutVendedor,cantidad, total,fecha,tipoVenta) VALUES (@idVenta,@idProducto, @rutCliente, @rutVendedor,@cantidad, @total,@fecha,@tipoVenta)";
                        MySqlCommand comm = new MySqlCommand(consultinsert, getconexion.getConexion());
                        //long id = comm.LastInsertedId;
                        //id = id + 1;
                        comm.Parameters.AddWithValue("@idVenta", idultimo);
                        comm.Parameters.AddWithValue("@idProducto", vts.idProducto);
                        comm.Parameters.AddWithValue("@rutCliente", vts.rutCliente);
                        comm.Parameters.AddWithValue("@rutVendedor", vts.rutVendedor);
                        comm.Parameters.AddWithValue("@cantidad", vts.cantidad);
                        comm.Parameters.AddWithValue("@total", vts.total);
                        comm.Parameters.AddWithValue("@fecha", vts.fecha);
                        comm.Parameters.AddWithValue("@tipoVenta", vts.tipoVenta);
                        comm.ExecuteNonQuery();


                    }
                }





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

        public int getUltimoIngresadoenVentas()
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT idVenta FROM venta ";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();
            int mayor = 0;
            ventasFacade vt = new ventasFacade();
            if (vt.getVentastotales() == 1)
            {
                total = 1;
            }

            else if (vt.getVentastotales() > 1)
            {
                while (rdr.Read())
                {
                    if (rdr.GetInt64(0) > mayor)
                    {
                        mayor = rdr.GetInt32(0);
                    }
                }

            }
            total = mayor;


            getconexion.CerrarConexion();

            return total;

        }
        public int getVentastotales()
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT*FROM venta ";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }

        public List<MVentas> getVentasByFechaDia(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT*FROM venta WHERE YEAR(fecha)=" + fecha.Year + " AND MONTH(fecha)=" + fecha.Month + " AND DAY(fecha)=" + fecha.Day + "";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetInt64(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetDateTime(6), rdr.GetString(7)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }
        public int getVentasByFechaDiaPagoEfectivo(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT tipoVenta FROM venta WHERE YEAR(fecha)=" + fecha.Year + " AND MONTH(fecha)=" + fecha.Month + " AND DAY(fecha)=" + fecha.Day + " AND tipoVenta=\"efectivo\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }
        public int getVentasByFechaDiaPagoCuenta(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT tipoVenta FROM venta WHERE YEAR(fecha)=" + fecha.Year + " AND MONTH(fecha)=" + fecha.Month + " AND DAY(fecha)=" + fecha.Day + " AND tipoVenta=\"cuenta\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }
        public int getVentasByFechaDiaPagoDebito(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT tipoVenta FROM venta WHERE YEAR(fecha)=" + fecha.Year + " AND MONTH(fecha)=" + fecha.Month + " AND DAY(fecha)=" + fecha.Day + " AND tipoVenta=\"debito\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }
        public int getVentasByFechaDiaPagoCheque(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT tipoVenta FROM venta WHERE YEAR(fecha)=" + fecha.Year + " AND MONTH(fecha)=" + fecha.Month + " AND DAY(fecha)=" + fecha.Day + " AND tipoVenta=\"cheque\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }


        public List<MVentas> getVentasByidProd(string idproducto)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT*FROM venta WHERE idProducto=\"" + idproducto + "\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetInt64(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetDateTime(6), rdr.GetString(7)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }

        //#########################################################
        //################      MES         ######################3
        //#########################################################
        public List<MVentas> getVentasByFechaMes(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT*FROM venta WHERE MONTH(fecha) =" + fecha.Month + " AND year(fecha)=" + fecha.Year;

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetInt64(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetDateTime(6), rdr.GetString(7)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }

        public int getVentasByFechaMesPagoEfectivo(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT*FROM venta WHERE MONTH(fecha) =" + fecha.Month + " AND year(fecha)=" + fecha.Year + " AND tipoVenta=\"efectivo\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }
        public int getVentasByFechaMesPagocuenta(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT*FROM venta WHERE MONTH(fecha) =" + fecha.Month + " AND year(fecha)=" + fecha.Year + " AND tipoVenta=\"cuenta\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }

        public int getVentasByFechaMesPagodebito(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT*FROM venta WHERE MONTH(fecha) =" + fecha.Month + " AND year(fecha)=" + fecha.Year + " AND tipoVenta=\"debito\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }
        public int getVentasByFechaMesPagoCheque(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT*FROM venta WHERE MONTH(fecha) =" + fecha.Month + " AND year(fecha)=" + fecha.Year + " AND tipoVenta=\"cheque\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }

        public List<MVentas> getVentasbyIdProdGroupByFecha(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT fecha, idProducto,(SELECT nombre FROM db.producto WHERE idProducto=\"" + idproducto + "\") as nombre,sum(cantidad)as cantidad, sum(total) as total ,(SELECT idCategoria FROM db.producto WHERE idProducto=\"" + idproducto + "\") as idCategoria  FROM db.venta  where idProducto=\"" + idproducto + "\" AND month(fecha)=" + fecha.Month + " AND year(fecha)=" + fecha.Year + " GROUP BY fecha;";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }

        public List<MVentas> getCostoVentasbyIdProdGroupByFecha(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            //string consulta = "SELECT fecha, idProducto,(SELECT nombre FROM db.producto WHERE idproducto=\"" + idproducto + "\") as nombre,sum(cantidad)as cantidad, sum(total) as total ,(SELECT idCategoria FROM db.producto WHERE idproducto=\"" + idproducto + "\") as idCategoria  FROM db.venta  where idproducto=\"" + idproducto + "\" AND month(fecha)=" + fecha.Month + " AND year(fecha)=" + fecha.Year + " GROUP BY fecha;";
            string consulta = "SELECT fecha,idproducto,(SELECT nombre FROM db.producto WHERE idproducto=\"" + idproducto + "\") as nombre,(select precioReal from producto where idproducto=\"" + idproducto + "\")as precioReal,sum(cantidad), (SELECT idCategoria FROM db.producto WHERE idProducto=\"" + idproducto + "\") as idCategoria  FROM venta WHERE  idProducto=\"" + idproducto + "\" AND month(fecha)=" + fecha.Month + " AND year(fecha)=" + fecha.Year + " GROUP BY fecha";
            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), (Convert.ToInt32(rdr.GetString(3)) * rdr.GetInt32(4)), rdr.GetInt32(5)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }
        public List<MVentas> getVentasbyIdProdSinNombreGroupByFecha(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT fecha, idProducto,sum(cantidad)as cantidad, sum(total) as total FROM db.venta  where idProducto=\"" + idproducto + "\" AND month(fecha)=" + fecha.Month + " AND year(fecha)=" + fecha.Year + " GROUP BY fecha;";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", rdr.GetInt32(2), rdr.GetInt32(3), 0));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }
        public List<MVentas> getCostoVentasbyIdProdSinNombreGroupByFecha(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();
            ProductoFacade prodFac = new ProductoFacade();
            string consulta = "";
            bool existe = prodFac.getExisteProductoByidProd(idproducto);
            if (existe)
            {
                consulta = "SELECT fecha,idproducto,(select precioReal from producto where idproducto=\"" + idproducto + "\")as precioReal,cantidad   FROM venta WHERE idProducto=\"" + idproducto + "\" AND month(fecha)=" + fecha.Month + " AND year(fecha)=" + fecha.Year + " GROUP BY fecha;";
            }
            else
            {
                consulta = "SELECT fecha,idproducto,sum(cantidad)   FROM venta WHERE idProducto=\"" + idproducto + "\" AND month(fecha)=" + fecha.Month + " AND year(fecha)=" + fecha.Year + " GROUP BY fecha;";
            }
            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                //Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", rdr.GetString(2), rdr.GetInt32(3), Convert.ToInt32(rdr.GetInt32(2)) * rdr.GetInt32(3), 0));
                if (existe)
                {
                    Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", rdr.GetString(2), rdr.GetInt32(3), Convert.ToInt32(rdr.GetInt32(2)) * rdr.GetInt32(3), 0));
                }
                else
                {
                    Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", "0", rdr.GetInt32(2), 0, 0));

                }

            }
            getconexion.CerrarConexion();

            return Listventas;

        }
        //#########################################################
        //################      DIA         ######################3
        //#########################################################
        public List<MVentas> getVentasbyIdProdGroupByFechaDia(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT fecha, idProducto,(SELECT nombre FROM db.producto WHERE idProducto=\"" + idproducto + "\") as nombre,sum(cantidad)as cantidad, sum(total) as total ,(SELECT idCategoria FROM db.producto WHERE idProducto=\"" + idproducto + "\") as idCategoria  FROM db.venta  where idProducto=\"" + idproducto + "\" AND  YEAR(fecha)=" + fecha.Year + " AND MONTH(fecha)=" + fecha.Month + " AND DAY(fecha)=" + fecha.Day + " GROUP BY fecha;";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }
        public List<MVentas> getCostosVentasbyIdProdGroupByFechaDia(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT fecha,idproducto,(SELECT nombre FROM db.producto WHERE idproducto=\"" + idproducto + "\") as nombre,(select precioReal from producto where idproducto=\"" + idproducto + "\")as precioReal,sum(cantidad), (SELECT idCategoria FROM db.producto WHERE idProducto=\"" + idproducto + "\") as idCategoria  FROM venta WHERE idProducto=\"" + idproducto + "\" and YEAR(fecha)=" + fecha.Year + " AND MONTH(fecha)=" + fecha.Month + " AND DAY(fecha)=" + fecha.Day + " GROUP BY fecha;";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), (Convert.ToInt32(rdr.GetString(3)) * rdr.GetInt32(4)), rdr.GetInt32(5)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }

        public List<MVentas> getVentasbyIdProdSinNombreGroupByFechaDia(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT fecha, idProducto,sum(cantidad)as cantidad, sum(total) as total FROM db.venta  where idProducto=\"" + idproducto + "\" AND day(fecha)=" + fecha.Day + "  AND month(fecha)=" + fecha.Month + " AND year(fecha)=" + fecha.Year + " GROUP BY fecha;";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", rdr.GetInt32(2), rdr.GetInt32(3), 0));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }


        public List<MVentas> getCostoVentasbyIdProdSinNombreGroupByFechaDia(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();
            ProductoFacade prodFac = new ProductoFacade();
            string consulta = "";
            bool existe = prodFac.getExisteProductoByidProd(idproducto);
            if (existe)
            {
                consulta = "SELECT fecha,idproducto,(select precioReal from producto where idproducto=\"" + idproducto + "\")as precioReal,cantidad   FROM venta WHERE YEAR(fecha)=" + fecha.Year + " AND MONTH(fecha)=" + fecha.Month + " AND DAY(fecha)=" + fecha.Day + " GROUP BY fecha;";
            }
            else
            {
                consulta = "SELECT fecha,idproducto,sum(cantidad)   FROM venta WHERE YEAR(fecha)=" + fecha.Year + " AND MONTH(fecha)=" + fecha.Month + " AND DAY(fecha)=" + fecha.Day + " GROUP BY fecha;";
            }

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                if (existe)
                {
                    Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", rdr.GetString(2), rdr.GetInt32(3), Convert.ToInt32(rdr.GetInt32(2)) * rdr.GetInt32(3), 0));
                }
                else
                {
                    Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", "0", rdr.GetInt32(2), 0, 0));

                }

            }
            getconexion.CerrarConexion();

            return Listventas;

        }

        //#########################################################
        //################      AÑO         ######################3
        //#########################################################

        public List<MVentas> getVentasbyIdProdGroupByFechaAño(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT fecha, idProducto,(SELECT nombre FROM db.producto WHERE idproducto=\"" + idproducto + "\") as nombre,sum(cantidad)as cantidad, sum(total) as total ,(SELECT idCategoria FROM db.producto WHERE idproducto=\"" + idproducto + "\") as idCategoria  FROM db.venta  where idproducto=\"" + idproducto + "\" AND year(fecha)=" + fecha.Year + "  GROUP BY fecha;";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3), rdr.GetInt32(4), rdr.GetInt32(5)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }
        public int getVentasByFechaAñoPagoEfectivo(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT*FROM venta WHERE year(fecha)=" + fecha.Year + " AND tipoVenta=\"efectivo\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }
        public int getVentasByFechaAñoPagocuenta(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT*FROM venta WHERE year(fecha)=" + fecha.Year + " AND tipoVenta=\"cuenta\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }

        public int getVentasByFechasAñoPagodebito(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT*FROM venta WHERE  year(fecha)=" + fecha.Year + " AND tipoVenta=\"debito\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }
        public int getVentasByFechaAñoPagoCheque(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            int total = 0;

            string consulta = "SELECT*FROM venta WHERE year(fecha)=" + fecha.Year + " AND tipoVenta=\"cheque\"";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                total = total + 1;


            }
            getconexion.CerrarConexion();

            return total;

        }
        public List<MVentas> getCostoVentasbyIdProdGroupByFechaAño(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            //string consulta = "SELECT fecha, idProducto,(SELECT nombre FROM db.producto WHERE idproducto=\"" + idproducto + "\") as nombre,sum(cantidad)as cantidad, sum(total) as total ,(SELECT idCategoria FROM db.producto WHERE idproducto=\"" + idproducto + "\") as idCategoria  FROM db.venta  where idproducto=\"" + idproducto + "\" AND year(fecha)=" + fecha.Year + "  GROUP BY fecha;";
            string consulta = "SELECT fecha,idproducto,(SELECT nombre FROM db.producto WHERE idproducto=\"" + idproducto + "\") as nombre,(select precioReal from producto where idproducto=\"" + idproducto + "\")as precioReal,sum(cantidad), (SELECT idCategoria FROM db.producto WHERE idproducto=\"" + idproducto + "\") as idCategoria  FROM venta WHERE  idproducto=\"" + idproducto + "\" AND year(fecha)=" + fecha.Year + "  GROUP BY fecha;";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), (Convert.ToInt32(rdr.GetString(3)) * rdr.GetInt32(4)), rdr.GetInt32(5)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }


        public List<MVentas> getVentasbyIdProdSinNombreGroupByFechaAño(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT fecha, idProducto,sum(cantidad)as cantidad, sum(total) as total FROM db.venta  where idproducto=\"" + idproducto + "\" AND year(fecha)=" + fecha.Year + "  GROUP BY fecha;";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", rdr.GetInt32(2), rdr.GetInt32(3), 0));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }
        public List<MVentas> getCostoVentasbyIdProdSinNombreGroupByFechaAño(string idproducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();
            ProductoFacade prodFac = new ProductoFacade();
            string consulta = "";
            bool existe = prodFac.getExisteProductoByidProd(idproducto);
            if (existe)
            {
                //string consulta = "SELECT fecha, idProducto,sum(cantidad)as cantidad, sum(total) as total FROM db.venta  where idproducto=\"" + idproducto + "\" AND year(fecha)=" + fecha.Year + "  GROUP BY fecha;";
                consulta = "SELECT fecha,idproducto,(select precioReal from producto where idproducto=\"" + idproducto + "\")as precioReal,cantidad   FROM venta WHERE idproducto=\"" + idproducto + "\" AND year(fecha)=" + fecha.Year + "  GROUP BY fecha";
            }
            else
            {
                consulta = "SELECT fecha,idproducto,sum(cantidad)   FROM venta WHERE idproducto=\"" + idproducto + "\" AND year(fecha)=" + fecha.Year + "  GROUP BY fecha";
            }
            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                //Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", rdr.GetString(2), rdr.GetInt32(3), Convert.ToInt32(rdr.GetInt32(2)) * rdr.GetInt32(3), 0));
                if (existe)
                {
                    Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", rdr.GetString(2), rdr.GetInt32(3), Convert.ToInt32(rdr.GetInt32(2)) * rdr.GetInt32(3), 0));
                }
                else
                {
                    Listventas.Add(new MVentas(rdr.GetDateTime(0), rdr.GetString(1), "Sin Nombre", "0", rdr.GetInt32(2), 0, 0));

                }


            }
            getconexion.CerrarConexion();

            return Listventas;

        }


        public List<MVentas> getVentasByFechaAño(DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT*FROM venta WHERE YEAR(fecha)='" + fecha.Year + "'";

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetInt64(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetDateTime(6), rdr.GetString(7)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }
        //Detales de Venta para devolucion
        public List<MVentas> getVentasForDevolucion(double id, string idProducto, DateTime fecha)
        {
            MySqlDataReader rdr = null;
            List<MVentas> Listventas = new List<MVentas>();

            string consulta = "SELECT*FROM venta WHERE idVenta=" + id + " and idProducto=\"" + idProducto + "\" and day(fecha)=" + fecha.Day + " and month(fecha)=" + fecha.Month + " and year(fecha)=" + fecha.Year;

            MySqlCommand cmd = new MySqlCommand(consulta, getconexion.getConexion());
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Listventas.Add(new MVentas(rdr.GetInt64(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), rdr.GetInt32(5), rdr.GetDateTime(6), rdr.GetString(7)));

            }
            getconexion.CerrarConexion();

            return Listventas;

        }
        public string borrarventaByidVenta(double id, string idProducto, DateTime fecha)
        {
            string borrar = "";
            try
            {

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                string cons = "DELETE FROM venta WHERE  idVenta=" + id + " and idProducto=\"" + idProducto + "\" and day(fecha)=" + fecha.Day + " and month(fecha)=" + fecha.Month + " and year(fecha)=" + fecha.Year;
                MySqlCommand cmdCategoria = new MySqlCommand(cons, getconexion.getConexion());
                cmdCategoria.ExecuteNonQuery();


            }
            catch (Exception e)
            {
                borrar = e.ToString();
            }
            return borrar;


        }
        public string actualizarventaDevolucion(double id, string idProducto, DateTime fecha, int cantidad, int total)
        {
            //Boolean guardar = false;
            string res = "";

            try
            {

                //int stockAnterior = Convert.ToInt32(getStockProductoByidProd(idProducto));
                //int stockActualizado = stockAnterior + Convert.ToInt32(stock);
                string consultinsert = "UPDATE  venta SET cantidad=@cantidad,total=@total WHERE idVenta=" + id + " and idProducto=\"" + idProducto + "\" and day(fecha)=" + fecha.Day + " and month(fecha)=" + fecha.Month + " and year(fecha)=" + fecha.Year;
                MySqlCommand comm = new MySqlCommand(consultinsert, getconexion.getConexion());
                comm.Parameters.AddWithValue("@cantidad", cantidad);
                comm.Parameters.AddWithValue("@total", total);
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



    }


}
