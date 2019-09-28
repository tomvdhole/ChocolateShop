using ChocolateShopApi.Models;
using ChocolateShopApi.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ChocolateShopViewModels;

namespace ChocolateShopApi.Controllers
{
    public class BrandsController : EntityController<Brand, EntityViewModel>
    {
        public BrandsController(ILogger<BrandsController> logger, IEntityService<Brand> service)
            : base(logger, service)
        { }

        protected override IEnumerable<EntityViewModel> GetViewModels(IEnumerable<Brand> entities)
            => entities.Select(e => new EntityViewModel { Id = e.Id, Name = e.Name });
       
        protected override EntityViewModel GetViewModel(Brand entity)
            => new EntityViewModel { Id = entity.Id, Name = entity.Name };

        protected async override Task<Brand> PostEntity(EntityViewModel model)
            => await Service.PostEntity(new Brand { Id = model.Id, Name = model.Name });

        protected async override Task PutEntity(EntityViewModel model)
            => await Service.PutEntity(new Brand { Id = model.Id, Name = model.Name });

        protected async override Task<IEnumerable<Brand>> GetEntitiesWithIncludes()
            => await Service.GetEntities("BrandCategories.Category.CategoryProducts.Product");

        protected async override Task<Brand> GetEntityWithIncludes(int id)
            => await Service.GetEntity(id, "BrandCategories.Category.CategoryProducts.Product");
    }
}

        