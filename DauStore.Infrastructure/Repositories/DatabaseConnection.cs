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
            get { return new MySqlConnection("Host=MYSQL8003.site4now.net ;Port=3306 ;Database=db_aa29e8_hehe ; User Id=aa29e8_hehe; Password=Vnvd8788"); }
        }
    }
}