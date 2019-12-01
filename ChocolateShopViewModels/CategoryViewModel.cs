using System;
using System.Collections.Generic;
using System.Text;

namespace ChocolateShopViewModels
{
    public class CategoryViewModel: EntityViewModelBase
    {
        public CategoryViewModel()
        {
            Products = new List<ProductViewModel>();
        }

        public IList<ProductViewModel> Products { get; set; }
    }
}
