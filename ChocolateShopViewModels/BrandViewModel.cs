using System;
using System.Collections.Generic;
using System.Text;

namespace ChocolateShopViewModels
{
    public class BrandViewModel: EntityViewModelBase
    {
        public BrandViewModel()
        {
            Categories = new List<CategoryViewModel>();
        }

        public IList<CategoryViewModel> Categories { get; set; }
    }
}
