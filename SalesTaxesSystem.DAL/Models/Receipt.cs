using System;
using System.Collections.Generic;

namespace SalesTaxesSystem.DAL.Models
{
    public class Receipt
    {
        public Decimal Total { get; set; }
        public Decimal SalesTaxes { get; set; }
        public IList<Product> Products { get; set; } = new List<Product>();
    }
}
