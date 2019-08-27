using ChocolateShopApi.Models;
using ChocolateShopApi.Services;
using ChocolateShopViewModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocolateShopApi.Controllers
{
    public class CategoriesController : EntityController<Category, EntityViewModel>
    {
        public CategoriesController(ILogger<CategoriesController> logger, IEntityService<Category> service)
            : base(logger, service)
        { }
       
        protected override IEnumerable<EntityViewModel> GetViewModels(IEnumerable<Category> entities)
            => entities.Select(e => new EntityViewModel { Id = e.Id, Name = e.Name });

        protected override EntityViewModel GetViewModel(Category entity)
            => new EntityViewModel { Id = entity.Id, Name = entity.Name };

        protected async override Task<Category> PostEntity(EntityViewModel model)
           => await Service.PostEntity(new Category { Id = model.Id, Name = model.Name });

        protected async override Task PutEntity(EntityViewModel model)
           => await Service.PutEntity(new Category { Id = model.Id, Name = model.Name });
    }
}
