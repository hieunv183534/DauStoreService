using DauStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Interfaces.IRepositories
{
    public interface IVoucherRepository : IBaseRepository<Voucher>
    {
        Object GetVouchers(string searchTerms, int index, int count);
    }
}
