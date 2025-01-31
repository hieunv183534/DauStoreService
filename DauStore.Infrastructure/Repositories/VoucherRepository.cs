﻿using Dapper;
using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DauStore.Infrastructure.Repositories
{
    public class VoucherRepository : BaseRepository<Voucher>, IVoucherRepository
    {
        //public List<Voucher> GetVouchers(string searchTerms, int index, int count)
        //{
        //    using (var dbConnection = DatabaseConnection.DbConnection)
        //    {
        //        DynamicParameters parameters = new DynamicParameters();
        //        parameters.Add("@SearchTerms", searchTerms);
        //        parameters.Add("@Indexx", index);
        //        parameters.Add("@Count", count);
        //        parameters.Add("Total", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //        var procName = $"Proc_GetVouchers";

        //        var vouchers = dbConnection.Query<Voucher>(procName, param: parameters, commandType: CommandType.StoredProcedure);
        //        var total = parameters.Get<int>("Total");
        //        return (List<Voucher>)vouchers;
        //    }
        //}
        public int UseVoucher(Guid voucherId)
        {
            using (var dbConnection = DatabaseConnection.DbConnection)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@VoucherId", voucherId);
                var procName = $"Proc_UseVoucher";

                int rows = dbConnection.Execute(procName, param: parameters, commandType: CommandType.StoredProcedure);
                return rows;
            }
        }
    }
}
