using System;
using System.Collections.Generic;

namespace SalesTaxesSystem.DAL.Models
{
    public class Product
    {
        public Product()
        {

        }

        public Product(Product product, int quantity)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            TaxPrice = product.TaxPrice;
            Quantity = quantity;
            Classes = product.Classes;
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public Decimal Price { get; set; }
        public Decimal TaxPrice { get; set; }
        public Decimal Taxes { get; set; }
        public int Quantity { get; set; }
        public ICollection<string> Classes { get; set; } = new List<string>();
    }
}
