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

        public Guid NotificationId { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }

        public bool Seen { get; set; }
    }
}
