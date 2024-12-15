using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DeliveryMaster
{
    public partial class RoutesForm : Form
    {
        public RoutesForm()
        {
            InitializeComponent();
            CreateUI();
        }

        private void CreateUI()
        {
            this.Text = "Маршрути";
            this.Size = new System.Drawing.Size(800, 500);

            DataGridView dgvRoutes = new DataGridView
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(760, 350),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            this.Controls.Add(dgvRoutes);

            Button btnAdd = new Button { Text = "Додати", Location = new System.Drawing.Point(10, 380) };
            Button btnEdit = new Button { Text = "Редагувати", Location = new System.Drawing.Point(110, 380) };
            Button btnDelete = new Button { Text = "Видалити", Location = new System.Drawing.Point(210, 380) };
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);

            btnAdd.Click += (s, e) => ShowInputDialog("Додати маршрут", dgvRoutes, true);
            btnEdit.Click += (s, e) => ShowInputDialog("Редагувати маршрут", dgvRoutes, false);
            btnDelete.Click += (s, e) => DeleteRoute(dgvRoutes);

            LoadRoutes(dgvRoutes);
        }

        private void LoadRoutes(DataGridView dgvRoutes)
        {
            try
            {
                string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                string query = "SELECT RouteID, StartPoint, EndPoint, DistanceKM, EstimatedTime, ActualTime FROM Route";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    dataAdapter.Fill(dataTable);
                    dgvRoutes.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні маршрутів: " + ex.Message);
            }
        }

        private void ShowInputDialog(string title, DataGridView dgvRoutes, bool isAdd)
        {
            Form inputForm = new Form
            {
                Text = title,
                Size = new System.Drawing.Size(400, 400)
            };

            TextBox txtRouteID = new TextBox { Location = new System.Drawing.Point(150, 20), Width = 200 };
            TextBox txtStartPoint = new TextBox { Location = new System.Drawing.Point(150, 60), Width = 200 };
            TextBox txtEndPoint = new TextBox { Location = new System.Drawing.Point(150, 100), Width = 200 };
            TextBox txtDistanceKM = new TextBox { Location = new System.Drawing.Point(150, 140), Width = 200 };
            TextBox txtEstimatedTime = new TextBox { Location = new System.Drawing.Point(150, 180), Width = 200 };
            TextBox txtActualTime = new TextBox { Location = new System.Drawing.Point(150, 220), Width = 200 };

            inputForm.Controls.Add(new Label { Text = "RouteID:", Location = new System.Drawing.Point(20, 20) });
            inputForm.Controls.Add(new Label { Text = "StartPoint:", Location = new System.Drawing.Point(20, 60) });
            inputForm.Controls.Add(new Label { Text = "EndPoint:", Location = new System.Drawing.Point(20, 100) });
            inputForm.Controls.Add(new Label { Text = "DistanceKM:", Location = new System.Drawing.Point(20, 140) });
            inputForm.Controls.Add(new Label { Text = "EstimatedTime:", Location = new System.Drawing.Point(20, 180) });
            inputForm.Controls.Add(new Label { Text = "ActualTime:", Location = new System.Drawing.Point(20, 220) });

            inputForm.Controls.Add(txtRouteID);
            inputForm.Controls.Add(txtStartPoint);
            inputForm.Controls.Add(txtEndPoint);
            inputForm.Controls.Add(txtDistanceKM);
            inputForm.Controls.Add(txtEstimatedTime);
            inputForm.Controls.Add(txtActualTime);

            if (!isAdd && dgvRoutes.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvRoutes.SelectedRows[0];
                txtRouteID.Text = row.Cells["RouteID"].Value.ToString();
                txtStartPoint.Text = row.Cells["StartPoint"].Value.ToString();
                txtEndPoint.Text = row.Cells["EndPoint"].Value.ToString();
                txtDistanceKM.Text = row.Cells["DistanceKM"].Value.ToString();
                txtEstimatedTime.Text = row.Cells["EstimatedTime"].Value.ToString();
                txtActualTime.Text = row.Cells["ActualTime"].Value.ToString();
                txtRouteID.ReadOnly = true; 
            }

            Button btnSave = new Button { Text = "Зберегти", Location = new System.Drawing.Point(150, 280) };
            btnSave.Click += (s, e) =>
            {
                try
                {
                    string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                    string query;

                    if (isAdd)
                    {
                        query = "INSERT INTO Route (RouteID, StartPoint, EndPoint, DistanceKM, EstimatedTime, ActualTime) " +
                                "VALUES (@RouteID, @StartPoint, @EndPoint, @DistanceKM, @EstimatedTime, @ActualTime)";
                    }
                    else
                    {
                        query = "UPDATE Route SET StartPoint = @StartPoint, EndPoint = @EndPoint, DistanceKM = @DistanceKM, " +
                                "EstimatedTime = @EstimatedTime, ActualTime = @ActualTime WHERE RouteID = @RouteID";
                    }

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@RouteID", txtRouteID.Text);
                        command.Parameters.AddWithValue("@StartPoint", txtStartPoint.Text);
                        command.Parameters.AddWithValue("@EndPoint", txtEndPoint.Text);
                        command.Parameters.AddWithValue("@DistanceKM", txtDistanceKM.Text);
                        command.Parameters.AddWithValue("@EstimatedTime", txtEstimatedTime.Text);
                        command.Parameters.AddWithValue("@ActualTime", txtActualTime.Text);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show(isAdd ? "Маршрут додано!" : "Маршрут оновлено!");
                    LoadRoutes(dgvRoutes);
                    inputForm.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка: " + ex.Message);
                }
            };

            inputForm.Controls.Add(btnSave);
            inputForm.ShowDialog();
        }

        private void DeleteRoute(DataGridView dgvRoutes)
        {
            if (dgvRoutes.SelectedRows.Count > 0)
            {
                int routeID = (int)dgvRoutes.SelectedRows[0].Cells["RouteID"].Value;
                DialogResult result = MessageBox.Show("Ви дійсно хочете видалити маршрут?", "Підтвердження", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                        string query = "DELETE FROM Route WHERE RouteID = @RouteID";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@RouteID", routeID);
                            connection.Open();
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Маршрут видалено!");
                        LoadRoutes(dgvRoutes);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при видаленні: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Виберіть маршрут для видалення.");
            }
        }
    }
}
