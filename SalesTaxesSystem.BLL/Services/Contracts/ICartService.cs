using SalesTaxesSystem.DAL.Models;
using System.Collections.Generic;

namespace SalesTaxesSystem.BLL.Services.Contracts
{
    public interface ICartService
    {
        Cart AddProduct(Cart cart, Product product, int quantity);
        Receipt CheckOutCart(Cart cart, IList<Tax> taxes);
    }
}
