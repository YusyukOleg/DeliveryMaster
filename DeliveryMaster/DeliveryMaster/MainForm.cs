using System;
using System.Windows.Forms;

namespace DeliveryMaster
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            CreateUI();
        }

        private void CreateUI()
        {
            this.Text = "DeliveryMaster - Головне меню";
            this.Size = new System.Drawing.Size(800, 600);

            MenuStrip menuStrip = new MenuStrip();
            this.MainMenuStrip = menuStrip;

            ToolStripMenuItem clientsMenu = new ToolStripMenuItem("Клієнти");
            clientsMenu.Click += (s, e) => OpenClientsForm();

            ToolStripMenuItem couriersMenu = new ToolStripMenuItem("Кур'єри");
            couriersMenu.Click += (s, e) => OpenCouriersForm();

            ToolStripMenuItem routesMenu = new ToolStripMenuItem("Маршрути");
            routesMenu.Click += (s, e) => OpenRoutesForm();

            ToolStripMenuItem ordersMenu = new ToolStripMenuItem("Замовлення");
            ordersMenu.Click += (s, e) => OpenOrdersForm();

            ToolStripMenuItem productsMenu = new ToolStripMenuItem("Продукти");
            productsMenu.Click += (s, e) => OpenProductsForm();

            ToolStripMenuItem reportsMenu = new ToolStripMenuItem("Звіти");
            reportsMenu.Click += (s, e) => OpenReportsForm();

            menuStrip.Items.Add(clientsMenu);
            menuStrip.Items.Add(couriersMenu);
            menuStrip.Items.Add(routesMenu);
            menuStrip.Items.Add(ordersMenu);
            menuStrip.Items.Add(productsMenu);
            menuStrip.Items.Add(reportsMenu);

            this.Controls.Add(menuStrip);
        }

        private void OpenClientsForm()
        {
            ClientsForm clientsForm = new ClientsForm();
            clientsForm.Show();
        }

        private void OpenCouriersForm()
        {
            CouriersForm couriersForm = new CouriersForm();
            couriersForm.Show();
        }

        private void OpenRoutesForm()
        {
            RoutesForm routesForm = new RoutesForm();
            routesForm.Show();
        }

        private void OpenOrdersForm()
        {
            OrdersForm ordersForm = new OrdersForm();
            ordersForm.Show();
        }

        private void OpenProductsForm()
        {
            ProductsForm productsForm = new ProductsForm();
            productsForm.Show();
        }

        private void OpenReportsForm()
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.Show();
        }
    }
}