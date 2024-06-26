using ConTransaction.Commands;
using ConTransaction.Entities;
using ConTransaction.Services;
using ConTransaction.TransactionDBDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ConTransaction
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Venta venta = new Venta();
                venta.Cliente = "Edgar";

                List<Producto> productos = ConsultarProductosDesdeBaseDeDatos();

                foreach (var producto in productos)
                {
                    VentaDetalle detalle = new VentaDetalle();
                    detalle.Cantidad = 1; 
                    detalle.Descripcion = producto.Nombre;
                    detalle.PrecioUnitario = producto.PrecioUnitario;
                    venta.Conceptos.Add(detalle);
                }

                VentaServices ventaServices = new VentaServices();
                int folio = ventaServices.GuardarVenta(venta);

                MessageBox.Show("Venta guardada correctamente. Folio: " + folio);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar la venta: " + ex.Message);
            }
        }

        private List<Producto> ConsultarProductosDesdeBaseDeDatos()
        {
            List<Producto> productos = new List<Producto>();

            try
            {
                string connectionString = "Server=localhost;Database=TransactionDB;Trusted_Connection=True;";

                string query = "SELECT Id, Nombre, PrecioUnitario FROM Productos";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Producto producto = new Producto();

                            if (!reader.IsDBNull(reader.GetOrdinal("Id")))
                                producto.Id = reader.GetInt32(reader.GetOrdinal("Id"));

                            if (!reader.IsDBNull(reader.GetOrdinal("Nombre")))
                                producto.Nombre = reader.GetString(reader.GetOrdinal("Nombre"));

                            if (!reader.IsDBNull(reader.GetOrdinal("PrecioUnitario")))
                                producto.PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario"));

                            productos.Add(producto);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron productos en la base de datos.");
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar productos desde la base de datos: " + ex.Message);
            }

            return productos;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'transactionDBDataSet11.Productos' Puede moverla o quitarla según sea necesario.
            this.productosTableAdapter1.Fill(this.transactionDBDataSet11.Productos);

        }

    }
   
        
    }


