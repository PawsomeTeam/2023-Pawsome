using System.Net.Http.Json;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public class ShoppingCartService : IShoppingCartService
{
    private readonly HttpClient httpClient;

    public ShoppingCartService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<List<CartItemDto>> GetItems(int userId)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<CartItemDto>().ToList();
                }

                return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status : {response.StatusCode} Message - {message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CartItemDto> AddItem(CartItemAddToDto cartItemAddToDto)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<CartItemAddToDto>("api/ShoppingCart", cartItemAddToDto);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(CartItemDto);
                }

                return await response.Content.ReadFromJsonAsync<CartItemDto>();
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

    public async Task<CartItemDto> DeleteItem(int id)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/ShoppingCart/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CartItemDto>();
            }

            return default(CartItemDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}