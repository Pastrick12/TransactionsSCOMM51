using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace TuProyecto
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection;
        private SqlTransaction sqlTransaction;

        public Form1()
        {


            // Establecer la cadena de conexión (puedes obtenerla desde el Explorador de Servidores)
            string connectionString = "Server=localhost;Database=TransactionDB;Trusted_Connection=True;TrustServerCertificate=True;";

            sqlConnection = new SqlConnection(connectionString);
        }



        private void LoadData()
        {
            try
            {
                sqlConnection.Open();
                string query = "SELECT p.IdProducto, p.Nombre, p.Precio, s.Cantidad FROM Productos p INNER JOIN Stock s ON p.IdProducto = s.IdProducto";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, sqlConnection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void ActualizarStock(int idProducto, int cantidad)
        {
            try
            {
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();

                // Actualizar el stock
                string updateQuery = "UPDATE Stock SET Cantidad = Cantidad + @Cantidad WHERE IdProducto = @IdProducto";
                SqlCommand updateCommand = new SqlCommand(updateQuery, sqlConnection, sqlTransaction);
                updateCommand.Parameters.AddWithValue("@Cantidad", cantidad);
                updateCommand.Parameters.AddWithValue("@IdProducto", idProducto);
                updateCommand.ExecuteNonQuery();

                // Confirmar la transacción
                sqlTransaction.Commit();

                MessageBox.Show("Stock actualizado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el stock: " + ex.Message);
                try
                {
                    sqlTransaction.Rollback();
                }
                catch (Exception rollbackEx)
                {
                    MessageBox.Show("Error al deshacer la transacción: " + rollbackEx.Message);
                }
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private void btnActualizarStock_Click(object sender, EventArgs e)
        {
            // Ejemplo de uso para actualizar el stock de un producto
            int idProducto = 1; // Id del producto a actualizar (debes obtenerlo según el contexto de tu aplicación)
            int cantidad = 10; // Cantidad a agregar al stock

            ActualizarStock(idProducto, cantidad);

            // Puedes llamar a LoadData() nuevamente para refrescar los datos en el DataGridView
            LoadData();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            this.ResumeLayout(false);

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }
}