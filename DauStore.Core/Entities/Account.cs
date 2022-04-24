using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class Account : BaseEntity
    {
        public Account()
        {

        }

        [PrimaryKey]
        public Guid AccountId { get; set; }

        [Requied]
        public string AccountName { get; set; }

        [Requied]
        [NotAllowDuplicate]
        public string  Phone { get; set; }

        public string Email { get; set; }

        [Requied]
        public string Password { get; set; }

        [Requied]
        public string Role { get; set; }

    }
}
