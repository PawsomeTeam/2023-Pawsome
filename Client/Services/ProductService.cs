using System.Net;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public class ProductService : IProductService
{
    private readonly HttpClient httpClient;
    private readonly IProductService api;

    public ProductService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<ProductDto>> GetItems()
    {
        try
        {
            var response = await this.httpClient.GetAsync("api/Product");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<ProductDto>().ToList();
                }

                return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ProductDto> GetItem(int id)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(ProductDto);
                }

                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);

            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ProductDto> CreateItem(ProductDto productDto)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<ProductDto>("api/Product", productDto);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(ProductDto);
                }

                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status : {response.StatusCode} Message - {message}");
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ProductDto> DeleteItem(int id)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }

            return default(ProductDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ProductDto> UpdateItem(ProductDto productDto)
    {
        try
        {
            var jsonRequest = JsonConvert.SerializeObject(productDto);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            var response = await httpClient.PatchAsync($"api/Product/{productDto.Id}", content);

            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductDto>();
            }
            return null;

        }
        catch (Exception)
        {
            //Log exception
            throw;
        }
    }
}