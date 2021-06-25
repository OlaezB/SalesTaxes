using SalesTaxesSystem.DAL.Models;
using SalesTaxesSystem.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesTaxesSystem.Services
{
    public class ParseService : IParseService
    {
        private string taxTabFormatter;
        private string cartTabFormatter;
        private string catalogTabFormatter;

        private Dictionary<string, int> taxColumns = new Dictionary<string, int> { { "Name", -20 }, { "Rate", -20 }, { "Applies to", -20 }, { "Excepts", -20 } };
        private Dictionary<string, int> catalogColumns = new Dictionary<string, int>{ { "Id", -5 }, { "Product Name", -20}, { "Price", -8}, { "Categories", -20 } };
        private Dictionary<string, int> receiptColumns = new Dictionary<string, int>{ { "Description", -20 }, { "Price", -20 } };

        public ParseService()
        {
            taxTabFormatter = Format(taxColumns);
            cartTabFormatter = Format(receiptColumns);
            catalogTabFormatter = Format(catalogColumns);
        }

        private string Format(Dictionary<string, int> columns)
        {
            string stringFormat = "";
            for (int i = 0; i < columns.Count; i++)
                stringFormat += "{{" + i + ",{" + i + "}}} ";

            stringFormat += "\n";

            var columnsWidth = columns.Values
                .Select(x => x.ToString())
                .ToArray();

            return string.Format(stringFormat, columnsWidth);
        }

        public string ParseCatalog(IList<Product> list)
        {
            string[] columnNames = catalogColumns.Keys.ToArray();
            string catalogString = string.Format(catalogTabFormatter, columnNames);


            foreach (var product in list)
            {
                catalogString += ParseCatalogProduct(product);
            }

            return catalogString;
        }

        public string ParseCatalogProduct(Product product)
        {
            return string.Format(catalogTabFormatter, product.Id, product.Name, product.Price.ToString("C2"), string.Join(",", product.Classes));
        }

        public string ParseReceipt(Receipt receipt)
        {
            int tableWidth = Math.Abs(receiptColumns.Values.Sum());
            string[] columnNames = receiptColumns.Keys.ToArray();
            string receiptString = string.Format(cartTabFormatter, columnNames);

            foreach (var product in receipt.Products)
            {
                receiptString += ParseReceiptProduct(product);
            }

            receiptString += $"\n{("Sale Taxes: " + receipt.SalesTaxes.ToString("C2")).PadLeft(tableWidth)}";
            receiptString += $"\n{("Total: " + receipt.Total.ToString("C2")).PadLeft(tableWidth)}";

            return receiptString;
        }

        public string ParseReceiptProduct(Product product)
        {
            string itemPrice = (product.TaxPrice * product.Quantity).ToString("C2");
            string itemString = itemPrice + (product.Quantity > 1 ? $" ({product.Quantity} @ {product.TaxPrice.ToString("C2")})" : "");

            return string.Format(cartTabFormatter, product.Name, itemString);
        }

        public string ParseTaxes(IList<Tax> list)
        {
            string[] columnNames = taxColumns.Keys.ToArray();
            string catalogString = string.Format(taxTabFormatter, columnNames);


            foreach (var tax in list)
            {
                catalogString += ParseTaxItem(tax);
            }

            return catalogString;
        }

        public string ParseTaxItem(Tax tax)
        {
            return string.Format(taxTabFormatter, tax.Name, tax.Rate.ToString("C2"), string.Join(',', tax.Classes), string.Join(',', tax.Exceptions));
        }
    }
}
