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
    public class TaxRepository : ITaxRepository
    {
        private readonly ILogger<TaxRepository> _logger;
        private readonly IConfiguration _config;

        public TaxRepository(ILogger<TaxRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        public IList<Tax> Get()
        {
            string jsonString = "";
            string itemsFileName = _config.GetValue<string>("TaxesFile");

            try
            {
                jsonString = File.ReadAllText(itemsFileName);
                var taxList = JsonSerializer.Deserialize<List<Tax>>(jsonString);
                return taxList;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error trying to fetch items from file {taxesFileName}", itemsFileName);
                return null;
            }

        }
    }
}
