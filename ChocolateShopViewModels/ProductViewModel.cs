using System;
using System.Collections.Generic;
using System.Text;

namespace ChocolateShopViewModels
{
    public class ProductViewModel: EntityImageViewModel
    {
        public IList<CategoryProductViewModel> CategoryProducts { get; set; }
    }
}
