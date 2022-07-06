using DauStore.Core.Entities;
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
    public class ItemController : ControllerBase
    {
        protected IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [AllowAnonymous]
        [HttpGet("getItems")]
        public IActionResult GetItems([FromQuery] string categoryCode, [FromQuery] string searchTerms,
            [FromQuery] string tag, [FromQuery] int orderState, [FromQuery] int index, [FromQuery] int count)
        {
            var serviceResult = _itemService.GetItems(categoryCode, searchTerms, tag, orderState, index, count);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [AllowAnonymous]
        [HttpGet("getItemById/{itemId}")]
        public IActionResult GetItems([FromRoute] Guid itemId) 
        {
            var serviceResult = _itemService.GetById(itemId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("addItem")]
        public IActionResult AddItem([FromBody] Item item)
        {
            var serviceResult = _itemService.Add(item);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("updateItem/{itemId}")]
        public IActionResult UpdateItem([FromBody] Item item, [FromRoute] Guid itemId)
        {
            var serviceResult = _itemService.Update(item, itemId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("deleteItem/{itemId}")]
        public IActionResult DeleteItem([FromRoute] Guid itemId)
        {
            var serviceResult = _itemService.Delete(itemId);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("getNewItemCode")]
        public IActionResult GetNewCode()
        {
            var serviceResult = _itemService.GetNewItemCode();
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("changeInStock/{itemId}/{changeNumber}")]
        public IActionResult ChangeInStock([FromRoute] Guid itemId, [FromRoute] int changeNumber)
        {
            var serviceResult = _itemService.ChangeInStock(itemId, changeNumber);
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }
    }
}
