using System.Collections.Generic;

namespace MVC.Models.ViewModels
{
    public class CategoryDetailsViewModel
    {
        public Category CategoryData { get; set; }
        public IEnumerable<Product> ProductCollection { get; set; }
    }
}