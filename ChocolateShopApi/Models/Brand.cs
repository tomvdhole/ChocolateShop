using System.Collections.Generic;

namespace ChocolateShopApi.Models
{
    public class Brand: EntityBase
    {
        public Brand()
        {
            BrandCategories = new List<BrandCategory>();
        }

        public IList<BrandCategory> BrandCategories { get; set; }
    }
}

