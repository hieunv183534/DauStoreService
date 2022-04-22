using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class User : BaseEntity
    {
        public User()
        {

        }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string  Phone { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

    }
}
