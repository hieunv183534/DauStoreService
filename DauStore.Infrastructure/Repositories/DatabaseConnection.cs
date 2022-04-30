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
            get { return new MySqlConnection("Host=MYSQL8001.site4now.net ;Port=3306 ;Database=db_a85e4f_store ; User Id=a85e4f_store; Password=Vnvd8788"); }
        }
    }
}