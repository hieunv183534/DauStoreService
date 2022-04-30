using Dapper;
using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DauStore.Infrastructure.Repositories
{
    public class ItemRepository : BaseRepository<Item>, IItemRepository
    {
        public object GetItems(string categoryCode, string searchTerms, string tag, int orderState, int index, int count)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CategoryCode", categoryCode);
                parameters.Add("@SearchTerms", searchTerms);
                parameters.Add("@Tag", tag);
                parameters.Add("@OrderState", orderState);
                parameters.Add("@Indexx", index);
                parameters.Add("@Count", count);
                parameters.Add("Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
                var procName = $"Proc_GetItems";

                var items = dbConnection.Query<Item>(procName, param: parameters, commandType: CommandType.StoredProcedure);
                var total = parameters.Get<int>("Total");
                return new
                {
                    total = total,
                    data = items
                };
            }
        }
    }
}
