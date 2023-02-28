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

        public async Task<Animal> AddAnimal(Animal animal)
        {
            var response = await _httpClient.PostAsJsonAsync<Animal>("api/animal", animal);
            return await response.Content.ReadFromJsonAsync<Animal>();
        }


    }
}
