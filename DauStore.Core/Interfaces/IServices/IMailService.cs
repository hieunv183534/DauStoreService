using DauStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DauStore.Core.Interfaces.IServices
{
    public interface IMailService 
    {
        void SendMail(MailContent mailContent);
    }
}
