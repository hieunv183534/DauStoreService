using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using DauStore.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DauStore.Core.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity>
    {

        #region Declare

        protected IBaseRepository<TEntity> _baseRepository;
        public ServiceResult _serviceResult;

        #endregion

        #region Constructor

        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult();
        }

        #endregion


        public virtual ServiceResult Add(TEntity entity)
        {
            try
            {
                // xử lí nghiệp vụ thêm
                var validateRs = Validate(entity, "add");

                if (validateRs.Code != -1)
                {
                    _serviceResult.Response = validateRs;
                    _serviceResult.StatusCode = 400;
                    return _serviceResult;
                }

                //Mã hóa password
                var passwordProp = entity.GetType().GetProperty("Password");
                if (passwordProp != null)
                {

                    string password = (string)passwordProp.GetValue(entity);
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                    passwordProp.SetValue(entity, passwordHash);
                }


                // thêm dữ liệu vào db
                var rowAffect = _baseRepository.Add(entity);
                if (rowAffect > 0)
                {
                    _serviceResult.Response = new ResponseModel(2001, "Cập nhật, thêm dữ liệu thành công");
                    _serviceResult.StatusCode = 201;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(5005, "Lỗi không xác định");
                    _serviceResult.StatusCode = 500;
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

        public virtual ServiceResult Delete(Guid entityId)
        {
            try
            {
                // xử lí nghiệp vụ xóa
                // xóa dữ liệu khỏi db
                var rowAffect = _baseRepository.Delete(entityId);
                if (rowAffect > 0)
                {
                    _serviceResult.Response = new ResponseModel(2001, "Cập nhật, thêm dữ liệu thành công");
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(5005, "Unknown Error");
                    _serviceResult.StatusCode = 500;
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

        public ServiceResult DeleteByProp(string propName, object propValue)
        {
            try
            {
                // xử lí nghiệp vụ xóa
                // xóa dữ liệu khỏi db
                var rowAffect = _baseRepository.DeleteByProp(propName, propValue);
                if (rowAffect > 0)
                {
                    _serviceResult.Response = new ResponseModel(2001, "OK");
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(5005, "Unknown Error");
                    _serviceResult.StatusCode = 500;
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

        public ServiceResult GetById(Guid entityId)
        {
            try
            {
                // xử lí nghiệp vụ lấy 1 dữ liệu
                // lấy bản ghi theo id
                var entity = _baseRepository.GetById(entityId);
                if (entity != null)
                {
                    _serviceResult.Response = new ResponseModel(2000, "OK", entity);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(2004, "No data or end of list data");
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

        public ServiceResult GetByProp(string propName, object propValue)
        {
            try
            {
                // xử lí nghiệp vụ lấy 1 dữ liệu
                // lấy bản ghi theo id
                var entity = _baseRepository.GetByProp(propName, propValue);
                if (entity != null)
                {
                    _serviceResult.Response = new ResponseModel(2000, "OK", entity);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(2004, "No data or end of list data");
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

        public virtual ServiceResult Update(TEntity entity, Guid entityId)
        {
            try
            {
                // xử lí nghiệp vụ cập nhật
                var validateRs = Validate(entity, "update");

                if (validateRs.Code != -1)
                {
                    _serviceResult.Response = validateRs;
                    _serviceResult.StatusCode = 400;
                    return _serviceResult;
                }

                //Mã hóa password
                var passwordProp = entity.GetType().GetProperty("Password");
                if (passwordProp != null)
                {
                    string password = (string)passwordProp.GetValue(entity);
                    if (password.Length < 50)
                    {
                        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                        passwordProp.SetValue(entity, passwordHash);
                    }
                }

                // thêm dữ liệu vào db
                var rowAffect = _baseRepository.Update(entity, entityId);
                if (rowAffect > 0)
                {
                    _serviceResult.Response = new ResponseModel(2001, "OK", rowAffect);
                    _serviceResult.StatusCode = 201;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(5005, "Unknown Error");
                    _serviceResult.StatusCode = 500;
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

        public ServiceResult GetAll()
        {
            try
            {
                // xử lí nghiệp vụ lấy dữ liệu
                // lấy bản ghi theo id
                var entitys = _baseRepository.GetAll();
                if (entitys.Count > 0)
                {
                    // Xử lí sắp xếp theo cấp nếu là category
                    if (typeof(TEntity).Equals(typeof(Category)))
                    {
                        var categoryCodeProp = typeof(Category).GetProperty("CategoryCode");
                        entitys = entitys.OrderBy( e =>
                        {
                            string categoryCode = (string)categoryCodeProp.GetValue(e);
                            return categoryCode.Split('_').Length;
                        }).ToList();
                    }

                    _serviceResult.Response = new ResponseModel(2000, "OK", entitys);
                    _serviceResult.StatusCode = 200;
                    return _serviceResult;
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(2004, "No data or end of list data");
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="mode"></param>
        /// <returns> messenger lỗi tương ứng </returns>
        /// Author HieuNv
        public ResponseModel Validate(TEntity entity, string mode)
        {
            var props = entity.GetType().GetProperties();

            foreach (var prop in props)
            {
                // kiểm tra trường bắt buộc nhập reqiued !!!! 1
                if (prop.IsDefined(typeof(Requied), false))
                {
                    var propValue = prop.GetValue(entity);
                    if (propValue == null || (propValue.ToString() == String.Empty))
                    {
                        return new ResponseModel(3002, $"{prop.Name} Requied!");
                    }
                }
                // kiểm tra trùng
                if (prop.IsDefined(typeof(NotAllowDuplicate), false))
                {
                    var entityDuplicate = _baseRepository.GetByProp(prop.Name, prop.GetValue(entity));
                    if (mode == "add" && entityDuplicate != null)
                    {
                        return new ResponseModel(3003, "Duplicate field");
                    }
                    else if (mode == "update" && entityDuplicate != null)
                    {
                        if (
                            entityDuplicate.GetType().GetProperty($"{typeof(TEntity).Name}Id").GetValue(entityDuplicate).ToString() !=
                            entity.GetType().GetProperty($"{typeof(TEntity).Name}Id").GetValue(entity).ToString()
                            )
                        {
                            return new ResponseModel(3003, "Duplicate field");
                        }
                    }
                }
                // Kiểm tra email
                if (prop.Name == "Email")
                {
                    if (prop.GetValue(entity) != null && prop.GetValue(entity) != "")
                    {
                        if (!Common.IsValidEmail((string)prop.GetValue(entity)))
                        {
                            return new ResponseModel(3001, "Email invalid");
                        }
                    }
                }
                //kiểm tra số điện thoại
                if (prop.Name == "Phone")
                {
                    if (prop.GetValue(entity) != null && prop.GetValue(entity) != "")
                    {
                        if (!Common.IsValidPhoneNumber((string)prop.GetValue(entity)))
                        {
                            return new ResponseModel(3001, "PhoneNumber invalid");
                        }
                    }
                }
                //kiểm tra password
                if (prop.Name == "Password")
                {
                    if (prop.GetValue(entity) != null && prop.GetValue(entity) != "")
                    {
                        string password = (string)prop.GetValue(entity);
                        bool isHash = (password.Length > 50);
                        if (!isHash)
                        {
                            if (!Common.IsValidPassword((string)prop.GetValue(entity)))
                            {
                                return new ResponseModel(3001, "Password invalid");
                            }
                        }
                    }
                }
            }
            return new ResponseModel(-1, "");
        }


    }
}
