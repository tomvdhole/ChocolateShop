using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocolateShopApp.Clients
{
    public interface IClient<TViewModel>
    {
        Task<TViewModel> GetEntity(string path, int id);
        Task<IEnumerable<TViewModel>> GetEntities(string path);
        Task SaveEntity(string path, TViewModel model);
        Task UpdateEntity(string path, TViewModel model);
        Task DeleteEntity(string path, TViewModel model);
    }
}
