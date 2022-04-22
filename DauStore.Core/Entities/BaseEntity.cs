using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
