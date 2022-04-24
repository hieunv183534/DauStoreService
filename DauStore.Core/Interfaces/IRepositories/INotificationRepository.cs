using DauStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Interfaces.IRepositories
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        int SeenAll(Guid accountId);

        Object GetNotifications(Guid accountId, int index, int count);
    }
}
