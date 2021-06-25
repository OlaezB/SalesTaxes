using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesTaxesSystem.BLL.Services.Contracts;
using SalesTaxesSystem.DAL.Models;
using SalesTaxesSystem.DAL.Repositories.Contracts;
using SalesTaxesSystem.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesTaxesSystem
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly IConfiguration _config;
        private readonly ICartService _cartService;
        private readonly IParseService _parseService;
        private readonly IProductRepository _productRepository;
        private readonly ITaxRepository _taxRepository;

        private Dictionary<string, string> commands;
        private Cart cart;
        private IList<Tax> taxes;
        private IList<Product> products;
        private string parsedTaxes;
        private string parsedCatalog;
        private string commandString;

        public App(
            ILogger<App> logger,
            IConfiguration configuration,
            ICartService cartService,
            IParseService parseService,
            IProductRepository productRepository,
            ITaxRepository taxRepository
            )
        {
            _logger = logger;
            _config = configuration;
            _cartService = cartService;
            _parseService = parseService;
            _productRepository = productRepository;
            _taxRepository = taxRepository;

            cart = new Cart();
            taxes = new List<Tax>();
            products = new List<Product>();
            commandString = "";
            parsedCatalog = "";
        }

        public void Run()
        {
            SetUp();

            PrintCatalog(false);

            bool exit = false;

            var input = "";
            do
            {
                input = Console.ReadLine();

                if (commands.ContainsKey(input))
                    exit = Act(input);
                else if (!CanConvert(input))
                    continue;
                else
                    AddProcuct(input);


            } while (!exit);

            Console.WriteLine("Thanks for buying");
            Console.ReadLine();
        }

        public void SetUp()
        {
            commands = _config.GetSection("Commands").Get<Dictionary<string, string>>();

            products = _productRepository.Get();
            taxes = _taxRepository.Get();

            if (products == null)
            {
                products = new List<Product>();
                Console.WriteLine("There was an error reading products from source, setting up an empty list instead\n\n");
            }

            if (taxes == null)
            {
                taxes = new List<Tax>();
                Console.WriteLine("There was an error reading taxes from source, setting up an empty list instead\n\n");
            }

            parsedTaxes = _parseService.ParseTaxes(taxes);
            parsedCatalog = _parseService.ParseCatalog(products);
            commandString = String.Join(", or ", commands.Select(cmd => $"[{cmd.Key}] to {cmd.Value}"));
        }

        public void AddProcuct(string input)
        {
            string msg = $"Thats not a valid input, enter either a valid product Id";
            long productId = GetLongInput(input, msg);

            var product = products.FirstOrDefault(i => i.Id == productId);
            if (product == null)
            {
                PrintCatalog();
                Console.WriteLine();
                msg = $"Id {productId} not found in catalog, check if you entered it right or try another product";
                Console.WriteLine(msg);
                return;
            }

            msg = $"How many of {product.Name} do you want to add to your cart?";
            Console.WriteLine(msg);
            input = Console.ReadLine();
            msg = $"Thats not a valid input, please enter an integer number\n" + msg;
            int quantity = GetIntInput(input, msg);

            _cartService.AddProduct(cart, product, quantity);
            PrintCatalog();
            return;
        }

        public bool Act(string input)
        {
            switch (input)
            {
                case "c":
                    if (!cart.Products.Any())
                    {
                        PrintCatalog();
                        Console.WriteLine("Your cart is empty, please add some products before checking out");
                        return false;
                    }
                    var receipt = _cartService.CheckOutCart(cart, taxes);
                    var receiptString = _parseService.ParseReceipt(receipt);
                    Console.WriteLine(receiptString);
                    Console.WriteLine("Do you want to start a new transaction? [y]/[N]");
                    input = Console.ReadLine();
                    if (input == "y")
                    {
                        PrintCatalog();
                        cart.Products = new List<Product>();
                    }

                    return input != "y";

                case "r":
                    PrintCatalog(true);
                    return false;

                case "t":
                    PrintCatalog(true, true);
                    return false;

                case "e":
                    return true;

                default:
                    Console.WriteLine("Sorry, the functionality to this command hasn't been implemented yet. Try again with another command");
                    return false;
            }
        }

        public void PrintCatalog(bool clear = true, bool taxes = false)
        {
            if (clear) Console.Clear();
            Console.WriteLine("Welcome to the Sales Tax Calculator System");
            Console.WriteLine("Products that can be added to Cart");
            Console.WriteLine(parsedCatalog);
            if (taxes)
            {
                Console.WriteLine("Taxes registered");
                Console.WriteLine(parsedTaxes);
            }
            Console.WriteLine("Add your products by typing the Id of the product, press enter and when prompted enter the quantity. \nOr enter any of the following commmands " + commandString);
        }

        public bool CanConvert(string input)
        {
            long value;
            if (long.TryParse(input, out value))
                return true;

            var parsedCatalog = _parseService.ParseCatalog(products);
            PrintCatalog();
            Console.WriteLine();
            Console.WriteLine($"Thats not a valid input, enter either an product Id, {commandString}");
            return false;
        }

        public long GetLongInput(string input, string msg)
        {
            long value;
            while (!long.TryParse(input, out value))
            {
                var parsedCatalog = _parseService.ParseCatalog(products);
                PrintCatalog();
                Console.WriteLine();
                Console.WriteLine(msg);
                input = Console.ReadLine();
            }

            return value;
        }

        public int GetIntInput(string input, string msg)
        {
            int value;
            while (!int.TryParse(input, out value))
            {
                var parsedCatalog = _parseService.ParseCatalog(products);
                PrintCatalog();
                Console.WriteLine();
                Console.WriteLine(msg);
                input = Console.ReadLine();
            }

            return value;
        }
    }
}
