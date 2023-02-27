using Microsoft.AspNetCore.Components;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages.StoreManagement.Products;

public class CreateProductBase : ComponentBase
{
    [Parameter]
    public int Id { get; set; }
    
    [Inject]
    public IProductService ProductService { get; set; }
    
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    public ProductDto Product { get; set; } = new ProductDto();
    
    public string ErrorMessage { get; set; }

  
    protected async Task Create_Product_Click(ProductDto product)
    {
        try
        {
            var productDto = new ProductDto
                {
                    Id = Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImageURL = product.ImageURL,
                    Price = product.Price,
                    Qty = product.Qty,
                    CategoryId = 1,
                    CategoryName = "Beauty"
                };

               await this.ProductService.CreateItem(productDto);
               NavigationManager.NavigateTo("");
        }
        catch (Exception)
        {
            throw;
        }

    }
    
    
}