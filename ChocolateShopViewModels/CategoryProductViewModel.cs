using System;
using System.Collections.Generic;
using System.Text;

namespace ChocolateShopViewModels
{
    public class CategoryProductViewModel
    {
        public int CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }

        public int ProductId { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
