using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DeliveryMaster
{
    public partial class ProductsForm : Form
    {
        public ProductsForm()
        {
            InitializeComponent();
            CreateUI();
        }

        private void CreateUI()
        {
            this.Text = "Продукти";
            this.Size = new System.Drawing.Size(600, 400);

            DataGridView dgvProducts = new DataGridView
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(560, 300),
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            this.Controls.Add(dgvProducts);

            Button btnAdd = new Button { Text = "Додати", Location = new System.Drawing.Point(10, 320) };
            Button btnEdit = new Button { Text = "Редагувати", Location = new System.Drawing.Point(110, 320) };
            Button btnDelete = new Button { Text = "Видалити", Location = new System.Drawing.Point(210, 320) };
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);

            btnAdd.Click += (s, e) => AddProduct(dgvProducts);
            btnEdit.Click += (s, e) => EditProduct(dgvProducts);
            btnDelete.Click += (s, e) => DeleteProduct(dgvProducts);

            LoadProducts(dgvProducts);
        }

        private void LoadProducts(DataGridView dgvProducts)
        {
            try
            {
                string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                string query = "SELECT ProductID, ProductName, Category, Price, Description, Quantity FROM Product";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    dataAdapter.Fill(dataTable);
                    dgvProducts.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні продуктів: " + ex.Message);
            }
        }

        private void AddProduct(DataGridView dgvProducts)
        {
            Form inputForm = new Form();
            inputForm.Text = "Додавання продукту";
            inputForm.Size = new System.Drawing.Size(300, 250);

            TextBox txtProductID = new TextBox { Location = new System.Drawing.Point(120, 10), Width = 150 };
            TextBox txtProductName = new TextBox { Location = new System.Drawing.Point(120, 40), Width = 150 };
            TextBox txtCategory = new TextBox { Location = new System.Drawing.Point(120, 70), Width = 150 };
            TextBox txtPrice = new TextBox { Location = new System.Drawing.Point(120, 100), Width = 150 };
            TextBox txtDescription = new TextBox { Location = new System.Drawing.Point(120, 130), Width = 150 };
            TextBox txtQuantity = new TextBox { Location = new System.Drawing.Point(120, 160), Width = 150 };

            inputForm.Controls.Add(new Label { Text = "Product ID", Location = new System.Drawing.Point(10, 10) });
            inputForm.Controls.Add(txtProductID);
            inputForm.Controls.Add(new Label { Text = "Назва", Location = new System.Drawing.Point(10, 40) });
            inputForm.Controls.Add(txtProductName);
            inputForm.Controls.Add(new Label { Text = "Категорія", Location = new System.Drawing.Point(10, 70) });
            inputForm.Controls.Add(txtCategory);
            inputForm.Controls.Add(new Label { Text = "Ціна", Location = new System.Drawing.Point(10, 100) });
            inputForm.Controls.Add(txtPrice);
            inputForm.Controls.Add(new Label { Text = "Опис", Location = new System.Drawing.Point(10, 130) });
            inputForm.Controls.Add(txtDescription);
            inputForm.Controls.Add(new Label { Text = "Кількість", Location = new System.Drawing.Point(10, 160) });
            inputForm.Controls.Add(txtQuantity);

            Button btnSave = new Button { Text = "Зберегти", Location = new System.Drawing.Point(120, 190) };
            inputForm.Controls.Add(btnSave);

            btnSave.Click += (s, e) =>
            {
                string productID = txtProductID.Text;
                string productName = txtProductName.Text;
                string category = txtCategory.Text;
                string price = txtPrice.Text;
                string description = txtDescription.Text;
                string quantity = txtQuantity.Text;

                if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(quantity))
                {
                    MessageBox.Show("Усі поля повинні бути заповнені.");
                    return;
                }

                try
                {
                    string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                    string query = "INSERT INTO Product (ProductID, ProductName, Category, Price, Description, Quantity) VALUES (@ProductID, @ProductName, @Category, @Price, @Description, @Quantity)";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@ProductID", productID);
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Quantity", quantity);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Продукт успішно додано!");
                    LoadProducts(dgvProducts); 
                    inputForm.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при додаванні продукту: " + ex.Message);
                }
            };

            inputForm.ShowDialog();
        }

        private void EditProduct(DataGridView dgvProducts)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dgvProducts.SelectedRows[0].Index;
                int productId = (int)dgvProducts.Rows[selectedRowIndex].Cells["ProductID"].Value;

                Form inputForm = new Form();
                inputForm.Text = "Редагування продукту";
                inputForm.Size = new System.Drawing.Size(300, 250);

                TextBox txtProductID = new TextBox { Text = productId.ToString(), Location = new System.Drawing.Point(120, 10), Width = 150, ReadOnly = true };
                TextBox txtProductName = new TextBox { Location = new System.Drawing.Point(120, 40), Width = 150 };
                TextBox txtCategory = new TextBox { Location = new System.Drawing.Point(120, 70), Width = 150 };
                TextBox txtPrice = new TextBox { Location = new System.Drawing.Point(120, 100), Width = 150 };
                TextBox txtDescription = new TextBox { Location = new System.Drawing.Point(120, 130), Width = 150 };
                TextBox txtQuantity = new TextBox { Location = new System.Drawing.Point(120, 160), Width = 150 };

                inputForm.Controls.Add(new Label { Text = "Product ID", Location = new System.Drawing.Point(10, 10) });
                inputForm.Controls.Add(txtProductID);
                inputForm.Controls.Add(new Label { Text = "Назва", Location = new System.Drawing.Point(10, 40) });
                inputForm.Controls.Add(txtProductName);
                inputForm.Controls.Add(new Label { Text = "Категорія", Location = new System.Drawing.Point(10, 70) });
                inputForm.Controls.Add(txtCategory);
                inputForm.Controls.Add(new Label { Text = "Ціна", Location = new System.Drawing.Point(10, 100) });
                inputForm.Controls.Add(txtPrice);
                inputForm.Controls.Add(new Label { Text = "Опис", Location = new System.Drawing.Point(10, 130) });
                inputForm.Controls.Add(txtDescription);
                inputForm.Controls.Add(new Label { Text = "Кількість", Location = new System.Drawing.Point(10, 160) });
                inputForm.Controls.Add(txtQuantity);

                Button btnSave = new Button { Text = "Зберегти", Location = new System.Drawing.Point(120, 190) };
                inputForm.Controls.Add(btnSave);

                string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                string query = "SELECT * FROM Product WHERE ProductID = @ProductID";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ProductID", productId);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        txtProductName.Text = reader["ProductName"].ToString();
                        txtCategory.Text = reader["Category"].ToString();
                        txtPrice.Text = reader["Price"].ToString();
                        txtDescription.Text = reader["Description"].ToString();
                        txtQuantity.Text = reader["Quantity"].ToString();
                    }
                }

                btnSave.Click += (s, e) =>
                {
                    string productName = txtProductName.Text;
                    string category = txtCategory.Text;
                    string price = txtPrice.Text;
                    string description = txtDescription.Text;
                    string quantity = txtQuantity.Text;

                    if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(quantity))
                    {
                        MessageBox.Show("Усі поля повинні бути заповнені.");
                        return;
                    }

                    try
                    {
                        string updateQuery = "UPDATE Product SET ProductName = @ProductName, Category = @Category, Price = @Price, Description = @Description, Quantity = @Quantity WHERE ProductID = @ProductID";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(updateQuery, connection);
                            command.Parameters.AddWithValue("@ProductID", productId);
                            command.Parameters.AddWithValue("@ProductName", productName);
                            command.Parameters.AddWithValue("@Category", category);
                            command.Parameters.AddWithValue("@Price", price);
                            command.Parameters.AddWithValue("@Description", description);
                            command.Parameters.AddWithValue("@Quantity", quantity);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Продукт успішно оновлено!");
                        LoadProducts(dgvProducts);
                        inputForm.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при оновленні продукту: " + ex.Message);
                    }
                };

                inputForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть продукт для редагування.");
            }
        }

        private void DeleteProduct(DataGridView dgvProducts)
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                int selectedRowIndex = dgvProducts.SelectedRows[0].Index;
                int productId = (int)dgvProducts.Rows[selectedRowIndex].Cells["ProductID"].Value;

                DialogResult result = MessageBox.Show("Ви дійсно хочете видалити цей продукт?", "Підтвердження", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
                        string query = "DELETE FROM Product WHERE ProductID = @ProductID";

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@ProductID", productId);
                            connection.Open();
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Продукт успішно видалено!");
                        LoadProducts(dgvProducts);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при видаленні продукту: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть продукт для видалення.");
            }
        }
    }
}
