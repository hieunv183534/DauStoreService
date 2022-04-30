using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class Voucher : BaseEntity
    {
        public Voucher()
        {

        }

        [PrimaryKey]
        public Guid VoucherId { get; set; }

        [Requied]
        [NotAllowDuplicate]
        public string VoucherCode { get; set; }

        public string Description { get; set; }

        public float SaleRate { get; set; }

        public double MaxNumber { get; set; }

        public double SaleNumber { get; set; }

        public double MinTotal { get; set; }

        public int Quota { get; set; }

        public DateTime DateExpired { get; set; }
    }
}
