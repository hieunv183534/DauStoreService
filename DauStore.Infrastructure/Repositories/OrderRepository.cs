using Dapper;
using DauStore.Core.Entities;
using DauStore.Core.Enums;
using DauStore.Core.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DauStore.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public Object GetOrders(OrderStatusEnum orderStatus, string searchTerms, double startTime, double endTime, int orderTimeState, int index, int count)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@OrderStatus", orderStatus);
                parameters.Add("@SearchTerms", searchTerms);
                parameters.Add("@StartTime", startTime);
                parameters.Add("@EndTime", endTime);
                parameters.Add("@OrderTimeState", orderTimeState);
                parameters.Add("@Indexx", index);
                parameters.Add("@Count", count);
                parameters.Add("Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var procName = $"Proc_GetOrders";

                var orders = dbConnection.Query<Order>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                var total = parameters.Get<int>("Total");
                return new
                {
                    total = total,
                    data = orders
                };
            }
        }

        public List<Order> LookupOrder(string key)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@KeyString", key);
                var procName = $"Proc_LookupOrder";
                var orders = dbConnection.Query<Order>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return (List<Order>)orders;
            }
        }
    }
}
