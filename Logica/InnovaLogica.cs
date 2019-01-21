using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class InnovaLogica
    {
        public string Orden { get; set; }
        public string Recibo { get; set; }
        public int Consec { get; set; }
        public string Articulo { get; set; }

        public static DataTable ConsultaReciboMov(InnovaLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT re.fecha,re.no_recibo,re.orden,rq.no_art,rq.desc_art,rc.cantidad,rq.um,rc.almacen,re.recibio "+
                "FROM recibo_mov rc INNER JOIN recibo re on rc.no_recibo = re.no_recibo "+
                "INNER JOIN req_mov rq on rc.csc_mov = rq.csc_mov and rc.no_req = rq.no_req "+
                "WHERE re.no_recibo = '" + rec.Recibo + "' and rc.csc_mov = " + rec.Consec + "";
                datos = AccesoDatos.ConsultarInnova(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable ConsultaRecibo(InnovaLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT rq.no_art,rq.desc_art,rc.cantidad,rq.um  " +
                "FROM recibo_mov rc INNER JOIN req_mov rq on rc.csc_mov = rq.csc_mov and rc.no_req = rq.no_req " +
                "WHERE rc.no_recibo = '" + rec.Recibo + "'";
                datos = AccesoDatos.ConsultarInnova(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
        public static DataTable ConsultaReciboArt(InnovaLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT rc.csc_mov,rc.no_recibo,rc.orden,rq.no_art,rq.desc_art,rc.cantidad,rq.um  " +
                "FROM recibo_mov rc INNER JOIN req_mov rq on rc.csc_mov = rq.csc_mov and rc.no_req = rq.no_req " +
                "WHERE rc.no_recibo = '" + rec.Recibo + "' and rq.no_art = '" + rec.Articulo + "'";
                datos = AccesoDatos.ConsultarInnova(sQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaOrden(InnovaLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT rq.no_art,rq.desc_art,rc.cantidad,rq.um  " +
                "FROM recibo_mov rc INNER JOIN req_mov rq on rc.csc_mov = rq.csc_mov and rc.no_req = rq.no_req WHERE rc.orden = '" + rec.Orden + "' ";
                datos = AccesoDatos.ConsultarInnova(sQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaOrdenArt(InnovaLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT rc.csc_mov,rc.no_recibo,rc.orden,rq.no_art,rq.desc_art,rc.cantidad,rq.um  "+
                "FROM recibo_mov rc INNER JOIN req_mov rq on rc.csc_mov = rq.csc_mov and rc.no_req = rq.no_req "+
                "WHERE rc.orden = '"+rec.Orden+"' and rq.no_art = '"+rec.Articulo+"'";
                datos = AccesoDatos.ConsultarInnova(sQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }

        public static DataTable ConsultaArticulo(InnovaLogica rec)
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT * FROM articulo WHERE no_art = '" + rec.Articulo + "' ";
                datos = AccesoDatos.ConsultarInnova(sQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
