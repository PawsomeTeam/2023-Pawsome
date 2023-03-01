using PawsomeProject.Shared.Models;
using System.Net.Http.Json;
using System.Net;
using System.Net.Http;

namespace PawsomeProject.Client.Services
{
    public class HttpBasedAnimalService : IAnimalService
    {
        private readonly HttpClient _httpClient;

        public HttpBasedAnimalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AnimalDto>> GetAll()
        {
            var response = await _httpClient.GetFromJsonAsync<List<AnimalDto>>("api/animal");
            return response;
        }

        public async Task<AnimalDto> AddAnimal(AnimalDto animalDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<AnimalDto>("api/animal", animalDto);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(AnimalDto);
                    }

                    return await response.Content.ReadFromJsonAsync<AnimalDto>();
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
        public async Task DeleteAnimal(int id)
        {
            try
            {
                var result = await _httpClient.DeleteAsync($"api/animal/{id}");
                if (result.StatusCode == HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

       
    }
}