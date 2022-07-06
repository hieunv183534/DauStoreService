using DauStore.Core.Entities;
using DauStore.Core.Enums;
using DauStore.Core.Interfaces.IServices;
using GemBox.Document;
using GemBox.Document.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DauStore.Api.Controllers
{
    [Authorize(Roles = "admin, customer")]
    [Route("api")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        protected IOrderService _orderService;
        protected IBaseService<Voucher> _voucherService;

        public OrderController(IOrderService orderService, IBaseService<Voucher> voucherService)
        {
            _orderService = orderService;
            _voucherService = voucherService;
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

        [Authorize(Roles = "admin")]
        [HttpGet("getOrderDocFile/{orderId}")]
        public IActionResult GetOrderDocFile([FromRoute] Guid orderId , [FromQuery] string unitAddress)
        {
            var serviceResult = _orderService.GetById(orderId);
            if (serviceResult.StatusCode == 200)
            {
                Order order = (Order)serviceResult.Response.Data;
                string dir = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot/bill");
                string fileName = $"{order.OrderCode}.docx";
                string filePath = Path.Combine(dir, fileName);
                // If using Professional version, put your serial key below.
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");


                DocumentModel document = DocumentModel.Load(Path.Combine(dir, @"TemplateBill.docx"));

                // Template document contains 4 tables, each contains some set of information.
                Table[] tables = document.GetChildElements(true, ElementType.Table).Cast<Table>().ToArray();

                // First table contains invoice number and date.
                Table invoiceTable = tables[0];
                invoiceTable.Rows[0].Cells[0].Blocks.Add(new Paragraph(document, "- Mã đơn: "+  order.OrderCode + " \n- Ngày đặt: " + order.CreatedAt.ToString("d MMM yyyy HH:mm")+
                    $"\n- Người đặt: {order.BuyerName} \n- Số điện thoại: {order.Phone} \n- Địa chỉ: {order.Address}, {unitAddress}" + $" \n- Ghi chú: {order.Note}"));


                string[] items = order.Items.Split(" _and_ ");

                // Third table contains amount and prices, it only has one data row in the template document.
                // So, we'll dynamically add cloned rows for the rest of our data items.
                Table mainTable = tables[1];
                    

                string str = "";
                for (int rowIndex = 0; rowIndex < items.Length; rowIndex++)
                {
                    string[] itemArr = items[rowIndex].Split("|");
                    string itemString = $"- {itemArr[0]} {itemArr[4]} giá {itemArr[2]} VND \n";
                    str += itemString;
                }

                // Hiển thị thông tin giảm giá
                if (!order.VoucherId.Equals(Guid.Empty))
                {
                    var sr = _voucherService.GetById(order.VoucherId);
                    if(sr.StatusCode == 200)
                    {
                        Voucher voucher = (Voucher)sr.Response.Data;
                        mainTable.Rows.Insert(1, mainTable.Rows[0].Clone(true));
                        mainTable.Rows[1].Cells[0].Blocks.Add(new Paragraph(document, $"Mã giảm giá: {voucher.Description}"));
                    }
                    
                }
                mainTable.Rows[0].Cells[0].Blocks.Add(new Paragraph(document, str));


                // Last cell in the last, total, row has some predefined formatting stored in an empty paragraph.
                // So, in this case instead of adding new paragraph we'll add our data into an existing paragraph.
                mainTable.Rows.Last().Cells[0].Blocks.Cast<Paragraph>(0).Content.LoadText( "Tổng tiền: " + order.TotalMoney.ToString() + "VND");


                document.Save(filePath);
                return StatusCode(serviceResult.StatusCode, filePath);
            }
            return StatusCode(serviceResult.StatusCode, serviceResult.Response);
        }
    }
}
