using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class Item : BaseEntity
    {
        public Item()
        {

        }

        public Guid ItemId { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string Description { get; set; }

        public double RealPrice { get; set; }

        public float SaleRate { get; set; }

        public string Medias { get; set; }

        public string Tag { get; set; }

        public string CategoryCode { get; set; }
    }
}
