using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetItems();
    Task<ProductDto> GetItem(int id);
    Task<ProductDto> CreateItem(ProductDto productDto);

    Task<ProductDto> DeleteItem(int id); 
    Task<ProductDto> UpdateItem(ProductDto productDto); 
} 