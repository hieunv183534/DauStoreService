using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DauStore.Api.Controllers
{
    [Authorize(Roles = "admin, customer")]
    [Route("api")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        #region Declare

        IBaseService<Notification> _notificationService;
        IBaseService<Account> _accountService;
        INotificationService _notificationService1;

        #endregion

        #region Constructor

        public NotificationController(IBaseService<Notification> notificationService, IBaseService<Account> accountService, INotificationService notificationService_)
        {
            _notificationService = notificationService;
            _accountService = accountService;
            _notificationService1 = notificationService_;
        }

        #endregion

        [HttpGet("getNotifications")]
        public IActionResult GetNotifications([FromQuery] int index, [FromQuery] int count)
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("Phone", phoneNumber).Response.Data;
            var serviceResult = _notificationService1.GetNotifications(acc.AccountId,index,count);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }


        [Authorize(Roles = "admin")]
        [HttpPost("addNotification")]
        public IActionResult AddNotification([FromBody] Notification notification)
        {
            if(_accountService.GetById(notification.AccountId).StatusCode == 200)
            {
                var serviceResult = _notificationService.Add(notification);
                return StatusCode(serviceResult.StatusCode, serviceResult.Response);
            }
            else
            {
                return StatusCode(400, new ResponseModel(3001, "UserId không trùng với bất kì tài khoản nào"));
            }
        }

        [HttpPost("seenAllNotification")]
        public IActionResult SeenAllNotification()
        {
            var phoneNumber = User.FindFirstValue(ClaimTypes.Name);
            var acc = (Account)_accountService.GetByProp("Phone", phoneNumber).Response.Data;
            var serviceResult = _notificationService1.SeenAll(acc.AccountId);
            return StatusCode(serviceResult.StatusCode,serviceResult.Response);
        }
    }
}
