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

    public Task<OrderDto> GetItem(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<OrderDto> AddItem(OrderDto orderDto)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<OrderDto>("api/Order", orderDto);
            
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return default(OrderDto);
                }

                return await response.Content.ReadFromJsonAsync<OrderDto>();
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