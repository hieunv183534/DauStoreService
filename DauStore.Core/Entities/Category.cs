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

        public Guid CategoryId { get; set; }

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }
    }
}
