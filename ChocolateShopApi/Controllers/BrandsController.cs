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
            => entities.Select(e => new BrandViewModel { Id = e.Id, Name = e.Name, BrandCategories = GetBrandCategoryViewModels(e.BrandCategories) });
        
       
        protected override BrandViewModel GetViewModel(Brand entity)
            => new BrandViewModel { Id = entity.Id, Name = entity.Name , BrandCategories = GetBrandCategoryViewModels(entity.BrandCategories)};

        protected async override Task<Brand> PostEntity(BrandViewModel model)
            => await Service.PostEntity(new Brand { Id = model.Id, Name = model.Name });

        protected async override Task PutEntity(BrandViewModel model)
            => await Service.PutEntity(new Brand { Id = model.Id, Name = model.Name });

        protected async override Task<IEnumerable<Brand>> GetEntitiesWithIncludes()
            => await Service.GetEntities("BrandCategories.Category.CategoryProducts.Product");

        protected async override Task<Brand> GetEntityWithIncludes(int id)
            => await Service.GetEntity(id, "BrandCategories.Category.CategoryProducts.Product");


        private IList<BrandCategoryViewModel> GetBrandCategoryViewModels(IList<BrandCategory> brandCategories)
        {
            var model = new List<BrandCategoryViewModel>();

            foreach(var bc in brandCategories)
                model.Add(new BrandCategoryViewModel { Brand = GetBrandViewModel(bc.Brand), BrandId = bc.BrandId, Category = GetCategoryViewModel(bc.Category), CategoryId = bc.CategoryId });

            return model;
        }

        private BrandViewModel GetBrandViewModel(Brand brand)
            => new BrandViewModel { Id = brand.Id, Name = brand.Name };

        private CategoryViewModel GetCategoryViewModel(Category category)
            => new CategoryViewModel { Id = category.Id, Name = category.Name, CategoryProducts = GetCategoryProductViewModel(category.CategoryProducts)};


        private IList<CategoryProductViewModel> GetCategoryProductViewModel(IList<CategoryProduct> categoryProducts)
        {
            var model = new List<CategoryProductViewModel>();

            foreach (var cp in categoryProducts)
                model.Add(new CategoryProductViewModel { Category = null, CategoryId = cp.CategoryId, Product = GetProductViewModel(cp.Product), ProductId = cp.ProductId });

            return model;
        }

        private ProductViewModel GetProductViewModel(Product product)
            => new ProductViewModel { Id = product.Id, Name = product.Name, Image = product.Image };


        //private IList<BrandCategory> GetBrandCategories(IList<BrandCategoryViewModel> brandCategories)
        //{
        //    var model = new List<BrandCategory>();

        //    foreach (var bc in brandCategories)
        //        model.Add(new BrandCategory { Brand = GetBrand(bc.Brand), BrandId = bc.BrandId, Category = GetCategory(bc.Category), CategoryId = bc.CategoryId });

        //    return model;
        //}

        //private Brand GetBrand(BrandViewModel model)
        //    => new Brand { Id = model.Id, Name = model.Name };

        //private Category GetCategory(CategoryViewModel model)
        //    => new Category { Id = model.Id, Name = model.Name };
    }
}

        