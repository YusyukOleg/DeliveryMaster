using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DeliveryMaster
{
    public partial class CouriersForm : Form
    {
        private string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
        private DataGridView dgvCouriers;
        private Panel panelInputs;
        private TextBox txtCourierID, txtFirstName, txtLastName, txtPhoneNumber, txtVehicleNumber, txtPerformanceMetric;
        private Button btnSave, btnCancel;

        public CouriersForm()
        {
            InitializeComponent();
            CreateUI();
            LoadCouriers();
        }

        private void CreateUI()
        {
            this.Text = "Кур'єри";
            this.Size = new System.Drawing.Size(800, 600);

            dgvCouriers = new DataGridView
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(760, 300),
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            this.Controls.Add(dgvCouriers);

            Button btnAdd = new Button { Text = "Додати", Location = new System.Drawing.Point(10, 330) };
            Button btnEdit = new Button { Text = "Редагувати", Location = new System.Drawing.Point(110, 330) };
            Button btnDelete = new Button { Text = "Видалити", Location = new System.Drawing.Point(210, 330) };
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);

            btnAdd.Click += BtnAdd_Click;
            btnEdit.Click += BtnEdit_Click;
            btnDelete.Click += BtnDelete_Click;

            panelInputs = new Panel
            {
                Size = new System.Drawing.Size(760, 200),
                Location = new System.Drawing.Point(10, 380),
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false
            };
            this.Controls.Add(panelInputs);

            panelInputs.Controls.Add(new Label { Text = "ID кур'єра:", Location = new System.Drawing.Point(10, 10) });
            txtCourierID = new TextBox { Location = new System.Drawing.Point(150, 10), Width = 200 };
            panelInputs.Controls.Add(txtCourierID);

            panelInputs.Controls.Add(new Label { Text = "Ім'я:", Location = new System.Drawing.Point(10, 40) });
            txtFirstName = new TextBox { Location = new System.Drawing.Point(150, 40), Width = 200 };
            panelInputs.Controls.Add(txtFirstName);

            panelInputs.Controls.Add(new Label { Text = "Прізвище:", Location = new System.Drawing.Point(10, 70) });
            txtLastName = new TextBox { Location = new System.Drawing.Point(150, 70), Width = 200 };
            panelInputs.Controls.Add(txtLastName);

            panelInputs.Controls.Add(new Label { Text = "Телефон:", Location = new System.Drawing.Point(10, 100) });
            txtPhoneNumber = new TextBox { Location = new System.Drawing.Point(150, 100), Width = 200 };
            panelInputs.Controls.Add(txtPhoneNumber);

            panelInputs.Controls.Add(new Label { Text = "Номер ТЗ:", Location = new System.Drawing.Point(10, 130) });
            txtVehicleNumber = new TextBox { Location = new System.Drawing.Point(150, 130), Width = 200 };
            panelInputs.Controls.Add(txtVehicleNumber);

            panelInputs.Controls.Add(new Label { Text = "Продуктивність:", Location = new System.Drawing.Point(10, 160) });
            txtPerformanceMetric = new TextBox { Location = new System.Drawing.Point(150, 160), Width = 200 };
            panelInputs.Controls.Add(txtPerformanceMetric);

            btnSave = new Button { Text = "Зберегти", Location = new System.Drawing.Point(400, 10) };
            btnCancel = new Button { Text = "Скасувати", Location = new System.Drawing.Point(400, 50) };
            panelInputs.Controls.Add(btnSave);
            panelInputs.Controls.Add(btnCancel);

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += (s, e) => panelInputs.Visible = false;
        }

        private void LoadCouriers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Courier", connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvCouriers.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження кур'єрів: " + ex.Message);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            txtCourierID.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtPhoneNumber.Clear();
            txtVehicleNumber.Clear();
            txtPerformanceMetric.Clear();

            ShowInputPanel();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCouriers.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvCouriers.SelectedRows[0];
                txtCourierID.Text = row.Cells["CourierID"].Value.ToString();
                txtFirstName.Text = row.Cells["FirstName"].Value.ToString();
                txtLastName.Text = row.Cells["LastName"].Value.ToString();
                txtPhoneNumber.Text = row.Cells["PhoneNumber"].Value.ToString();
                txtVehicleNumber.Text = row.Cells["VehicleNumber"].Value.ToString();
                txtPerformanceMetric.Text = row.Cells["PerformanceMetric"].Value.ToString();

                ShowInputPanel();
            }
            else
            {
                MessageBox.Show("Виберіть кур'єра для редагування.");
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCouriers.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow row = dgvCouriers.SelectedRows[0];
                    string courierID = row.Cells["CourierID"].Value.ToString();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("DELETE FROM Courier WHERE CourierID = @CourierID", connection);
                        command.Parameters.AddWithValue("@CourierID", courierID);
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Кур'єра видалено успішно!");
                    LoadCouriers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка видалення кур'єра: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Виберіть кур'єра для видалення.");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) || string.IsNullOrWhiteSpace(txtLastName.Text) || string.IsNullOrWhiteSpace(txtPhoneNumber.Text) || string.IsNullOrWhiteSpace(txtPerformanceMetric.Text))
            {
                MessageBox.Show("Всі поля, окрім номера транспорту, повинні бути заповнені.");
                return;
            }

            if (!decimal.TryParse(txtPerformanceMetric.Text, out decimal performanceMetric))
            {
                MessageBox.Show("Продуктивність повинна бути числом.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command;

                    if (string.IsNullOrEmpty(txtCourierID.Text))
                    {
                        command = new SqlCommand("INSERT INTO Courier (FirstName, LastName, PhoneNumber, VehicleNumber, PerformanceMetric) " +
                                                 "VALUES (@FirstName, @LastName, @PhoneNumber, @VehicleNumber, @PerformanceMetric)", connection);
                    }
                    else 
                    {
                        command = new SqlCommand("INSERT INTO Courier (CourierID, FirstName, LastName, PhoneNumber, VehicleNumber, PerformanceMetric) " +
                                                 "VALUES (@CourierID, @FirstName, @LastName, @PhoneNumber, @VehicleNumber, @PerformanceMetric)", connection);
                        command.Parameters.AddWithValue("@CourierID", txtCourierID.Text); 
                    }

                    command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    command.Parameters.AddWithValue("@PhoneNumber", txtPhoneNumber.Text);
                    command.Parameters.AddWithValue("@VehicleNumber", string.IsNullOrWhiteSpace(txtVehicleNumber.Text) ? DBNull.Value : (object)txtVehicleNumber.Text);
                    command.Parameters.AddWithValue("@PerformanceMetric", performanceMetric);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Зміни збережено успішно!");
                panelInputs.Visible = false;
                LoadCouriers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка збереження даних: " + ex.Message);
            }
        }

        private void ShowInputPanel()
        {
            panelInputs.Visible = true;
        }
    }
}
