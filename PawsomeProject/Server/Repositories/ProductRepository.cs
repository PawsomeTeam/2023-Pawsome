using Microsoft.EntityFrameworkCore;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

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

    public async Task<Product> GetItem(int id)
    {
        var product = await pawsomeDbContext.Products.FindAsync(id);
        return product;
    }

    public async Task<ProductCategory> GetCategory(int id)
    {
        var category = await pawsomeDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
        return category;
    }

    public async Task<Product> AddItem(ProductDto product)
    {
        var item = new Product
        {
            Name = product.Name,
            Description = product.Description,
            ImageURL = product.ImageURL,
            Price = product.Price,
            Qty = product.Qty,
            CategoryId = product.CategoryId
        };
        if (item != null)
        {
            var result = await this.pawsomeDbContext.Products.AddAsync(item);
            await this.pawsomeDbContext.SaveChangesAsync();
            return result.Entity;
        }

        return null;
    }

    public Task<Product> UpdateItem(int id, ProductDto product)
    {
        throw new NotImplementedException();
    }

    public Task<Product> DeleteItem(int id)
    {
        throw new NotImplementedException();
    }
}