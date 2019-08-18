using System.ComponentModel.DataAnnotations;

namespace ChocolateShopViewModels
{
    public class EntityImageViewModel: EntityViewModelBase
    {
        public byte[] Image { get; set; }
    }
}
