using System.ComponentModel.DataAnnotations.Schema;

namespace ChocolateShopApi.Models
{
    public class Product: EntityBase
    {
        public byte[] Image { get; set; }

        [ForeignKey(nameof(Brand))]
        public int? BrandId { get; set; }
        public Brand Brand { get; set; }

        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}

