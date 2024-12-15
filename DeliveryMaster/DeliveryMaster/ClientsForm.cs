using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DeliveryMaster
{
    public partial class ClientsForm : Form
    {
        public ClientsForm()
        {
            InitializeComponent();
            CreateUI();
        }

        private void CreateUI()
        {
            this.Text = "Клієнти";
            this.Size = new System.Drawing.Size(600, 400);

            DataGridView dgvClients = new DataGridView
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(560, 300),
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            this.Controls.Add(dgvClients);

            Button btnAdd = new Button { Text = "Додати", Location = new System.Drawing.Point(10, 320) };
            Button btnEdit = new Button { Text = "Редагувати", Location = new System.Drawing.Point(110, 320) };
            Button btnDelete = new Button { Text = "Видалити", Location = new System.Drawing.Point(210, 320) };
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);

            btnAdd.Click += (s, e) => AddClient(dgvClients);
            btnEdit.Click += (s, e) => EditClient(dgvClients);
            btnDelete.Click += (s, e) => DeleteClient(dgvClients);

            LoadClients(dgvClients);
        }

        private void LoadClients(DataGridView dgvClients)
        {
            try
            {
                string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                string query = "SELECT ClientID, FirstName, LastName, PhoneNumber, DeliveryAddress FROM Client";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    dataAdapter.Fill(dataTable);
                    dgvClients.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні клієнтів: " + ex.Message);
            }
        }

        private void AddClient(DataGridView dgvClients)
        {
            Form inputForm = new Form();
            inputForm.Text = "Додавання клієнта";
            inputForm.Size = new System.Drawing.Size(300, 250);

            TextBox txtClientID = new TextBox { Location = new System.Drawing.Point(120, 10), Width = 150 };
            TextBox txtFirstName = new TextBox { Location = new System.Drawing.Point(120, 40), Width = 150 };
            TextBox txtLastName = new TextBox { Location = new System.Drawing.Point(120, 70), Width = 150 };
            TextBox txtPhoneNumber = new TextBox { Location = new System.Drawing.Point(120, 100), Width = 150 };
            TextBox txtDeliveryAddress = new TextBox { Location = new System.Drawing.Point(120, 130), Width = 150 };

            inputForm.Controls.Add(new Label { Text = "Client ID", Location = new System.Drawing.Point(10, 10) });
            inputForm.Controls.Add(txtClientID);
            inputForm.Controls.Add(new Label { Text = "Ім'я", Location = new System.Drawing.Point(10, 40) });
            inputForm.Controls.Add(txtFirstName);
            inputForm.Controls.Add(new Label { Text = "Прізвище", Location = new System.Drawing.Point(10, 70) });
            inputForm.Controls.Add(txtLastName);
            inputForm.Controls.Add(new Label { Text = "Телефон", Location = new System.Drawing.Point(10, 100) });
            inputForm.Controls.Add(txtPhoneNumber);
            inputForm.Controls.Add(new Label { Text = "Адреса", Location = new System.Drawing.Point(10, 130) });
            inputForm.Controls.Add(txtDeliveryAddress);

            Button btnSave = new Button { Text = "Зберегти", Location = new System.Drawing.Point(120, 160) };
            inputForm.Controls.Add(btnSave);

            btnSave.Click += (s, e) =>
            {
                string clientID = txtClientID.Text;
                string firstName = txtFirstName.Text;
                string lastName = txtLastName.Text;
                string phoneNumber = txtPhoneNumber.Text;
                string deliveryAddress = txtDeliveryAddress.Text;

                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(deliveryAddress))
                {
                    MessageBox.Show("Усі поля повинні бути заповнені.");
                    return;
                }

                try
                {
                    string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                    string query = "INSERT INTO Client (ClientID, FirstName, LastName, PhoneNumber, DeliveryAddress) VALUES (@ClientID, @FirstName, @LastName, @PhoneNumber, @DeliveryAddress)";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ClientID", clientID);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                        command.Parameters.AddWithValue("@DeliveryAddress", deliveryAddress);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Клієнта успішно додано!");
                    LoadClients(dgvClients);
                    inputForm.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при додаванні клієнта: " + ex.Message);
                }
            };

            inputForm.ShowDialog();
        }

        private void EditClient(DataGridView dgvClients)
        {
            if (dgvClients.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dgvClients.SelectedRows[0].Index;
                int clientId = (int)dgvClients.Rows[selectedRowIndex].Cells["ClientID"].Value;

                Form inputForm = new Form();
                inputForm.Text = "Редагування клієнта";
                inputForm.Size = new System.Drawing.Size(300, 250);

                TextBox txtClientID = new TextBox { Text = clientId.ToString(), Location = new System.Drawing.Point(120, 10), Width = 150, ReadOnly = true };
                TextBox txtFirstName = new TextBox { Location = new System.Drawing.Point(120, 40), Width = 150 };
                TextBox txtLastName = new TextBox { Location = new System.Drawing.Point(120, 70), Width = 150 };
                TextBox txtPhoneNumber = new TextBox { Location = new System.Drawing.Point(120, 100), Width = 150 };
                TextBox txtDeliveryAddress = new TextBox { Location = new System.Drawing.Point(120, 130), Width = 150 };

                inputForm.Controls.Add(new Label { Text = "Client ID", Location = new System.Drawing.Point(10, 10) });
                inputForm.Controls.Add(txtClientID);
                inputForm.Controls.Add(new Label { Text = "Ім'я", Location = new System.Drawing.Point(10, 40) });
                inputForm.Controls.Add(txtFirstName);
                inputForm.Controls.Add(new Label { Text = "Прізвище", Location = new System.Drawing.Point(10, 70) });
                inputForm.Controls.Add(txtLastName);
                inputForm.Controls.Add(new Label { Text = "Телефон", Location = new System.Drawing.Point(10, 100) });
                inputForm.Controls.Add(txtPhoneNumber);
                inputForm.Controls.Add(new Label { Text = "Адреса", Location = new System.Drawing.Point(10, 130) });
                inputForm.Controls.Add(txtDeliveryAddress);

                Button btnSave = new Button { Text = "Зберегти", Location = new System.Drawing.Point(120, 160) };
                inputForm.Controls.Add(btnSave);

                btnSave.Click += (s, e) =>
                {
                    string firstName = txtFirstName.Text;
                    string lastName = txtLastName.Text;
                    string phoneNumber = txtPhoneNumber.Text;
                    string deliveryAddress = txtDeliveryAddress.Text;

                    if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(deliveryAddress))
                    {
                        MessageBox.Show("Усі поля повинні бути заповнені.");
                        return;
                    }

                    try
                    {
                        string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                        string query = "UPDATE Client SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, DeliveryAddress = @DeliveryAddress WHERE ClientID = @ClientID";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@FirstName", firstName);
                            command.Parameters.AddWithValue("@LastName", lastName);
                            command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            command.Parameters.AddWithValue("@DeliveryAddress", deliveryAddress);
                            command.Parameters.AddWithValue("@ClientID", clientId);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Дані клієнта успішно оновлено!");
                        LoadClients(dgvClients);
                        inputForm.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при оновленні клієнта: " + ex.Message);
                    }
                };

                inputForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть клієнта для редагування.");
            }
        }

        private void DeleteClient(DataGridView dgvClients)
        {
            if (dgvClients.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dgvClients.SelectedRows[0].Index;
                int clientId = (int)dgvClients.Rows[selectedRowIndex].Cells["ClientID"].Value;

                DialogResult dialogResult = MessageBox.Show("Ви дійсно хочете видалити цього клієнта?", "Підтвердження видалення", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                        string query = "DELETE FROM Client WHERE ClientID = @ClientID";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@ClientID", clientId);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Клієнта успішно видалено!");
                        LoadClients(dgvClients);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при видаленні клієнта: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть клієнта для видалення.");
            }
        }
    }
}
