using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using DauStore.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DauStore.Core.Services
{
    public class VoucherService : BaseService<Voucher>, IVoucherService
    {

        protected IVoucherRepository _voucherRepository;

        public VoucherService(IBaseRepository<Voucher> baseRepository, IVoucherRepository voucherRepository) : base(baseRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public ServiceResult GetVouchers(int canuseState, string searchTerms, int index, int count)
        {
            try
            {
                searchTerms = (searchTerms == null) ? "" : searchTerms;
                List<Voucher> vouchers = _voucherRepository.GetAll();
                // lọc theo mã còn sử dụng hay không sử dụng được
                if (canuseState == 1)
                {
                    // lấy các voucher vừa còn hạn vừa còn quota
                    vouchers = (from voucher in vouchers where (voucher.Quota > 0) && (voucher.DateExpired > DateTime.Now) select voucher).ToList();
                }
                else if (canuseState == 2)
                {
                    // lấy các voucher hoặc hết hạn hoặc hết quota
                    vouchers = (from voucher in vouchers where (voucher.Quota <= 0) || (voucher.DateExpired <= DateTime.Now) select voucher).ToList();
                }

                vouchers = (from voucher in vouchers where (voucher.VoucherCode.Contains(searchTerms)) || (voucher.Description.Contains(searchTerms)) select voucher).ToList();
                var result = new
                {
                    total = vouchers.Count,
                    data = vouchers.Skip(index).Take(count).ToList()
                };

                if (vouchers.Count > 0)
                {
                    _serviceResult.Response = new ResponseModel(2000, "Ok", result);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(2004, "Không có bản ghi nào!", result);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }
    }
}
