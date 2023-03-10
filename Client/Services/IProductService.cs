using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetItems();
    Task<ProductDto> GetItem(int id);
    Task<ProductDto> CreateItem(ProductDto productDto);

    Task DeleteItem(int id); 
    Task<ProductDto> UpdateItem(ProductDto productDto);
    Task<List<ProductCategoryDto>> GetCategories();
    string fileName(string url);
} 