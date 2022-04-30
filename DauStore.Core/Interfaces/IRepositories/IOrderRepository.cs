using DauStore.Core.Entities;
using DauStore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Interfaces.IRepositories
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Object GetOrders(OrderStatusEnum orderStatus, string searchTerms, double startTime, double endTime, int orderTimeState, int index, int count);
        List<Order> LookupOrder(string key);
    }
}
