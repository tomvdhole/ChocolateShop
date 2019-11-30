using System;
using System.Collections.Generic;
using System.Text;

namespace ChocolateShopViewModels
{
    public class CategoryViewModel: EntityViewModelBase
    {
        public CategoryViewModel()
        {
            BrandCategories = new List<BrandCategoryViewModel>();
            CategoryProducts = new List<CategoryProductViewModel>();
        }

        public IList<BrandCategoryViewModel> BrandCategories { get; set; }
        public IList<CategoryProductViewModel> CategoryProducts { get; set; }
    }
}
