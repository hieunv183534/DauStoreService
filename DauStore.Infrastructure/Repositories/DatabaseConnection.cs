using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DauStore.Infrastructure.Repositories
{
    public class DatabaseConnection
    {
        /// <summary>
        /// Khởi tạo đối tương connector
        /// </summary>
        public static IDbConnection DbConnection
        {
            get { return new MySqlConnection("Host=MYSQL5038.site4now.net ;Port=3306 ;Database=db_a845ec_mystore ; User Id=a845ec_mystore; Password=Vnvd8788"); }
        }
    }
}