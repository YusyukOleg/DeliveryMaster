using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DeliveryMaster
{
    public partial class OrdersForm : Form
    {
        private SqlConnection connection;

        private DataGridView dgvOrders;
        private TextBox txtOrderID, txtClientID, txtCourierID, txtRouteID, txtOrderDateTime, txtStatus, txtTotalAmount, txtProductID;
        private Button btnAdd, btnUpdate, btnDelete;

        public OrdersForm()
        {
            this.Text = "Управління Замовленнями";
            this.Size = new System.Drawing.Size(900, 600);

            InitializeCustomComponents(); 

            connection = new SqlConnection("Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;");
            LoadOrders();
        }

        private void InitializeCustomComponents()
        {
            dgvOrders = new DataGridView { Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(860, 300) };
            dgvOrders.SelectionChanged += DgvOrders_SelectionChanged;

            Label lblOrderID = new Label { Text = "Номер замовлення:", Location = new System.Drawing.Point(10, 320), AutoSize = true };
            txtOrderID = new TextBox { Location = new System.Drawing.Point(150, 320), Width = 150 };

            Label lblClientID = new Label { Text = "Номер клієнта:", Location = new System.Drawing.Point(10, 360), AutoSize = true };
            txtClientID = new TextBox { Location = new System.Drawing.Point(150, 360), Width = 150 };

            Label lblCourierID = new Label { Text = "Номер кур'єра:", Location = new System.Drawing.Point(10, 400), AutoSize = true };
            txtCourierID = new TextBox { Location = new System.Drawing.Point(150, 400), Width = 150 };

            Label lblRouteID = new Label { Text = "Номер маршруту:", Location = new System.Drawing.Point(300, 320), AutoSize = true };
            txtRouteID = new TextBox { Location = new System.Drawing.Point(420, 320), Width = 150 };

            Label lblOrderDateTime = new Label { Text = "Дата та час:", Location = new System.Drawing.Point(300, 360), AutoSize = true };
            txtOrderDateTime = new TextBox { Location = new System.Drawing.Point(420, 360), Width = 150 };

            Label lblStatus = new Label { Text = "Статус:", Location = new System.Drawing.Point(300, 400), AutoSize = true };
            txtStatus = new TextBox { Location = new System.Drawing.Point(420, 400), Width = 150 };

            Label lblTotalAmount = new Label { Text = "Загальна сума:", Location = new System.Drawing.Point(580, 320), AutoSize = true };
            txtTotalAmount = new TextBox { Location = new System.Drawing.Point(700, 320), Width = 150, ReadOnly = true };

            Label lblProductID = new Label { Text = "Продукт ID:", Location = new System.Drawing.Point(580, 360), AutoSize = true };
            txtProductID = new TextBox { Location = new System.Drawing.Point(700, 360), Width = 150 };

            btnAdd = new Button { Text = "Додати", Location = new System.Drawing.Point(580, 400), Size = new System.Drawing.Size(100, 30) };
            btnAdd.Click += BtnAdd_Click;

            btnUpdate = new Button { Text = "Оновити", Location = new System.Drawing.Point(690, 400), Size = new System.Drawing.Size(100, 30) };
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button { Text = "Видалити", Location = new System.Drawing.Point(800, 400), Size = new System.Drawing.Size(100, 30) };
            btnDelete.Click += BtnDelete_Click;

            this.Controls.Add(dgvOrders);
            this.Controls.Add(lblOrderID);
            this.Controls.Add(txtOrderID);
            this.Controls.Add(lblClientID);
            this.Controls.Add(txtClientID);
            this.Controls.Add(lblCourierID);
            this.Controls.Add(txtCourierID);
            this.Controls.Add(lblRouteID);
            this.Controls.Add(txtRouteID);
            this.Controls.Add(lblOrderDateTime);
            this.Controls.Add(txtOrderDateTime);
            this.Controls.Add(lblStatus);
            this.Controls.Add(txtStatus);
            this.Controls.Add(lblTotalAmount);
            this.Controls.Add(txtTotalAmount);
            this.Controls.Add(lblProductID);
            this.Controls.Add(txtProductID);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
        }

        private void LoadOrders()
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [Order]", connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvOrders.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                connection.Open();

                using (SqlCommand command = new SqlCommand(
                    "INSERT INTO [Order] (OrderID, ClientID, CourierID, RouteID, OrderDateTime, Status, ProductID) VALUES (@OrderID, @ClientID, @CourierID, @RouteID, @OrderDateTime, @Status, @ProductID)",
                    connection))
                {
                    command.Parameters.AddWithValue("@OrderID", int.Parse(txtOrderID.Text));
                    command.Parameters.AddWithValue("@ClientID", int.Parse(txtClientID.Text));
                    command.Parameters.AddWithValue("@CourierID", int.Parse(txtCourierID.Text));
                    command.Parameters.AddWithValue("@RouteID", int.Parse(txtRouteID.Text));
                    command.Parameters.AddWithValue("@OrderDateTime", DateTime.Parse(txtOrderDateTime.Text));
                    command.Parameters.AddWithValue("@Status", txtStatus.Text);
                    command.Parameters.AddWithValue("@ProductID", int.Parse(txtProductID.Text));

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Замовлення успішно додано.");
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                connection.Open();

                using (SqlCommand command = new SqlCommand(
                    "UPDATE [Order] SET ClientID = @ClientID, CourierID = @CourierID, RouteID = @RouteID, OrderDateTime = @OrderDateTime, Status = @Status, ProductID = @ProductID WHERE OrderID = @OrderID",
                    connection))
                {
                    command.Parameters.AddWithValue("@OrderID", int.Parse(txtOrderID.Text));
                    command.Parameters.AddWithValue("@ClientID", int.Parse(txtClientID.Text));
                    command.Parameters.AddWithValue("@CourierID", int.Parse(txtCourierID.Text));
                    command.Parameters.AddWithValue("@RouteID", int.Parse(txtRouteID.Text));
                    command.Parameters.AddWithValue("@OrderDateTime", DateTime.Parse(txtOrderDateTime.Text));
                    command.Parameters.AddWithValue("@Status", txtStatus.Text);
                    command.Parameters.AddWithValue("@ProductID", int.Parse(txtProductID.Text));

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Замовлення оновлено.");
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                connection.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM [Order] WHERE OrderID = @OrderID", connection))
                {
                    command.Parameters.AddWithValue("@OrderID", int.Parse(txtOrderID.Text));
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Замовлення видалено.");
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void DgvOrders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count > 0)
            {
                txtOrderID.Text = dgvOrders.CurrentRow.Cells["OrderID"].Value.ToString();
                txtClientID.Text = dgvOrders.CurrentRow.Cells["ClientID"].Value.ToString();
                txtCourierID.Text = dgvOrders.CurrentRow.Cells["CourierID"].Value.ToString();
                txtRouteID.Text = dgvOrders.CurrentRow.Cells["RouteID"].Value.ToString();
                txtOrderDateTime.Text = dgvOrders.CurrentRow.Cells["OrderDateTime"].Value.ToString();
                txtStatus.Text = dgvOrders.CurrentRow.Cells["Status"].Value.ToString();
                txtProductID.Text = dgvOrders.CurrentRow.Cells["ProductID"].Value.ToString();

                int orderID = int.Parse(dgvOrders.CurrentRow.Cells["OrderID"].Value.ToString());
                GetTotalAmount(orderID);
            }
        }

        private void GetTotalAmount(int orderID)
        {
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                connection.Open();

                SqlCommand command = new SqlCommand("SELECT TotalAmount FROM [Order] WHERE OrderID = @OrderID", connection);
                command.Parameters.AddWithValue("@OrderID", orderID);

                var result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    txtTotalAmount.Text = result.ToString();
                }
                else
                {
                    txtTotalAmount.Text = "0";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
