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

        [PrimaryKey]
        public Guid ItemId { get; set; }

        [Requied]
        [NotAllowDuplicate]
        public string ItemCode { get; set; }

        [Requied]
        [NotAllowDuplicate]
        public string ItemName { get; set; }

        public string Description { get; set; }

        [Requied]
        public double RealPrice { get; set; }

        [Requied]
        public float SaleRate { get; set; }

        [Requied]
        public string Medias { get; set; }

        public string Tag { get; set; }

        [Requied]
        public string CategoryCode { get; set; }
    }
}
