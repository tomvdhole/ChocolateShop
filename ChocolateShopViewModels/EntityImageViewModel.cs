using System.ComponentModel.DataAnnotations;

namespace ChocolateShopViewModels
{
    public class EntityImageViewModel:EntityViewModel
    {
        public byte[] Image { get; set; }
    }
}
