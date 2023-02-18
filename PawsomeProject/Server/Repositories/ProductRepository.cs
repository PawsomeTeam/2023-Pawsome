using Microsoft.EntityFrameworkCore;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Models;

namespace PawsomeProject.Server.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly PawsomeDBContext pawsomeDbContext;

    public ProductRepository(PawsomeDBContext pawsomeDbContext)
    {
        this.pawsomeDbContext = pawsomeDbContext;
    }
    public async Task<IEnumerable<Product>> GetItems()
    {
        var products = await this.pawsomeDbContext.Products.ToListAsync();
        return products;
    }

    public async Task<IEnumerable<ProductCategory>> GetCategories()
    {
        var categories = await this.pawsomeDbContext.ProductCategories.ToListAsync();
        return categories;
    }

    public Task<Product> GetItem(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ProductCategory> GetCategory(int id)
    {
        throw new NotImplementedException();
    }
}