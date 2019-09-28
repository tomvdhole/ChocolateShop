using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChocolateShopApi.Services
{
    public interface IEntityService<T>
    {
        Task<IEnumerable<T>> GetEntities(params string[] navigationProperties);
        Task<T> GetEntity(int id, params string[] navigationProperties);
        Task<T> PostEntity(T model);
        Task<T> PutEntity(T model);
        Task DeleteEntity(T model);
    }
}

