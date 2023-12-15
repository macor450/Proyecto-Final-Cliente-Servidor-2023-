﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proyecto_Final_Cliente_Servidor_2023
{
    public partial class FmBookCatalog : Form
    {
        string connectionString = "Data Source=DESKTOP-OTK3IQJ;Initial Catalog=AreaServices;Integrated Security=True;";
        private SqlConnection connection;
        public FmBookCatalog()
        {
            InitializeComponent();
            LlenarComboBoxArea();
            LlenarComboBoxCourse();
            MostrarDatos();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FmMenu Menu = new FmMenu();
            Menu.Show();
            this.Hide(); // Opcional: oculta el formulario actual si no lo necesitas más
        }

        public void LlenarComboBoxArea()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión
                    connection.Open();

                    // Query para obtener los IDs de la tabla foránea
                    string query = "SELECT IdArea FROM Area";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Crear un objeto SqlDataAdapter para obtener los datos
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        // Llenar el DataTable con los datos obtenidos
                        adapter.Fill(dataTable);

                        // Asignar los IDs al ComboBox
                        cmbIdArea.DataSource = dataTable;
                        cmbIdArea.DisplayMember = "IdArea";
                        cmbIdArea.ValueMember = "IdArea";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al llenar el ComboBox: " + ex.Message);
            }
        }

        public void LlenarComboBoxCourse()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión
                    connection.Open();

                    // Query para obtener los IDs de la tabla foránea
                    string query = "SELECT IdCourse FROM Course";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Crear un objeto SqlDataAdapter para obtener los datos
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        // Llenar el DataTable con los datos obtenidos
                        adapter.Fill(dataTable);

                        // Asignar los IDs al ComboBox
                        cmbIdCourse.DataSource = dataTable;
                        cmbIdCourse.DisplayMember = "idCourse";
                        cmbIdCourse.ValueMember = "idCourse";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al llenar el ComboBox: " + ex.Message);
            }
        }

        private void MostrarDatos()
        {

            string consulta = "SELECT * FROM BookCatalog Where status=1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(consulta, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                System.Data.DataTable dataTable = new System.Data.DataTable();
                adapter.Fill(dataTable);

                dgvBookCatalog.DataSource = dataTable;
            }
        }

        private void Limpiar()
        {
            txtBookTitle.Text = "";
            txtAuthorName.Text = "";
            txtPublisher.Text = "";
            txtInStock.Text = "";
            cmbIdArea.Text = "";
            cmbIdCourse.Text = "";
            txtStatus.Text = "";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            try
            {
                // Validar campos obligatorios antes de la inserción
                if (string.IsNullOrEmpty(txtAuthorName.Text) || string.IsNullOrEmpty(txtBookTitle.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta de inserción
                    string query = "INSERT INTO BookCatalog (bookTitle, authorName, publisher, inStock, idArea, idCourse, status)" +
                                   "VALUES (@bookTitle, @authorName, @publisher, @inStock, @idArea, @idCourse, @status)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parámetros de la consulta de inserción
                        command.Parameters.AddWithValue("@bookTitle", txtBookTitle.Text);
                        command.Parameters.AddWithValue("@authorName", txtAuthorName.Text);
                        command.Parameters.AddWithValue("@publisher", txtPublisher.Text);
                        command.Parameters.AddWithValue("@inStock", txtInStock.Text);
                        command.Parameters.AddWithValue("@idArea", cmbIdArea.SelectedValue);
                        command.Parameters.AddWithValue("@idCourse", cmbIdCourse.SelectedValue);
                        command.Parameters.AddWithValue("@status", 1);

                        // Ejecutar la consulta de inserción
                        command.ExecuteNonQuery();

                        MessageBox.Show("Inserción exitosa.");
                        MostrarDatos(); // Función para actualizar la vista con los nuevos datos
                        Limpiar(); // Función para limpiar los campos después de la inserción
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar en la base de datos: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            // Obtener los valores de los TextBox
            string bookTitle = txtBookTitle.Text;
            string authorName = txtAuthorName.Text;
            string publisher = txtPublisher.Text;
            string inStock = txtInStock.Text;
          

            try
            {
                if (dgvBookCatalog.SelectedRows.Count > 0)
                {
                    int idToDelete = Convert.ToInt32(dgvBookCatalog.SelectedRows[0].Cells["idBookCatalog"].Value);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "UPDATE BookCatalog SET bookTitle = @BookTitle, authorName = @AuthorName, publisher = @Publisher, inStock = @InStock " +
                         
                          "WHERE idBookCatalog = @idBookCatalog"; // Reemplaza "TuTabla" por el nombre de tu tabla y ajusta el WHERE según tus necesidades



                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Agregar parámetros para la consulta UPDATE
                            command.Parameters.AddWithValue("@BookTitle", bookTitle);
                            command.Parameters.AddWithValue("@AuthorName", authorName);
                            command.Parameters.AddWithValue("@Publisher", publisher);
                            command.Parameters.AddWithValue("@InStock", inStock);
                        
                            // Agregar el parámetro para el ID de la fila que se va a actualizar
                            command.Parameters.AddWithValue("@idBookCatalog", idToDelete);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Actualización exitosa.");
                                MostrarDatos();
                                // Aquí podrías agregar más lógica si es necesario después de la actualización
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el registro para actualizar.");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar actualizar: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBookCatalog.SelectedRows.Count > 0)
                {
                    int idToDelete = Convert.ToInt32(dgvBookCatalog.SelectedRows[0].Cells["idBookCatalog"].Value);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "UPDATE BookCatalog SET status = 0 WHERE idBookCatalog = @IdParaActualizar";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@IdParaActualizar", idToDelete);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Se elimino de la forma correcta");
                                MostrarDatos();
                                Limpiar();
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el registro para eliminar.");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, seleccione una fila para actualizar el estado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar actualizar el estado: " + ex.Message);
            }
        }
    }
}
