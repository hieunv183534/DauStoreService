using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Entities
{
    public class TokenAccount : BaseEntity
    {
        public TokenAccount(string phone, string token)
        {
            this.Phone = phone;
            this.Token = token;
        }

        public TokenAccount()
        {
        }

        #region Property

        public string Phone { get; set; }

        public string Token { get; set; }
        #endregion
    }
}
