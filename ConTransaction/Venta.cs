using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConTransaction
{
    public class Venta
    {
        public int Folio { get; set; }
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public decimal Total { get; set; }
        public List<VentaDetalle> Conceptos { get; set; } = new List<VentaDetalle>();

        public void AgregarProducto(Producto producto, int cantidad)
        {
            VentaDetalle detalle = new VentaDetalle
            {
                VentaId = this.Id,
                ProductoId = producto.Id, 
                Cantidad = cantidad,
                Descripcion = producto.Nombre,
                PrecioUnitario = producto.PrecioUnitario,
                Renglon = Conceptos.Count + 1
            };

            Conceptos.Add(detalle);
            Total += detalle.Importe;
        }
    }

}
