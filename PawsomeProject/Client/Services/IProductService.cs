using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetItems();
    Task<ProductDto> GetItem(int id);
    Task<ProductDto> AddItems(ProductDto productDto);

} 