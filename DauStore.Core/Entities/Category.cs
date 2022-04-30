using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {

        }

        [PrimaryKey]
        public Guid CategoryId { get; set; }

        [Requied]
        [NotAllowDuplicate]
        public string CategoryCode { get; set; }

        [Requied]
        public string CategoryName { get; set; }

        public int SeftCode { get; set; }

        public string ParentCode { get; set; }
    }
}
