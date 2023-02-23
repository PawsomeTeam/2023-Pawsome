using Microsoft.AspNetCore.Components;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages.StoreManagement.Products;

public class DisplayProductBase:ComponentBase
{
    [Parameter]
    public IEnumerable<ProductDto> Products { get; set; }
}