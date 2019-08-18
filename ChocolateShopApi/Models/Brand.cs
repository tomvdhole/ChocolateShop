using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChocolateShopApi.Models
{
    public class Brand: EntityBase
    {
        public Brand()
        {
            Products = new List<Product>();
        }

        [ForeignKey("BrandId")]
        public IEnumerable<Product> Products { get; set; }
    }
}

