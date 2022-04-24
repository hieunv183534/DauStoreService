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
        [PrimaryKey]
        public Guid OrderId { get; set; }

        [Requied, NotAllowDuplicate]
        public string OrderCode { get; set; }

        [Requied]
        public string BuyerName { get; set; }

        [Requied]
        public string Phone { get; set; }

        [Requied]
        public string Address { get; set; }

        public OrderStatus Status { get; set; }

        [Requied]
        public int PaymentMethod { get; set; }

        public bool PaymentStatus { get; set; }

        [Requied]
        public string Items { get; set; }

        [Requied]
        public double Total { get; set; }

        public double Ship { get; set; }
    }
}
