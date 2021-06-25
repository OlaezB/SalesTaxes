using System.Collections.Generic;

namespace SalesTaxesSystem.DAL.Models
{
    public class Cart
    {
        public float Total { get; set; }
        public float SalesTaxes { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
