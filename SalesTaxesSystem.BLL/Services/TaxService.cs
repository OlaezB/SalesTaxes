using SalesTaxesSystem.BLL.Services.Contracts;
using SalesTaxesSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesTaxesSystem.BLL.Services
{
    public class TaxService : ITaxService
    {
        public IList<Product> CalculateTaxes(IList<Tax> taxes, IList<Product> products)
        {
            var taxList = taxes.ToList();
            var productList = products.ToList();

            taxList.ForEach(tax =>
            {
                var applicableProductsForTax = productList.Where(product => ShouldApplyTax(tax, product)).ToList();
                applicableProductsForTax
                    .ForEach(product => product.Taxes += CalculateTaxValue(product.Price, tax.Rate));
            });
            productList.ForEach(product => product.TaxPrice = product.Price + product.Taxes);

            return products;
        }

        public bool ShouldApplyTax(Tax tax, Product product)
        {
            if (product.Classes.Intersect(tax.Exceptions).Any())
                return false;

            if (tax.Classes.Contains("All"))
                return true;

            if (product.Classes.Intersect(tax.Classes).Any())
                return true;

            return false;
        }

        public Decimal CalculateTaxValue (Decimal price, Decimal taxValue)
        {
            var tax = price * taxValue;
            var taxRounded = Math.Ceiling(tax * 20) / 20;

            return taxRounded;
        }
    }
}
