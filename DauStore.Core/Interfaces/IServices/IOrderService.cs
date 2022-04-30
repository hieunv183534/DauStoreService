using DauStore.Core.Entities;
using DauStore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Interfaces.IServices
{
    public interface IOrderService : IBaseService<Order>
    {
        ServiceResult GetOrders(OrderStatusEnum orderStatus, string searchTerms, double startTime, double endTime, int orderTimeState, int index, int count);

        ServiceResult UpdateOrderByAdmin(Order order, Guid orderId);

        ServiceResult UpdateOrderByCustomer(Order order, Guid orderId);

        ServiceResult LookupOrder(string key);
    }
}
