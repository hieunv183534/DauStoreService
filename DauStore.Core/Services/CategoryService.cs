using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using DauStore.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DauStore.Core.Services
{
    public class CategoryService : BaseService<Category> , ICategoryService
    {

        public CategoryService(IBaseRepository<Category> baseRepository) : base(baseRepository)
        {
            
        }

        public override ServiceResult Add(Category category)
        {
            bool isValid = true;
            int maxCode = 0;
            // xử lí nghiệp vụ trước khi add category
            List<Category> categories = _baseRepository.GetAll();
            categories = PreProcess(categories);
            categories.ForEach(c =>
            {

            });

            return base.Add(category);
        }

        private List<Category> PreProcess(List<Category> categories)
        {
            categories.ForEach(ele =>
            {
                // refactor list category
                var listCodeStr = ele.CategoryCode.Split("|");
                ele.SeftCode = Int32.Parse(listCodeStr.Last());
                var parentCodeArr = listCodeStr.SkipLast(1).ToArray();
                ele.ParentCode = "|" + String.Join("|", parentCodeArr);
            });
            return categories;
        }
    }
}
