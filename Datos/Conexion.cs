using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace Datos
{
    public class Conexion
    {

        public static string cadenaConexion = ConfigurationManager.ConnectionStrings["Clover_Connection"].ToString();

        private static void Cadena()
        {
            if (string.IsNullOrEmpty(cadenaConexion))
                cadenaConexion = "Data Source=mxni-app-02\\Innova;Initial Catalog=CloverLabel;Persist Security Info=True;User ID=SA;Password=innova2011";
        }
        public static string CadenaConexion()
        {
            Cadena();
            return cadenaConexion;
        }
    }
}
