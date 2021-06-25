using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesTaxesSystem.DAL.Models;
using SalesTaxesSystem.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SalesTaxesSystem.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly IConfiguration _config;

        public ProductRepository(ILogger<ProductRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        public IList<Product> Get()
        {
            string jsonString = "";
            string productsFileName = _config.GetValue<string>("ProductsFile");
            try
            {
                jsonString = File.ReadAllText(productsFileName);
                var productList = JsonSerializer.Deserialize<List<Product>>(jsonString);
                return productList;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to fetch products from file {itemsFileName}", productsFileName);
                return null;
            }
        }
    }
}
