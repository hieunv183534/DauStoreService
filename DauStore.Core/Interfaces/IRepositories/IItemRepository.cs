using DauStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DauStore.Core.Interfaces.IRepositories
{
    public interface IItemRepository : IBaseRepository<Item>
    {
        Object GetItems(string categoryCode, string searchTerms, string tag, int orderState, int index, int count);
    }
}
