using System.Collections.Generic;
using SalesTaxesSystem.DAL.Models;

namespace SalesTaxesSystem.Services.Contracts
{
    public interface IParseService
    {
        string ParseCatalogProduct(Product product);
        string ParseCatalog(IList<Product> list);
        string ParseReceiptProduct(Product product);
        string ParseReceipt(Receipt receipt);
        string ParseTaxItem(Tax tax);
        string ParseTaxes(IList<Tax> list);
    }
}