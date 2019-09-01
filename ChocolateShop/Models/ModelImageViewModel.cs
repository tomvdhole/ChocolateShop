using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChocolateShopApp.Models
{
    public class ModelImageViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IFormFile Image { get; set; }
    }
}
