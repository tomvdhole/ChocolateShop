using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChocolateShopApi.Models
{
    public class Category: EntityBase
    {
        public Category()
        {
            Products = new List<Product>();
        }

        [ForeignKey("CategoryId")]
        public IEnumerable<Product> Products { get; set; }
    }
}

