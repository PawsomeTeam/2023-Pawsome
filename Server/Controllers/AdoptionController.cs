using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawsomeProject.Server.Models;
using PawsomeProject.Server.Repositories;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AdoptionController : ControllerBase
{
    private readonly IAdoptionRepository _adoptionRepo;
    private readonly UserManager<User> _userManager;

    public AdoptionController(
        IConfiguration configuration,
        IAdoptionRepository adoptionRepo,
        UserManager<User> userManager
    )
    {
        _adoptionRepo = adoptionRepo;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AdoptionSubmissionDto>> SubmitAdoptionRequest(AdoptionSubmissionDto submission)
    {
        Adoption? newAdoption = await _adoptionRepo.Create(submission);
        if (newAdoption == null)
            throw new Exception("Failed to create adoption request");

        AdoptionSubmissionDto newSubmission = new()
        {
            Id = newAdoption.Id,
            AdopteeId = newAdoption.Adoptee.Id,
            AdopterEmail = newAdoption.Adopter.Email,
        };
        return newSubmission;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<AdoptionDetailsForAdminDto>>> GetAll()
    {
        try
        {
            IEnumerable<Adoption>? fullAdoptionsList;
            List<AdoptionDetailsForAdminDto> adminAdoptionsList;
            if (User.IsInRole("Admin"))
            {
                fullAdoptionsList = await _adoptionRepo.GetAll();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                fullAdoptionsList = await _adoptionRepo.GetAllByUser(user);
            }

            if (fullAdoptionsList == null || fullAdoptionsList.Count() < 1)
                return adminAdoptionsList = new List<AdoptionDetailsForAdminDto>();

            adminAdoptionsList = fullAdoptionsList.Select(a => new AdoptionDetailsForAdminDto
            {
                Id = a.Id,
                AdopteeId = a.Adoptee.Id,
                AdopteeName = a.Adoptee.Name,
                AdopterEmail = a.Adopter.Email,
                AdopterFisrtName = a.Adopter.FirstName,
                AdopterLastName = a.Adopter.LastName,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                StartProcessingAt = a.StartProcessingAt,
                CompletedAt = a.CompletedAt,
                CanceledAt = a.CanceledAt,
                NoteForAdministration = a.NoteForAdministration,
                NoteForAdopter = a.NoteForAdopter,
            }).ToList();

            return adminAdoptionsList;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }
}