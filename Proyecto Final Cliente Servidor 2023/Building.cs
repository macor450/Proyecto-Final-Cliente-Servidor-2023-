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
    public partial class FmBuilding : Form
    {
        string connectionString = "Data Source=DESKTOP-OTK3IQJ;Initial Catalog=AreaServices;Integrated Security=True;";
        private SqlConnection connection;
        public FmBuilding()
        {
            InitializeComponent();
            MostrarDatos();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FmMenu Menu = new FmMenu();
            Menu.Show();
            this.Hide(); // Opcional: oculta el formulario actual si no lo necesitas más
        }

        private void MostrarDatos()
        {

            string consulta = "SELECT * FROM Building Where status=1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(consulta, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                System.Data.DataTable dataTable = new System.Data.DataTable();
                adapter.Fill(dataTable);

                dgvBuilding.DataSource = dataTable;
            }
        }

        private void Limpiar()
        {
            txtIdBuilding.Text = "";
            txtNumber.Text = "";
            txtDescriptionBuilding.Text = "";
            txtStatus.Text = "";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

            try
            {
                // Validar campos obligatorios antes de la inserción
                if (string.IsNullOrEmpty(txtDescriptionBuilding.Text) || string.IsNullOrEmpty(txtNumber.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos obligatorios.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta de inserción
                    string query = "INSERT INTO Building (number, descriptionBuilding, status)" +
                                   "VALUES (@number, @descriptionBuilding,@status)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parámetros de la consulta de inserción
                        command.Parameters.AddWithValue("@number", txtNumber.Text);
                        command.Parameters.AddWithValue("@descriptionBuilding", txtDescriptionBuilding.Text);
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
            string idBuilding = txtIdBuilding.Text;
            string number = txtNumber.Text;
            string descriptionBuilding = txtDescriptionBuilding.Text;
    

            try
            {
                if (dgvBuilding.SelectedRows.Count > 0)
                {
                    int idToDelete = Convert.ToInt32(dgvBuilding.SelectedRows[0].Cells["idBuilding"].Value);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "UPDATE Building SET number = @Number, descriptionBuilding = @DescriptionBuilding" +

                          "WHERE idBuilding = @idBuilding"; // Reemplaza "TuTabla" por el nombre de tu tabla y ajusta el WHERE según tus necesidades



                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Agregar parámetros para la consulta UPDATE
                            command.Parameters.AddWithValue("@Number", number);
                            command.Parameters.AddWithValue("@DescriptionBuilding", descriptionBuilding);
                            
                            // Agregar el parámetro para el ID de la fila que se va a actualizar
                            command.Parameters.AddWithValue("@idBuilding", idToDelete);

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
                if (dgvBuilding.SelectedRows.Count > 0)
                {
                    int idToDelete = Convert.ToInt32(dgvBuilding.SelectedRows[0].Cells["idBuilding"].Value);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string query = "UPDATE Building SET status = 0 WHERE idBuilding = @IdParaActualizar";

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
