using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Datos;

namespace Logica
{
    public class UsuarioLogica
    {
        public string Usuario { get; set; }

        public static string GetArea(UsuarioLogica us)
        {
            try
            {
                string sArea = string.Empty;
                string sQuery = "SELECT * FROM t_usuario WHERE usuario = '" + us.Usuario + "'";
                DataTable datos = AccesoDatos.Consultar(sQuery);
                if (datos.Rows.Count != 0)
                    sArea = datos.Rows[0]["area"].ToString();
                return sArea;
            }
            catch
            {
                return "";
            }
        }
    }
}
