using SalesTaxesSystem.DAL.Models;
using SalesTaxesSystem.Services;
using System;
using System.Collections.Generic;
using Xunit;


namespace SalesTaxesSystem.Test
{
    public class ParseServiceTest : IDisposable
    {
        ParseService _parseService;

        public ParseServiceTest()
        {
            _parseService = new ParseService();
        }

        public void Dispose()
        {

        }

        [Fact]
        public void ParseCatalogProduct_ShouldReturnProductParsedToString()
        {
            string expected = "15    Soap bar             $1.50    Soap                 \n";
            var result = _parseService.ParseCatalogProduct(product);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ParseCatalog_ShouldReturnProductsListParsedToString()
        {
            string expected = "Id    Product Name         Price    Categories           \n15    Soap bar             $1.50    Soap                 \n15    Liquid Soap          $2.00    Soap                 \n15    Dish shoap           $1.00    Soap                 \n";

            var result = _parseService.ParseCatalog(products);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ParseProduct_ShouldReturnProductParsed()
        {
            string expected = "Soap bar             $3.30 (2 @ $1.65)    \n";

            var result = _parseService.ParseReceiptProduct(product);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ParseReceipt_ShouldReturnReceiptParsed()
        {
            string expected = "Description          Price                \nSoap bar             $3.30 (2 @ $1.65)    \nLiquid Soap          $4.60 (2 @ $2.30)    \nDish shoap           $2.30 (2 @ $1.15)    \n\n                       Sale Taxes: $0.90\n                            Total: $9.90";

            var result = _parseService.ParseReceipt(receipt);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ParseTaxItem_ShouldReturnTaxParsed()
        {
            string expected = "Basic Tax            $0.10                All                                       \n";

            var result = _parseService.ParseTaxItem(tax);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ParseTaxes_ShouldReturnTaxListParsed()
        {
            string expected = "Name                 Rate                 Applies to           Excepts              \nBasic Tax            $0.10                All                  Food,Book,Medical    \nImportation Tax      $0.05                Imported                                  \n";

            var result = _parseService.ParseTaxes(taxes);

            Assert.Equal(expected, result);
        }

        private static Tax tax = new Tax
        {
            Name = "Basic Tax",
            Rate = 0.10M,
            Classes = new List<string>() { "All" },
            Exceptions = new List<string>()
        };

        private static List<Tax> taxes = new List<Tax>
        {
            new Tax
                {
                    Name = "Basic Tax",
                    Rate = 0.10M,
                    Classes = new List<string>() { "All" },
                    Exceptions = new List<string>() { "Food", "Book", "Medical" }
                },
                new Tax
                {
                    Name = "Importation Tax",
                    Rate = 0.05M,
                    Classes = new List<string>() { "Imported" },
                    Exceptions = new List<string>()
                }
        };

        private static Product product = new Product
        {
            Id = 15,
            Name = "Soap bar",
            Price = 1.50M,
            Quantity = 2,
            Classes = new List<string>() { "Soap" },
            Taxes = 0.15M,
            TaxPrice = 1.65M
        };

        private static List<Product> products = new List<Product>
        {
            new Product
            {
                Id = 15,
                Name = "Soap bar",
                Price = 1.50M,
                Quantity = 2,
                Classes = new List<string>() { "Soap" },
                Taxes = 0.15M,
                TaxPrice = 1.65M
            },
            new Product
            {
                Id = 15,
                Name = "Liquid Soap",
                Price = 2.00M,
                Quantity = 2,
                Classes = new List<string>() { "Soap" },
                Taxes = 0.15M,
                TaxPrice = 2.30M
            },
            new Product
            {
                Id = 15,
                Name = "Dish shoap",
                Price = 1.00M,
                Quantity = 2,
                Classes = new List<string>() { "Soap" },
                Taxes = 0.15M,
                TaxPrice = 1.15M
            }
        };

        private Receipt receipt = new Receipt
        {
            Products = products,
            Total = 9.90M,
            SalesTaxes = 0.90M
        };
    }
}
