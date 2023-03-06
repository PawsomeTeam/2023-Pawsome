using PawsomeProject.Shared.Models;
using System.Net.Http.Json;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

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

        public async Task<AnimalDto> GetAnimalById(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Animal/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(AnimalDto);
                    }

                    return await response.Content.ReadFromJsonAsync<AnimalDto>();
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

        public async Task<AnimalDto> UpdateAnimal(AnimalDto animalDto)
        {
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(animalDto);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/Animal/{animalDto.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<AnimalDto>();
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string fileName(string url)
        {
            Match match = Regex.Match(url, @"([^/]+\.[^/]+)$");
            return match.Groups[1].Value;
        }
    }
}