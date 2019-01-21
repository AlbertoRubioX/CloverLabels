using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ReciboLogica
    {
        public string Orden { get; set; }
        public string Recibo { get; set; }
        public int Consec { get; set; }
        public string Articulo { get; set; }
        public string Usuario { get; set; }
        public static bool ConsultaPendientesRec(ReciboLogica rec)
        {
            try
            {
                string sQuery = "SELECT * FROM t_recibo_mov WHERE cast(f_carga as date) = cast('"+DateTime.Today+"' as date) and ind_print='0' and recibio = '"+rec.Usuario+"'";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static DataTable ListarPendientesRec(ReciboLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT * FROM t_recibo_mov WHERE cast(f_carga as date) = cast('" + DateTime.Today + "' as date) and ind_print='0' and recibio = '"+rec.Usuario+"'";
                datos = AccesoDatos.Consultar(sQuery);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaReciboMov(ReciboLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT * FROM t_recibo_mov WHERE recibo = '"+ rec.Recibo +"' and consec = " + rec.Consec + "";
                datos = AccesoDatos.Consultar(sQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaReciboArt(ReciboLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT * FROM t_recibo_mov WHERE recibo = '" + rec.Recibo + "' and no_art = '" + rec.Articulo + "'";
                datos = AccesoDatos.Consultar(sQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaRecibo(ReciboLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT * FROM t_recibo_mov WHERE recibo = '" + rec.Recibo + "' ";
                datos = AccesoDatos.Consultar(sQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable ConsultaOrden(ReciboLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT * FROM t_recibo_mov WHERE orden = '" + rec.Orden + "' ";
                datos = AccesoDatos.Consultar(sQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable ConsultaOrdenArt(ReciboLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT * FROM t_recibo_mov WHERE orden = '" + rec.Orden + "' and no_art = '"+rec.Articulo+"' ";
                datos = AccesoDatos.Consultar(sQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool ReciboPrint(ReciboLogica rec)
        {
            try
            {
                string sQuery = "UPDATE t_recibo_mov SET ind_print = '1',f_print = '"+DateTime.Now+"' WHERE recibo = '" + rec.Recibo + "' and consec = " + rec.Consec + " ";
                if (AccesoDatos.Borrar(sQuery) != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool ConsultaPendientesLoc()
        {
            try
            {
                string sQuery = "SELECT * FROM t_recibo_mov WHERE cast(f_carga as date) = cast('" + DateTime.Today + "' as date) and ind_print2='0'";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public static DataTable ListarPendientesLoc()
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT * FROM t_recibo_mov WHERE cast(f_carga as date) = cast('" + DateTime.Today + "' as date) and ind_print2='0'";
                datos = AccesoDatos.Consultar(sQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static bool LocacionPrint(ReciboLogica rec)
        {
            try
            {
                string sQuery = "UPDATE t_recibo_mov SET ind_print2 = '1',f_print2 = '" + DateTime.Now + "' WHERE recibo = '" + rec.Recibo + "' and consec = " + rec.Consec + " ";
                if (AccesoDatos.Borrar(sQuery) != 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
