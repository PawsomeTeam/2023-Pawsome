using PawsomeProject.Shared.Models;
using System.Net.Http.Json;

namespace PawsomeProject.Client.Services
{
    public class HttpBasedAnimalService : IAnimalService
    {
        private readonly HttpClient _httpClient;

        public HttpBasedAnimalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Animal>> GetAll()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Animal>>("api/animal");
            return response;
        }
    }
}
