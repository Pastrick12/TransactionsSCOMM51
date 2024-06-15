using SinTransaction.Commands;
using SinTransaction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SinTransaction.Services
{
    internal class VentaServices
    {
        public int GuardarVenta(Venta venta) 
        {
			try
			{
			VentaCommand command = new VentaCommand();
                command.GuardarVenta(venta);
                return venta.Folio;

			}
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
