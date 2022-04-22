using DauStore.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }

        public Guid OrderId { get; set; }

        public string OrderCode { get; set; }

        public string BuyerName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public OrderStatus Status { get; set; }

        public int PaymentMethod { get; set; }

        public bool PaymentStatus { get; set; }

        public string Items { get; set; }

        public double Total { get; set; }

        public double Ship { get; set; }
    }
}
