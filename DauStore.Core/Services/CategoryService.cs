using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using DauStore.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DauStore.Core.Services
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {

        public CategoryService(IBaseRepository<Category> baseRepository) : base(baseRepository)
        {

        }

        public override ServiceResult Add(Category category)
        {
            bool validParent = false;
            // xử lí nghiệp vụ trước khi add category
            List<Category> categories = _baseRepository.GetAll();
            if (category.ParentCode == "")
                validParent = true;
            else
            {
                for (int i = 0; i < categories.Count; i++)
                {
                    if (categories[i].CategoryCode == category.ParentCode)
                    {
                        validParent = true;
                        break;
                    }
                }
            }

            // nếu là valid parent thì chuyển sang check name
            int maxCode = 0;
            if (validParent)
            {
                var sameParentsQuery = from element in categories where element.ParentCode == category.ParentCode select element;
                var sameParents = sameParentsQuery.ToList();
                for (int i = 0; i < sameParents.Count; i++)
                {
                    if (sameParents[i].CategoryName == category.CategoryName)
                    {
                        // trường hợp invalid name do trùng tên
                        _serviceResult.Response = new ResponseModel(4001, "Tên trùng với 1 element cùng cấp!");
                        _serviceResult.StatusCode = 400;
                        return _serviceResult;
                    }
                    else
                    {
                        maxCode = (maxCode < sameParents[i].SeftCode) ? sameParents[i].SeftCode : maxCode;
                    }
                }

                // valid name. gọi base để add
                category.SeftCode = maxCode +1;
                category.CategoryCode = $"{category.ParentCode}|{category.SeftCode}";
                return base.Add(category);
            }
            else
            {
                _serviceResult.Response = new ResponseModel(4001, "Mã code của parent không hợp lệ!");
                _serviceResult.StatusCode = 400;
                return _serviceResult;
            }
        }

        public override ServiceResult Update(Category _category, Guid categoryId)
        {
            var category = _baseRepository.GetById(categoryId);
            category.CategoryName = _category.CategoryName;
            List<Category> categories = _baseRepository.GetAll();
            var sameParentsNotSeftSameName =  (from element in categories 
                                      where (element.ParentCode == category.ParentCode) &&
                                      (element.CategoryId != categoryId) &&
                                      (element.CategoryName == category.CategoryName) select element).ToList();
            if (sameParentsNotSeftSameName.Count == 0)
            {
                return base.Update(category, categoryId);
            }
            else
            {
                _serviceResult.Response = new ResponseModel(4001, "Tên mới bị trùng với sibling!");
                _serviceResult.StatusCode = 400;
                return _serviceResult;
            }
        }

        public override ServiceResult Delete(Guid categoryId)
        {
            Category category = _baseRepository.GetById(categoryId);
            if(category == null)
            {
                _serviceResult.Response = new ResponseModel(4001, "CategoryId không hợp lệ!");
                _serviceResult.StatusCode = 400;
                return _serviceResult;
            }
            List<Category> categories = _baseRepository.GetAll();
            // lấy các bản ghi là con
            var childs = (from element in categories where element.ParentCode == category.CategoryCode select element).ToList();
            if(childs.Count > 0)
            {
                _serviceResult.Response = new ResponseModel(4000, "Hãy xóa các element con trước!");
                _serviceResult.StatusCode = 400;
                return _serviceResult;
            }
            else
            {
                return base.Delete(categoryId);
            }
        }

        //private Category PreProcess(Category category)
        //{
        //    // complete category
        //    var listCodeStr = category.CategoryCode.Split("|");
        //    category.SeftCode = Int32.Parse(listCodeStr.Last());
        //    var parentCodeArr = listCodeStr.SkipLast(1).ToArray();
        //    category.ParentCode = "|" + String.Join("|", parentCodeArr);
        //    return category;
        //}
    }
}
