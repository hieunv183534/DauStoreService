using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using DauStore.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DauStore.Core.Services
{
    public class ItemService : BaseService<Item>, IItemService
    {
        protected IItemRepository _itemRepository;
        protected IBaseRepository<Category> _categoryRepository;

        public ItemService(IBaseRepository<Item> baseRepository, IItemRepository itemRepository, IBaseRepository<Category> categoryRepository) : base(baseRepository)
        {
            _itemRepository = itemRepository;
            _categoryRepository = categoryRepository;
        }

        public ServiceResult GetItems(string categoryCode, string searchTerms, string tag, int orderState, int index, int count)
        {
            try
            {
                var result = _itemRepository.GetItems(categoryCode, searchTerms, tag, orderState, index, count);
                List<Item> data = (List<Item>)result.GetType().GetProperty("data").GetValue(result, null);
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

        public override ServiceResult Add(Item item)
        {

            try
            {
                if (ValidateItemCode(item.ItemCode))
                {
                    if (ValidateCategoryCode(item.CategoryCode))
                    {
                        item.Description = Newtonsoft.Json.JsonConvert.SerializeObject(item.ListDescription);
                        return base.Add(item);
                    }
                    else
                    {
                        _serviceResult.Response = new ResponseModel(4001, "CategoryCode không đúng định dạng!");
                        _serviceResult.StatusCode = 400;
                        return _serviceResult;
                    }
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(4001, "Mã sản phẩm không đúng định dạng!");
                    _serviceResult.StatusCode = 400;
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

        public override ServiceResult Update(Item item, Guid itemId)
        {

            try
            {
                if ((item.ItemCode != null && ValidateItemCode(item.ItemCode)) || item.ItemCode == null)
                {
                    if (ValidateCategoryCode(item.CategoryCode))
                    {
                        item.ItemId = itemId;
                        item.Description = Newtonsoft.Json.JsonConvert.SerializeObject(item.ListDescription);
                        return base.Update(item, itemId);
                    }
                    else
                    {
                        _serviceResult.Response = new ResponseModel(4001, "CategoryCode không đúng định dạng!");
                        _serviceResult.StatusCode = 400;
                        return _serviceResult;
                    }
                }
                else
                {
                    _serviceResult.Response = new ResponseModel(4001, "Mã sản phẩm không đúng định dạng!");
                    _serviceResult.StatusCode = 400;
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

        private bool ValidateItemCode(string itemCode)
        {
            return Regex.IsMatch(itemCode, @"^((SP)(\d+))$");
        }

        private bool ValidateCategoryCode(string categoryCode)
        {
            if (categoryCode == null)
            {
                return false;
            }
            else if (categoryCode == "")
            {
                return true;
            }
            else
            {
                List<Category> categories = _categoryRepository.GetAll();
                for (int i = 0; i < categories.Count; i++)
                {
                    if (categories[i].CategoryCode == categoryCode)
                        return true;
                }
                return false;
            }
        }

        public ServiceResult GetNewItemCode()
        {
            try
            {
                string newCode = _baseRepository.GetNewCode();
                _serviceResult.Response = new ResponseModel(2000, "OK", newCode);
                _serviceResult.StatusCode = 200;
                return _serviceResult;
            }
            catch (Exception ex)
            {
                _serviceResult.Response = new ResponseModel(9999, "Exception Error", new { msg = ex.Message });
                _serviceResult.StatusCode = 500;
                return _serviceResult;
            }
        }

        public ServiceResult ChangeInStock(Guid itemId, int changeNumber)
        {
            try
            {
                int rows = _itemRepository.ChangInStock(changeNumber, itemId);
                _serviceResult.Response = new ResponseModel(2001, "OK", rows);
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
