using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConTransaction
{
    public class GestorProductos
    {
        private string connectionString = "Server=localhost;Database=TransactionDB;Trusted_Connection=True;";

        public void AgregarProducto(Producto producto)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Productos (Nombre, PrecioUnitario) VALUES (@Nombre, @PrecioUnitario);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@PrecioUnitario", producto.PrecioUnitario);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AgregarProductos()
        {
            Producto nuevoProducto = new Producto { Nombre = "Coca Cola", PrecioUnitario = 15, Id = 1 };
            Producto nuevoProducto1 = new Producto { Nombre = "Sabritas", PrecioUnitario = 10, Id = 2 };
            Producto nuevoProducto2 = new Producto { Nombre = "Galletas", PrecioUnitario = 20, Id = 3 };


            AgregarProducto(nuevoProducto);
            AgregarProducto(nuevoProducto1);
            AgregarProducto(nuevoProducto2);
        }

        public List<Producto> ObtenerProductos()
        {
            List<Producto> productos = new List<Producto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Nombre, PrecioUnitario FROM Productos;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Producto producto = new Producto
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                PrecioUnitario = reader.GetDecimal(2)
                            };

                            productos.Add(producto);
                        }
                    }
                }
            }

            return productos;
        }

        public void AgregarVenta(Venta venta)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string queryVenta = "INSERT INTO Ventas (Fecha, Cliente, Total) OUTPUT INSERTED.Id VALUES (@Fecha, @Cliente, @Total);";

                    using (SqlCommand commandVenta = new SqlCommand(queryVenta, connection, transaction))
                    {
                        commandVenta.Parameters.AddWithValue("@Fecha", venta.Fecha);
                        commandVenta.Parameters.AddWithValue("@Cliente", venta.Cliente);
                        commandVenta.Parameters.AddWithValue("@Total", venta.Total);

                        venta.Id = (int)commandVenta.ExecuteScalar();
                    }

                    string queryDetalle = "INSERT INTO VentaDetalles (VentaId, Renglon, ProductoId, Cantidad, Descripcion, PrecioUnitario) VALUES (@VentaId, @Renglon, @ProductoId, @Cantidad, @Descripcion, @PrecioUnitario);";
                    string queryActualizarStock = "UPDATE Stock SET Cantidad = Cantidad - @Cantidad WHERE ProductoId = @ProductoId;";

                    foreach (var detalle in venta.Conceptos)
                    {
                        using (SqlCommand commandDetalle = new SqlCommand(queryDetalle, connection, transaction))
                        {
                            commandDetalle.Parameters.AddWithValue("@VentaId", venta.Id);
                            commandDetalle.Parameters.AddWithValue("@Renglon", detalle.Renglon);
                            commandDetalle.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);
                            commandDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                            commandDetalle.Parameters.AddWithValue("@Descripcion", detalle.Descripcion);
                            commandDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);

                            commandDetalle.ExecuteNonQuery();
                        }

                        using (SqlCommand commandActualizarStock = new SqlCommand(queryActualizarStock, connection, transaction))
                        {
                            commandActualizarStock.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                            commandActualizarStock.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);

                            commandActualizarStock.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al agregar la venta: " + ex.Message);
                }
            }
        }
    }
}
