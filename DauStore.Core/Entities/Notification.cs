using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class Notification : BaseEntity
    {
        public Notification()
        {

        }

        [PrimaryKey]
        public Guid NotificationId { get; set; }

        [Requied]
        public string Content { get; set; }

        [Requied]
        public Guid AccountId { get; set; }

        public bool Seen { get; set; }
    }
}
