using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChocolateShopApi.Models
{
    public class Category: EntityBase
    {
        public Category()
        {
            BrandCategories = new List<BrandCategory>();
            CategoryProducts = new List<CategoryProduct>();
        }

        public IList<BrandCategory> BrandCategories { get; set; }
        public IList<CategoryProduct> CategoryProducts { get; set; }
    }
}

