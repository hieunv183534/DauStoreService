using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using DauStore.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
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

        public ServiceResult GetVouchers(string searchTerms, int index, int count)
        {
            try
            {

                var result = _voucherRepository.GetVouchers(searchTerms, index, count);
                List<Voucher> data = (List<Voucher>)result.GetType().GetProperty("data").GetValue(result, null);
                if (data.Count > 0)
                {
                    _serviceResult.Response = new ResponseModel(2000, "Ok", result);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(2004, "Không có bản ghi nào!", result);
                    _serviceResult.StatusCode = 204;
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
