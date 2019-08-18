using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocolateShopApp.Common.Options
{
    public class WebApiOptions
    {
        public string BaseUrl { get; set; }

        public string GetBrands { get; set; }
        public string GetCategories { get; set; }
        public string GetProducts { get; set; }

        public string GetBrand { get; set; }
        public string GetCategory { get; set; }
        public string GetProduct { get; set; }

        public string PostBrand { get; set; }
        public string PostCategory { get; set; }
        public string PostProduct { get; set; }

        public string PutBrand { get; set; }
        public string PutCategory { get; set; }
        public string PutProduct { get; set; }

        public string DeleteBrand { get; set; }
        public string DeleteCategory { get; set; }
        public string DeleteProduct { get; set; }
    }
}
