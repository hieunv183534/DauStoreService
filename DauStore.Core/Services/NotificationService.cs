using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using DauStore.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Services
{
    public class NotificationService : BaseService<Notification>, INotificationService
    {
        #region Declare

        INotificationRepository _notificationRepository;

        #endregion

        #region Constructor

        public NotificationService(INotificationRepository notificationRepository, IBaseRepository<Notification> baseRepository) : base(baseRepository)
        {
            _notificationRepository = notificationRepository;
        }


        public ServiceResult GetNotifications(Guid accountId, int index, int count)
        {
            try
            {
                var result = _notificationRepository.GetNotifications(accountId, index, count);
                List<Notification> data = (List<Notification>)result.GetType().GetProperty("data").GetValue(result, null);
                if (data.Count > 0)
                {
                    _serviceResult.Response = new ResponseModel(2000, "Ok", result);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(2004, "Không có bản ghi nào!", result);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }

        #endregion

        public ServiceResult SeenAll(Guid accountId)
        {
            try
            {
                int rowAffect = _notificationRepository.SeenAll(accountId);
                _serviceResult.Response = new ResponseModel(2001, "Cập nhật dữ liệu thành công!", rowAffect);
                _serviceResult.StatusCode = 201;
                return _serviceResult;
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }
    }
}
