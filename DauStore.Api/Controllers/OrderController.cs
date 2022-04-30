using DauStore.Core.Entities;
using DauStore.Core.Enums;
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
    public class OrderController : ControllerBase
    {
        protected IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getOrderById/{orderId}")]
        public IActionResult GetOrders([FromRoute] Guid orderId)
        {
            var serviceResult = _orderService.GetById(orderId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [AllowAnonymous]
        [HttpGet("lookupOrder/{key}")]
        public IActionResult LookupOrder([FromRoute] string key)
        {
            var serviceResult = _orderService.LookupOrder(key);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getOrders")]
        public IActionResult GetOrders([FromQuery] OrderStatusEnum orderStatus, [FromQuery] string searchTerms, [FromQuery] double startTime,
            [FromQuery] double endTime, [FromQuery] int orderTimeState, [FromQuery] int index, [FromQuery] int count)
        {
            endTime = (endTime == 0) ? 32472118800 : endTime;
            var serviceResult = _orderService.GetOrders(orderStatus, searchTerms, startTime, endTime, orderTimeState, index, count);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [AllowAnonymous]
        [HttpPost("addOrder")]
        public IActionResult AddOrder([FromBody] Order order)
        {
            var serviceResult = _orderService.Add(order);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("updateOrderByAdmin/{orderId}")]
        public IActionResult UpdateOrderByAdmin([FromBody] Order order, [FromRoute] Guid orderId)
        {
            var serviceResult = _orderService.UpdateOrderByAdmin(order, orderId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [AllowAnonymous]
        [HttpPut("updateOrderByCustomer/{orderId}")]
        public IActionResult UpdateOrderByCustomer([FromBody] Order order, [FromRoute] Guid orderId)
        {
            var serviceResult = _orderService.UpdateOrderByCustomer(order, orderId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("deleteOrder/{orderId}")]
        public IActionResult DeleteOrder([FromRoute] Guid orderId)
        {
            var serviceResult = _orderService.Delete(orderId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }
    }
}
