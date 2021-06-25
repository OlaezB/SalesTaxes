using SalesTaxesSystem.DAL.Models;
using System;
using System.Collections.Generic;

namespace SalesTaxesSystem.BLL.Services.Contracts
{
    public interface ITaxService
    {
        IList<Product> CalculateTaxes(IList<Tax> taxes, IList<Product> products);
        Decimal CalculateTaxValue(Decimal price, Decimal taxRate);
    }
}
