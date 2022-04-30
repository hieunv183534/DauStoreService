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

        [Requied]
        public string UnitCode { get; set; }

        public OrderStatusEnum Status { get; set; }

        [Requied]
        public PaymentMethodEnum PaymentMethod { get; set; }

        [Requied]
        public bool PaymentStatus { get; set; }

        [Requied]
        public string Items { get; set; }

        [Requied]
        public double TotalMoney { get; set; }

        public string Ship { get; set; }

        [Requied]
        public bool ShipPayStatus { get; set; }

        public string Note { get; set; }

        public Guid VoucherId { get; set; }
    }
}
