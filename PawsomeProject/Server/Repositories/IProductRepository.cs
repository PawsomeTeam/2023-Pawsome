using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetItems();
    Task<IEnumerable<ProductCategory>> GetCategories();
    Task<Product> GetItem(int id);
    Task<ProductCategory> GetCategory(int id);
    
    Task<Product> CreateItem(ProductDto product);
    Task<Product> UpdateItem(int id, ProductDto product);
    Task<Product> DeleteItem(int id);
}