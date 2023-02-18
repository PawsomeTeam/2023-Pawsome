using PawsomeProject.Server.Models;

namespace PawsomeProject.Server.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetItems();
    Task<IEnumerable<ProductCategory>> GetCategories();
    Task<Product> GetItem(int id);
    Task<ProductCategory> GetCategory(int id);
}