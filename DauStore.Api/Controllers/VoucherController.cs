using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using DauStore.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DauStore.Api.Controllers
{
    [Authorize(Roles = "admin, customer")]
    [Route("api")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        protected IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getVouchers")]
        public IActionResult GetVouchers([FromQuery] string searchTerms, [FromQuery] int index, [FromQuery] int count)
        {
            var serviceResult = _voucherService.GetVouchers(searchTerms, index, count);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("addVoucher")]
        public IActionResult AddVoucher([FromBody] Voucher voucher)
        {
            var serviceResult = _voucherService.Add(voucher);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("updateVoucher/{voucherId}")]
        public IActionResult UpdateVoucher([FromBody] Voucher voucher, [FromRoute] Guid voucherId)
        {
            var serviceResult = _voucherService.Update(voucher, voucherId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("deleteVoucher/{voucherId}")]
        public IActionResult DeleteVoucher( [FromRoute] Guid voucherId)
        {
            var serviceResult = _voucherService.Delete(voucherId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [AllowAnonymous]
        [HttpGet("getVoucherByCode/{voucherCode}")]
        public IActionResult GetVoucherByCode([FromRoute] string voucherCode)
        {
            var serviceResult = _voucherService.GetByProp("VoucherCode",voucherCode);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }
    }
}
