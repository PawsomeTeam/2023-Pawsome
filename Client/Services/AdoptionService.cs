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
}