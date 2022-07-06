using DauStore.Api.Authentication;
using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace DauStore.Api.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Delare

        protected IBaseService<TokenAccount> _tokenAccountService;
        protected IBaseService<Account> _accountService;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        #endregion

        #region Consstructor

        public AccountController(IBaseService<TokenAccount> tokenAccountService, IBaseService<Account> accountService, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _tokenAccountService = tokenAccountService;
            _accountService = accountService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        #endregion

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] Account account)
        {
            account.Role = "customer";
            var serviceResult = _accountService.Add(account);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }


        /// <summary>
        /// Lấy toàn bộ ds
        /// </summary>
        /// <returns></returns>
        /// Author HieuNv
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] Account account)
        {
            var token = _jwtAuthenticationManager.Authenticate(account.Phone, account.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new ResponseModel(1000, "OK", new { token = token }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        public IActionResult LogOut([FromHeader] string Authorization)
        {
            var serviceResult = _tokenAccountService.DeleteByProp("Token", Authorization);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost("changePassword")]
        public IActionResult ChangePassword([FromQuery] string password, [FromQuery] string newPassword)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("Phone", phoneNumber).Response.Data;
            if (BCrypt.Net.BCrypt.Verify(password, acc.Password))
            {
                acc.Password = newPassword;
                var serviceResult = _accountService.Update(acc, acc.AccountId);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            else
            {
                return StatusCode(400, new ResponseModel(1009, "Password incorrect!"));
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPost("addShopAccount")]
        public IActionResult AddShopAccount([FromBody] Account account)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("Phone", phoneNumber).Response.Data;
            if(acc.Phone != "0971883025")
            {
                return StatusCode(403, new ResponseModel(4003, "Tài khoản của bạn không có quyền tạo acc admin!"));
            }
            account.Role = "admin";
            var serviceResult = _accountService.Add(account);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("deleteAccount/{accountId}")]
        public IActionResult DeleteShopAccount([FromRoute] Guid accountId)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("Phone", phoneNumber).Response.Data;
            if (acc.Phone != "0971883025")
            {
                return StatusCode(403, new ResponseModel(4003, "Tài khoản của bạn không có quyền xóa acc!"));
            }else if (acc.AccountId.Equals(accountId))
            {
                return StatusCode(400, new ResponseModel(4000, "Không thể xóa tài khoản này!"));
            }
            var serviceResult = _accountService.Delete(accountId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getAccounts")]
        public IActionResult GetAccounts()
        {
            var serviceResult = _accountService.GetAll();
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [HttpPost("updateAccount")]
        public IActionResult UpdateAccount([FromBody] Account account)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("Phone", phoneNumber).Response.Data;
            acc.AccountName = (account.AccountName != null) ? account.AccountName : acc.AccountName;
            acc.Email = (account.Email != null) ? account.Email : acc.Email;
            acc.Phone = (account.Phone != null) ? account.Phone : acc.Phone;
            var serviceResult = _accountService.Update(acc, acc.AccountId);

            if (phoneNumber != acc.Phone)
            {
                var sr = _tokenAccountService.DeleteByProp("Phone", phoneNumber);
            }
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }
    }
}
