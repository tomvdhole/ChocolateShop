using System;
using System.Collections.Generic;
using System.Text;

namespace ChocolateShopViewModels
{
    public class BrandCategoryViewModel
    {
        public int BrandId { get; set; }
        public BrandViewModel Brand { get; set; }

        public int CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
