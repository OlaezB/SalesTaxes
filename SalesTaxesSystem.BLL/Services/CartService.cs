using Microsoft.Extensions.Logging;
using SalesTaxesSystem.BLL.Services.Contracts;
using SalesTaxesSystem.DAL.Models;
using System.Collections.Generic;
using System.Linq;

namespace SalesTaxesSystem.BLL.Services
{
    public class CartService : ICartService
    {
        private readonly ILogger<CartService> _logger;
        private readonly ITaxService _taxService;

        public CartService(ILogger<CartService> logger,ITaxService taxService)
        {
            _logger = logger;
            _taxService = taxService;
        }

        public Cart AddProduct(Cart cart, Product product, int quantity)
        {
            cart.Products.Add(new Product(product, quantity));
            return cart;
        }

        public Receipt CheckOutCart(Cart cart, IList<Tax> taxes)
        {
            var products = _taxService.CalculateTaxes(taxes, cart.Products.ToList());

            var saleTaxes = products.Sum(product => product.Quantity * product.Taxes);
            var saleTotal = products.Sum(product => product.Quantity * product.TaxPrice);

            return new Receipt()
            {
                Total = saleTotal,
                SalesTaxes = saleTaxes,
                Products = products
            };
        }
    }
}
