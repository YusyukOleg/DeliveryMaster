using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DeliveryMaster
{
    public static class DatabaseHelper
    {
        /// <summary>
        /// Отримати підключення до бази даних.
        /// </summary>
        /// <returns>Об'єкт SqlConnection</returns>
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DeliveryMaster"].ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Рядок підключення не знайдено у файлі конфігурації.");
            }

            return new SqlConnection(connectionString);
        }
    }
}
