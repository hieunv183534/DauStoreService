using DauStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Interfaces.IServices
{
    public interface IVoucherService : IBaseService<Voucher>
    {
        ServiceResult GetVouchers(string searchTerms, int index, int count);
    }
}
