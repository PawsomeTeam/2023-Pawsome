using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using PawsomeProject.Client.Services;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Pages.StoreManagement.Products;

public class EditProductBase : ComponentBase
{
    [Parameter] public int Id { get; set; }

    [Inject] public HttpClient HttpClient { get; set; }

    [Inject] public IProductService ProductService { get; set; } = default!;

    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    public ProductDto? Product { get; set; }

    public string? ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Product = await ProductService.GetItem(Id);
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
        }
    }

    protected async Task Update_Product_Click(ProductDto product)
    {
        try
        {
            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.Images.ElementAt(0).URL,
                Images = product.Images,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.CategoryId,
                CategoryName = "Beauty"
            };

            var returnedUpdateItemDto = await ProductService.UpdateItem(productDto);
            NavigationManager.NavigateTo("");
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected async Task Delete_Product_Click(List<ImageDto> images, int id)
    {
        foreach (var image in images.ToList())
        {
            await HandleDeleteImage(image.URL, image.Id);
        }

        await ProductService.DeleteItem(id);
        NavigationManager.NavigateTo("");
    }


    public async Task HandleSelectedImage(InputFileChangeEventArgs e)
    {
        foreach (var file in e.GetMultipleFiles())
        {
            var resizedFile = await file.RequestImageFileAsync("image/png", 300, 500);

            using (var ms = resizedFile.OpenReadStream(resizedFile.Size))
            {
                var content = new MultipartFormDataContent();
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "image", file.Name);
                var response = await HttpClient.PostAsync("api/Product/UploadImage", content);

                var url = await response.Content.ReadAsStringAsync();
                var imageDto = new ImageDto
                {
                    URL = url,
                    Type = "type"
                };

                Product.Images.Add(imageDto);
            }
        }

        Product.ImageURL = Product.Images.ElementAt(0).URL;
    }

    protected async Task HandleDeleteImage(string url, int id)
    {
        var imageURL = url;
        var filename = ProductService.fileName(imageURL);
        var response = await HttpClient.DeleteAsync($"api/Product/{filename}");
        if (response.IsSuccessStatusCode)
        {
            var result = await HttpClient.DeleteAsync($"api/Product/DeleteEImage/{id}");
            if (result.IsSuccessStatusCode)
            {
                var imageDto = Product.Images.Find(i => i.URL == url);
                Product.Images.Remove(imageDto);
                Console.WriteLine("Image File is deleted");
            }
        }
    }
}