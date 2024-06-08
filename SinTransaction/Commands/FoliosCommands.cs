using SinTransaction.Database;
using SinTransaction.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinTransaction.Commands
{
    internal class FoliosCommands
    {
        public int ObtenerSiguienteFolio()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Configuracion.ConnectionString))
                {
                    string query = "Select FolioActual + 1 from Folios";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        con.Open();
                        var resultado = cmd.ExecuteScalar();
                        int foliosiguiente = 0;
                        bool sePudoConvertir = false;
                        sePudoConvertir = int.TryParse(resultado.ToString(), out foliosiguiente);
                        if (!sePudoConvertir) 
                        {
                            throw new Exception("No se pudo obtener el folio");
                        }
                        return foliosiguiente;

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return 0;
        }
    }
}
