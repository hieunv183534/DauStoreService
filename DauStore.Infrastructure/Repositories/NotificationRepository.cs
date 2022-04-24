using Dapper;
using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DauStore.Infrastructure.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public object GetNotifications(Guid accountId, int index, int count)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Indexx", index);
                parameters.Add("@Count", count);
                parameters.Add("@AccountId", accountId);
                parameters.Add("Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var procName = $"Proc_GetNotifications";

                var notifications = dbConnection.Query<Notification>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                var total = parameters.Get<int>("Total");
                return new { 
                              total = total,
                              data = notifications
                           };
            }
        }

        public int SeenAll(Guid accountId)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AccountId",accountId);
                var procName = $"Proc_SeenAllNotification";
                var rowAffect = dbConnection.Execute(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return rowAffect;
            }
        }
    }
}
