using System;
using System.Collections.Generic;
using System.Text;

namespace ChocolateShopViewModels
{
    public class BrandViewModel: EntityViewModelBase
    {
        public BrandViewModel()
        {
            BrandCategories = new List<BrandCategoryViewModel>();
        }

        public IList<BrandCategoryViewModel> BrandCategories { get; set; }
    }
}
