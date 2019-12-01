using ChocolateShopApi.Models;
using ChocolateShopApi.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ChocolateShopViewModels;

namespace ChocolateShopApi.Controllers
{
    public class BrandsController : EntityController<Brand, BrandViewModel>
    {
        public BrandsController(ILogger<BrandsController> logger, IEntityService<Brand> service)
            : base(logger, service)
        { }

        protected override IEnumerable<BrandViewModel> GetViewModels(IEnumerable<Brand> entities)
            => entities.Select(e => new BrandViewModel { Id = e.Id, Name = e.Name, Categories = GetCategoryViewModels(e.BrandCategories) });

        protected override BrandViewModel GetViewModel(Brand entity)
            => new BrandViewModel { Id = entity.Id, Name = entity.Name, Categories = GetCategoryViewModels(entity.BrandCategories) };

        protected async override Task<Brand> PostEntity(BrandViewModel model)
            => await Service.PostEntity(new Brand { Id = model.Id, Name = model.Name });

        protected async override Task PutEntity(BrandViewModel model)
            => await Service.PutEntity(new Brand { Id = model.Id, Name = model.Name });

        protected async override Task<IEnumerable<Brand>> GetEntitiesWithIncludes()
            => await Service.GetEntities("BrandCategories.Category.CategoryProducts.Product");

        protected async override Task<Brand> GetEntityWithIncludes(int id)
            => await Service.GetEntity(id, "BrandCategories.Category.CategoryProducts.Product");

        private IList<CategoryViewModel> GetCategoryViewModels(IList<BrandCategory> brandCategories)
            => brandCategories.Select(bc => new CategoryViewModel { Id = bc.CategoryId, Name = bc.Category.Name, Products = bc.Category.CategoryProducts.Select(cp => new ProductViewModel { Id = cp.ProductId, Name = cp.Product.Name, Image = cp.Product.Image }).ToList() }).ToList();

    }
}