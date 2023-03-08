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

    public async Task<OrderDto> GetItem(string email)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Order/{email}");
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
                throw new Exception(message);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<OrderDto>> AllItems()
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Order/AllItems");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<OrderDto>().ToList();
                }

                return await response.Content.ReadFromJsonAsync<List<OrderDto>>();
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

    public async Task<List<OrderDto>> GetItems(string email)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Order/{email}/GetItems");
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return Enumerable.Empty<OrderDto>().ToList();
                }

                return await response.Content.ReadFromJsonAsync<List<OrderDto>>();
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}