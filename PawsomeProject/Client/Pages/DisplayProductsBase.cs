using Microsoft.AspNetCore.Components;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages;

public class DisplayProductBase:ComponentBase
{
    [Parameter]
    public IEnumerable<ProductDto> Products { get; set; }
}