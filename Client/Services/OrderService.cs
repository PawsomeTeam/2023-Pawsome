using System.Net.Http.Json;
using PawsomeProject.Shared.Models;
using System.Text.Json.Serialization;

namespace PawsomeProject.Client.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient httpClient;

    public OrderService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public Task<OrderItemDto> GetItem(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderItemDto> AddItem(OrderItemAddToDto orderItemAddToDto)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<OrderItemAddToDto>("api/Order", orderItemAddToDto);
            
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(OrderItemDto);
                }

                return await response.Content.ReadFromJsonAsync<OrderItemDto>();
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
    
}