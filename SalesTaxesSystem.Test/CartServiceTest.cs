using Microsoft.Extensions.Logging;
using Moq;
using SalesTaxesSystem.BLL.Services;
using SalesTaxesSystem.BLL.Services.Contracts;
using SalesTaxesSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace SalesTaxesSystem.Test
{
    public class CartServiceTest : IDisposable
    {
        private CartService _cartService;
        private Mock<ITaxService> _taxService;
        private Mock<ILogger<CartService>> _logger;
        private IList<Product> mockProducts;


        public CartServiceTest()
        {
            _taxService = new Mock<ITaxService>();
            _logger = new Mock<ILogger<CartService>>();

            mockProducts = CalculateTaxesMockResult();

            _taxService
                .Setup(p => p.CalculateTaxes(It.IsAny<List<Tax>>(), It.IsAny<List<Product>>()))
                .Returns(mockProducts);

            _cartService = new CartService(_logger.Object, _taxService.Object);
        }

        public void Dispose()
        {

        }

        [Theory]
        [MemberData(nameof(AddProductToCartData))]
        public void ShouldAddProductToCart(Cart cart, Product product, int quantity)
        {
            const int expected = 1;
            var result = _cartService.AddProduct(cart, product, quantity).Products.Count;

            Assert.True(result >= expected);
        }

        [Theory]
        [MemberData(nameof(CheckoutData))]
        public void ShouldGenerateReceiptWithCorrectTaxes(Cart cart, IList<Tax> taxes)
        {
            var expected = mockProducts.Sum(x => x.Quantity * x.Taxes);

            var result = _cartService.CheckOutCart(cart, taxes).SalesTaxes;

            Assert.Equal(expected, result);

        }

        [Theory]
        [MemberData(nameof(CheckoutData))]
        public void ShouldGenerateReceiptWithCorrectTotal(Cart cart, IList<Tax> taxes)
        {
            var expected = mockProducts.Sum(x => x.Quantity * x.TaxPrice);

            var result = _cartService.CheckOutCart(cart, taxes).Total;

            Assert.Equal(expected, result);

        }

        [Theory]
        [MemberData(nameof(CheckoutData))]
        public void ShouldGenerateReceiptWithCartProducts(Cart cart, IList<Tax> taxes)
        {
            var expectedProducts = mockProducts;

            var result = _cartService.CheckOutCart(cart, taxes).Products;

            Assert.Equal(expectedProducts.Count, result.Count);

            for (int i = 0; i < expectedProducts.Count; i++)
            {
                Assert.Equal(expectedProducts[i].Id, result[i].Id);
                Assert.Equal(expectedProducts[i].Name, result[i].Name);
                Assert.Equal(expectedProducts[i].Price, result[i].Price);
                Assert.Equal(expectedProducts[i].Quantity, result[i].Quantity);
                Assert.Equal(expectedProducts[i].Taxes, result[i].Taxes);
                Assert.Equal(expectedProducts[i].TaxPrice, result[i].TaxPrice);
            }
        }

        public static IEnumerable<object[]> AddProductToCartData()
        {
            yield return new object[]
            {
                new Cart(),
                new Product()
                {
                    Id = 15,
                    Name ="Peanut Butter",
                    Classes = new List<string>(){ "Food" },
                    Price = 2.99M
                },
                3
            };
        }

        public static IEnumerable<object[]> CheckoutData()
        {
            yield return new object[]
            {
                new Cart()
                {
                    Products = new List<Product>()
                    {
                        new Product()
                        {
                            Id = 15,
                            Name ="Peanut Butter",
                            Classes = new List<string>(){ "Food", "Junk" },
                            Quantity = 2,
                            Price = 2.99M
                        },
                        new Product()
                        {
                            Id = 15,
                            Name ="Tomatoes",
                            Classes = new List<string>(){ "Food", "Vegetables" },
                            Quantity = 3,
                            Price = 2.99M
                        },
                    }
                },
                new List<Tax>()
                {
                    new Tax()
                    {
                        Name = "Basic Tax",
                        Rate= 0.10M,
                        Classes = new List<string>() { "All" },
                        Exceptions= new List<string>() { "" }
                    },
                    new Tax()
                    {
                        Name = "Food tax",
                        Rate= 0.10M,
                        Classes = new List<string>() { "Food" },
                        Exceptions= new List<string>() { "Vegetables" }
                    }
                }
            };
        }
        
        public static IList<Product> CalculateTaxesMockResult()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = 15,
                    Name ="Peanut Butter",
                    Classes = new List<string>(){ "Food", "Junk" },
                    Quantity = 2,
                    Price = 2.99M,
                    Taxes = 0.60M,
                    TaxPrice = 3.59M

                },
                new Product()
                {
                    Id = 15,
                    Name ="Tomatoes",
                    Classes = new List<string>(){ "Food", "Vegetables" },
                    Quantity = 3,
                    Price = 2.99M,
                    Taxes = 0.30M,
                    TaxPrice = 3.29M
                }
            };
        }
    }

}
