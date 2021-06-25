using System;
using System.Collections.Generic;

namespace SalesTaxesSystem.DAL.Models
{
    public class Tax
    {
        public string Name { get; set; }
        public Decimal Rate { get; set; }
        public ICollection<string> Classes { get; set; } = new List<string>();
        public ICollection<string> Exceptions { get; set; } = new List<string>();
    }
}
