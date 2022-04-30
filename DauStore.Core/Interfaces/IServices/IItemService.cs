using DauStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Interfaces.IServices
{
    public interface IItemService : IBaseService<Item>
    {
        ServiceResult GetItems(string categoryCode, string searchTerms, string tag, int orderState, int index, int count);

        ServiceResult GetNewItemCode();
    }
}
