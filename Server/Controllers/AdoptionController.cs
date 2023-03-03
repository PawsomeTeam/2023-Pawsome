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
}