using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public class AdoptionService : IAdoptionService
{
    private readonly HttpClient httpClient;

    public AdoptionService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<AdoptionSubmissionDto> SubmitAdoptionRequest(AdoptionSubmissionDto AdoptionSubmissionDto)
    {
        var result = await httpClient.PostAsJsonAsync("api/Adoption/SubmitAdoptionRequest", AdoptionSubmissionDto);
        if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
        AdoptionSubmissionDto? submission = await result.Content.ReadFromJsonAsync<AdoptionSubmissionDto>();
        if (submission == null) throw new Exception("Failed to retreaave adoption submission");
        return submission;
    }


    public async Task<List<AdoptionDetailsForAdminDto>> GetAllAdoptions()
    {
        var result = await httpClient.GetFromJsonAsync<List<AdoptionDetailsForAdminDto>>("api/Adoption/GetAll");
        if (result == null) throw new Exception("no adoptions found");
        return result;
    }
    public async Task<AdoptionDetailsForAdminDto> GetAdoption(int id)
    {
        var result = await httpClient.GetFromJsonAsync<AdoptionDetailsForAdminDto>($"api/Adoption/Get/{id}");
        if (result == null) throw new Exception("no adoptions found");
        return result;
    }
}