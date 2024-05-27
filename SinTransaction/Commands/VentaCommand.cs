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
    internal class VentaCommand
    {
        public int GuardarVenta(Venta venta)
        {
			try
			{
                using (SqlConnection con = new SqlConnection(Configuracion.ConnectionString))
                {
                    string query = "insert into Ventas" +
                        "(Folio,Fecha,Cliente,Total) " +
                        "values" +
                        "(@Folio,getDate(),@Cliente,@Total),@@Scope_Identity()";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;

                        cmd.Parameters.AddWithValue("@Folio", venta.Folio);
                        cmd.Parameters.AddWithValue("@Cliente", venta.Cliente);
                        cmd.Parameters.AddWithValue("@Total", venta.Total);

                        con.Open();
                        cmd.ExecuteNonQuery();

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
