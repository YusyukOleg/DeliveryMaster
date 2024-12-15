using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DeliveryMaster
{
    public partial class ReportsForm : Form
    {
        private TextBox txtCourierID;
        private Label lblCourierID;
        private Button btnGenerateReport;
        private DataGridView dgvReports;

        public ReportsForm()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            lblCourierID = new Label();
            lblCourierID.Text = "Введіть ID кур'єра:";
            lblCourierID.Location = new System.Drawing.Point(10, 10);
            lblCourierID.AutoSize = true;

            txtCourierID = new TextBox();
            txtCourierID.Location = new System.Drawing.Point(150, 10);
            txtCourierID.Size = new System.Drawing.Size(200, 20);

            btnGenerateReport = new Button();
            btnGenerateReport.Text = "Генерувати звіт";
            btnGenerateReport.Location = new System.Drawing.Point(370, 10);
            btnGenerateReport.Click += BtnGenerateReport_Click;

            dgvReports = new DataGridView();
            dgvReports.Location = new System.Drawing.Point(10, 50);
            dgvReports.Size = new System.Drawing.Size(800, 400);

            this.Controls.Add(lblCourierID);
            this.Controls.Add(txtCourierID);
            this.Controls.Add(btnGenerateReport);
            this.Controls.Add(dgvReports);
        }

        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCourierID.Text))
            {
                MessageBox.Show("Будь ласка, введіть ID кур'єра.");
                return;
            }

            int courierID;
            if (!int.TryParse(txtCourierID.Text, out courierID))
            {
                MessageBox.Show("ID кур'єра має бути числом.");
                return;
            }

            string connectionString = "Server=OLEG\\SQLEXPRESS;Database=DeliveryMaster;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            C.FirstName + ' ' + C.LastName AS CourierName,
                            C.PhoneNumber AS CourierPhone,
                            C.PerformanceMetric,
                            O.OrderID,
                            O.Status,
                            O.TotalAmount,
                            Cl.FirstName + ' ' + Cl.LastName AS ClientName,
                            Cl.PhoneNumber AS ClientPhone,
                            R.StartPoint,
                            R.EndPoint,
                            R.DistanceKM,
                            R.ActualTime
                        FROM Courier C
                        LEFT JOIN [Order] O ON C.CourierID = O.CourierID
                        LEFT JOIN Route R ON O.RouteID = R.RouteID
                        LEFT JOIN Client Cl ON O.ClientID = Cl.ClientID
                        WHERE C.CourierID = @CourierID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CourierID", courierID);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dgvReports.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("Дані для цього кур'єра не знайдені.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка: " + ex.Message);
                }
            }
        }
    }
}
