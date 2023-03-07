using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IAdoptionService
{
    Task<AdoptionSubmissionDto> SubmitAdoptionRequest(AdoptionSubmissionDto AdoptionSubmissionDto);
    Task<List<AdoptionDetailsForAdminDto>> GetAllAdoptions();
    Task<List<AdoptionDetailsForAdopterDto>> GetAllAdoptionsForCurrentUser();
    Task<AdoptionDetailsForAdminDto> GetAdoption(int id);
    Task<AdoptionDetailsForAdminDto> UpdateAdoption(AdoptionDetailsForAdminDto AdoptionSubmissionDto);
    Task<AdoptionDetailsForAdminDto> DeleteAdoption(int id);
}