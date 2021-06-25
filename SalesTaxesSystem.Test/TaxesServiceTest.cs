using SalesTaxesSystem.BLL.Services;
using SalesTaxesSystem.DAL.Models;
using System;
using System.Collections.Generic;
using Xunit;


namespace SalesTaxesSystem.Test
{
    public class TaxesServiceTest : IDisposable
    {
        private TaxService _taxService;

        public TaxesServiceTest()
        {
            _taxService = new TaxService();
        }

        public void Dispose()
        {

        }

        [Theory]
        [InlineData(14.99, 0.1, 1.5)]
        [InlineData(14.99, 0.05, 0.75)]
        [InlineData(5.60, 0.1, 0.60)]
        [InlineData(5.20, 0.1, 0.55)]
        public void CalculateTax_ShouldReturnPriceWithTaxRounded(Decimal price, Decimal taxValue, Decimal expected)
        {
            var result = _taxService.CalculateTaxValue(price, taxValue);
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(TaxHasClassAll))]
        public void ShouldApplyTax_ReturnsTrue_IfTaxHasClassAll(Product product, Tax tax)
        {
            const bool expected = true;

            var result = _taxService.ShouldApplyTax(tax, product);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(ProductSameClassAsTax))]
        public void ShouldApplyTax_ReturnsTrue_IfProductHasOneOfTaxClassees(Product product, Tax tax)
        {
            const bool expected = true;

            var result = _taxService.ShouldApplyTax(tax, product);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(ProductClassNotInTaxClasses))]
        public void ShouldApplyTax_ReturnsFalse_IfProductDoesntHaveTaxClass(Product product, Tax tax)
        {
            const bool expected = false;

            var result = _taxService.ShouldApplyTax(tax, product);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(ProductSameClassAsTaxException))]
        public void ShouldApplyTax_ReturnsFalse_IfProductHasClassTaxExcepts(Product product, Tax tax)
        {
            const bool expected = false;

            var result = _taxService.ShouldApplyTax(tax, product);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(ProductsAndTaxesToBeAppliedTo))]
        public void CalculateTaxes_ShouldReturnProductsWithUpdatedTaxValues(List<Product> products, List<Tax> taxes, List<Product> expectedProducts)
        {
            var result = _taxService.CalculateTaxes(taxes, products);

            for (int i = 0; i < expectedProducts.Count; i++)
            {
                Assert.Equal(expectedProducts[i].Id, result[i].Id);
                Assert.Equal(expectedProducts[i].TaxPrice, result[i].TaxPrice);
            }
        }

        public static IEnumerable<object[]> ProductSameClassAsTax()
        {
            yield return new object[]
            {
                new Product()
                {
                    Name ="Dog Toy",
                    Price = 5.99M,
                    Classes = new List<string>() { "Pet toys" },
                    Quantity = 1
                },
                new Tax()
                {
                    Name = "Pet toys",
                    Classes = new List<string>() { "Pet toys"},
                    Exceptions = new List<string>(),
                    Rate = 0.15M
                }
            };
        }

        public static IEnumerable<object[]> TaxHasClassAll()
        {
            yield return new object[]
            {
                new Product()
                {
                    Name ="Dog Toy",
                    Price = 5.99M,
                    Classes = new List<string>() { "Pet toys" },
                    Quantity = 1
                },
                new Tax()
                {
                    Name = "Basic Tax",
                    Classes = new List<string>() { "All"},
                    Exceptions = new List<string>(),
                    Rate = 0.10M
                }
            };
        }

        public static IEnumerable<object[]> ProductSameClassAsTaxException()
        {
            yield return new object[]
            {
                new Product()
                {
                    Name ="Dog medicine",
                    Price = 5.99M,
                    Classes = new List<string>() { "Dog medicine" },
                    Quantity = 1
                },
                new Tax()
                {
                    Name = "Pets medicines",
                    Classes = new List<string>(),
                    Exceptions = new List<string>() { "Dog medicine" },
                    Rate = 0.10M
                }
            };
        }

        public static IEnumerable<object[]> ProductClassNotInTaxClasses()
        {
            yield return new object[]
            {
                new Product()
                {
                    Name ="Dog medicine",
                    Price = 5.99M,
                    Classes = new List<string>() { "Dog medicine" },
                    Quantity = 1
                },
                new Tax()
                {
                    Name = "Pets medicines",
                    Classes = new List<string>(),
                    Exceptions = new List<string>() { "Dog medicine" },
                    Rate = 0.10M
                }
            };
        }

        public static IEnumerable<object[]> ProductsAndTaxesToBeAppliedTo()
        {
            yield return new object[]
            {
                new List<Product>
                {
                    new Product()
                    {
                        Id = 15,
                        Name ="Dog medicine",
                        Price = 5.99M,
                        Classes = new List<string>() { "Dog medicine" },
                        Quantity = 1
                    }
                },
                new List<Tax>
                {
                    new Tax()
                    {
                        Name = "Pets medicines",
                        Classes = new List<string>() { "Dog medicine" },
                        Exceptions = new List<string>(),
                        Rate = 0.05M
                    }
                },
                new List<Product> // These are the updated products
                {
                    new Product()
                    {
                        Id = 15,
                        Name ="Dog medicine",
                        TaxPrice = 6.29M,
                        Classes = new List<string>() { "Dog medicine" },
                        Quantity = 1
                    }
                },
            };

            yield return new object[]
            {
                new List<Product>
                {
                    new Product()
                    {
                        Id = 15,
                        Name ="Dog Food",
                        Price = 10M,
                        Classes = new List<string>() { "Pet food" },
                        Quantity = 1
                    }
                },
                new List<Tax>
                {
                    new Tax()
                    {
                        Name = "Pet foods",
                        Classes = new List<string>() { "Pet food" },
                        Exceptions = new List<string>(),
                        Rate = 0.10M
                    }
                },
                new List<Product> // These are the updated products
                {
                    new Product()
                    {
                        Id = 15,
                        Name ="Dog medicine",
                        TaxPrice = 11M,
                        Classes = new List<string>() { "Dog medicine" },
                        Quantity = 1
                    }
                },
            };

            yield return new object[]
            {
                new List<Product>
                {
                    new Product()
                    {
                        Id = 15,
                        Name ="Dog toy",
                        Price = 3.70M,
                        Classes = new List<string>() { "Pets toys" },
                        Quantity = 1
                    }
                },
                new List<Tax>
                {
                    new Tax()
                    {
                        Name = "Basic tax",
                        Classes = new List<string>() { "All" },
                        Exceptions = new List<string>(),
                        Rate = 0.10M
                    }
                },
                new List<Product> // These are the updated products
                {
                    new Product()
                    {
                        Id = 15,
                        Name ="Dog medicine",
                        TaxPrice = 4.10M,
                        Classes = new List<string>() { "Dog medicine" },
                        Quantity = 1
                    }
                },
            };
        }
    }

}
