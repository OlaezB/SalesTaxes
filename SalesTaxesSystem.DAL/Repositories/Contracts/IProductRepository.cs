using SalesTaxesSystem.DAL.Models;
using System.Collections.Generic;

namespace SalesTaxesSystem.DAL.Repositories.Contracts
{
    public interface IProductRepository
    {
        IList<Product> Get();
    }
}
