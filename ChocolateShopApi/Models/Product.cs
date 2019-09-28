using System.Collections.Generic;

namespace ChocolateShopApi.Models
{
    public class Product: EntityBase
    {
        public byte[] Image { get; set; }

        public Product()
        {
            CategoryProducts = new List<CategoryProduct>();
        }

        public IList<CategoryProduct> CategoryProducts { get; set; }
    }
}

