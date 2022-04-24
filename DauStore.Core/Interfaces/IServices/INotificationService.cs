using DauStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Interfaces.IServices
{
    public interface INotificationService : IBaseService<Notification>
    {
        ServiceResult SeenAll(Guid accountId);

        ServiceResult GetNotifications(Guid accountId, int index, int count);
    }
}
