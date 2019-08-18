using ChocolateShopApi.Models;
using ChocolateShopApi.Services;
using ChocolateShopViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocolateShopApi.Controllers
{
    public class ProductsController : EntityController<ProductsController, Product, EntityImageViewModel>
    {
        public ProductsController(ILogger<ProductsController> logger, IEntityService<Product> service)
            : base(logger, service)
        { }
        
        protected override IEnumerable<EntityImageViewModel> GetViewModels(IEnumerable<Product> entities)
           => entities.Select(e => new EntityImageViewModel { Id = e.Id, Name = e.Name, Image = e.Image});

        protected override EntityImageViewModel GetViewModel(Product entity)
            => new EntityImageViewModel { Id = entity.Id, Name = entity.Name, Image = entity.Image };

        protected async override Task<Product> PostEntity(EntityImageViewModel model)
            => await Service.PostEntity(new Product { Id = model.Id, Name = model.Name, Image = model.Image });

        protected async override Task PutEntity(EntityImageViewModel model)
           => await Service.PutEntity(new Product { Id = model.Id, Name = model.Name, Image = model.Image });
    }
}