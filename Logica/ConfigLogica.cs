using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class ConfigLogica
    {
        public static DataTable Consutlar()
        {
            DataTable datos = new DataTable();
            try
            {
                string sQuery = "SELECT * FROM t_config WHERE clave='01'";
                datos = AccesoDatos.Consultar(sQuery);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return datos;
        }
    }
}
