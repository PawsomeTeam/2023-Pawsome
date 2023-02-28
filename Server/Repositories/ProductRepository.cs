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
    
    public async Task<List<Image>> GetImages(int id)
    {
        var images = await pawsomeDbContext.Images.Where(o => o.Product.Id == id).ToListAsync();
        return images;
    }

    public async Task<ProductCategory> GetCategory(int id)
    {
        var category = await pawsomeDbContext.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
        return category;
    }

    public async Task<Product> CreateItem(ProductDto product)
    {
        var item = new Product
        {
            Name = product.Name,
            Description = product.Description,
            ImageURL = product.ImageURL,
            Price = product.Price,
            Images = new List<Image>(),
            Qty = product.Qty,
            CategoryId = product.CategoryId
        };

        foreach (var image in product.Images)
        {
            Image newImage = new Image
            {
                URL = image.URL,
                Type = image.Type
            };
            item.Images.Add(newImage);
        }
        
        if (item != null)
        {
            await pawsomeDbContext.Images.AddRangeAsync(item.Images);
            var result = await pawsomeDbContext.Products.AddAsync(item);
            await pawsomeDbContext.SaveChangesAsync();
            return result.Entity;
        }

        return null;
    }

    public async Task<Product> UpdateItem(int id, ProductDto product)
    {
        var item = await this.pawsomeDbContext.Products.FindAsync(id);
        if (item != null)
        {
            item.Name = product.Name;
            item.Description = product.Description;
            item.Price = product.Price;
            item.Qty = product.Qty;
            await this.pawsomeDbContext.SaveChangesAsync();
            return item;
        }

        return null;
    }

    public async Task<Product> DeleteItem(int id)
    {
        var item = await this.pawsomeDbContext.Products.FindAsync(id);
        if (item != null)
        {
            this.pawsomeDbContext.Products.Remove(item);
            await this.pawsomeDbContext.SaveChangesAsync();
        }

        return item;
    }
}