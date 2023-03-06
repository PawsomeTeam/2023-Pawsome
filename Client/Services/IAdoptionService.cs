using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IAdoptionService
{
    Task<AdoptionSubmissionDto> SubmitAdoptionRequest(AdoptionSubmissionDto AdoptionSubmissionDto);
    Task<List<AdoptionDetailsForAdminDto>> GetAllAdoptions();
    Task<AdoptionDetailsForAdminDto> GetAdoption(int id);
}