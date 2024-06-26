using ConTransaction.Commands;
using ConTransaction.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConTransaction
{
    public class VentaServices
    {
        public int GuardarVenta(Venta venta)
        {
            try
            {
                decimal totalVenta = CalcularTotalVenta(venta);

                venta.Total = totalVenta;

                using (SqlConnection con = new SqlConnection(Configuracion.ConnectionString))
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();

                    try
                    {
                        FoliosCommands foliosCommands = new FoliosCommands();
                        venta.Folio = foliosCommands.ObtenerSiguienteFolio(con, tran);

                        VentaCommand command = new VentaCommand();
                        venta.Id = command.GuardarVenta(con, tran, venta);

                        foliosCommands.ActualizarFolio(con, tran);

                        int renglon = 1;
                        foreach (VentaDetalle concepto in venta.Conceptos)
                        {
                            concepto.VentaId = venta.Id;
                            concepto.Renglon = renglon;

                            VentaDetalleCommands conceptoCommand = new VentaDetalleCommands();
                            conceptoCommand.GuardarVentaDetalle(con, tran, concepto);

                            renglon++;
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        throw new Exception("Error al guardar venta y/o detalles: " + ex.Message);
                    }
                }

                return venta.Folio;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la venta: " + ex.Message);
            }
        }

        private decimal CalcularTotalVenta(Venta venta)
        {
            decimal total = 0;

            foreach (var detalle in venta.Conceptos)
            {
                total += detalle.Cantidad * detalle.PrecioUnitario;
            }

            return total;
        }
    }
}
