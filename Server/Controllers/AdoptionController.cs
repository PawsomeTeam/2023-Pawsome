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
    [HttpPost]
    public async Task<ActionResult<AdoptionDetailsForAdminDto>> Update(AdoptionDetailsForAdminDto newUpdate)
    {
        Adoption? a = await _adoptionRepo.Update(newUpdate);
        if (a == null)
            throw new Exception("Failed to update adoption");

        AdoptionDetailsForAdminDto updatedAdoption = new()
        {
            Id = a.Id,
            AdopteeId = a.Adoptee.Id,
            AdopteeName = a.Adoptee.Name,
            AdopteeType = a.Adoptee.Type,
            AdoppteeMainImageURL = a.Adoptee.Main_Image_Url,
            AdopterEmail = a.Adopter.Email,
            AdopterFullName = a.Adopter.FirstName + " " + a.Adopter.LastName,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            StartProcessingAt = a.StartProcessingAt,
            CompletedAt = a.CompletedAt,
            CanceledAt = a.CanceledAt,
            NoteForAdministration = a.NoteForAdministration,
            NoteForAdopter = a.NoteForAdopter,
            State = a.CanceledAt != null ? "Canceled" : a.CompletedAt != null ? "Completed" : a.StartProcessingAt != null ? "Processing" : null,
            StateDate = a.CanceledAt != null ? a.CanceledAt : a.CompletedAt != null ? a.CompletedAt : a.StartProcessingAt != null ? a.StartProcessingAt : null
        };
        return updatedAdoption;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            IEnumerable<Adoption>? fullAdoptionsList;
            List<AdoptionDetailsForAdminDto> adminAdoptionsList;
            fullAdoptionsList = await _adoptionRepo.GetAll();

            if (fullAdoptionsList == null || fullAdoptionsList.Count() < 1)
                return Ok(adminAdoptionsList = new List<AdoptionDetailsForAdminDto>());

            adminAdoptionsList = fullAdoptionsList.Select(a => new AdoptionDetailsForAdminDto
            {
                Id = a.Id,
                AdopteeId = a.Adoptee.Id,
                AdopteeName = a.Adoptee.Name,
                AdopteeType = a.Adoptee.Type,
                AdoppteeMainImageURL = a.Adoptee.Main_Image_Url,
                AdopterEmail = a.Adopter.Email,
                AdopterFullName = a.Adopter.FirstName + " " + a.Adopter.LastName,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                StartProcessingAt = a.StartProcessingAt,
                CompletedAt = a.CompletedAt,
                CanceledAt = a.CanceledAt,
                NoteForAdministration = a.NoteForAdministration,
                NoteForAdopter = a.NoteForAdopter,
                State = a.CanceledAt != null ? "Canceled" : a.CompletedAt != null ? "Completed" : a.StartProcessingAt != null ? "Processing" : null,
                StateDate = a.CanceledAt != null ? a.CanceledAt : a.CompletedAt != null ? a.CompletedAt : a.StartProcessingAt != null ? a.StartProcessingAt : null
            }).ToList();

            return Ok(adminAdoptionsList);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllForCurrentUser()
    {
        try
        {
            IEnumerable<Adoption>? currentUserAdoptionsList;
            var user = await _userManager.GetUserAsync(User);
            currentUserAdoptionsList = await _adoptionRepo.GetAllByUser(user);
            List<AdoptionDetailsForAdopterDto> adopterAdoptionsList;

            if (currentUserAdoptionsList == null || currentUserAdoptionsList.Count() < 1)
                return Ok(adopterAdoptionsList = new List<AdoptionDetailsForAdopterDto>());

            adopterAdoptionsList = currentUserAdoptionsList.Select(a => new AdoptionDetailsForAdopterDto
            {
                Id = a.Id,
                AdopteeId = a.Adoptee.Id,
                AdopteeName = a.Adoptee.Name,
                AdopteeType = a.Adoptee.Type,
                AdoppteeMainImageURL = a.Adoptee.Main_Image_Url,
                AdopterEmail = a.Adopter.Email,
                AdopterFullName = a.Adopter.FirstName + " " + a.Adopter.LastName,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                StartProcessingAt = a.StartProcessingAt,
                CompletedAt = a.CompletedAt,
                CanceledAt = a.CanceledAt,
                NoteForAdopter = a.NoteForAdopter,
                State = a.CanceledAt != null ? "Canceled" : a.CompletedAt != null ? "Completed" : a.StartProcessingAt != null ? "Processing" : null,
                StateDate = a.CanceledAt != null ? a.CanceledAt : a.CompletedAt != null ? a.CompletedAt : a.StartProcessingAt != null ? a.StartProcessingAt : null
            }).ToList();

            return Ok(adopterAdoptionsList);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AdoptionDetailsForAdminDto>> Get(int id)
    {
        try
        {
            Adoption? adoption = await _adoptionRepo.Get(id);

            if (adoption == null)
                throw new Exception("No adoption found");
            if (!User.IsInRole("Admin"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (adoption.Adopter.Id != user.Id)
                    throw new Exception("You are not authorized to view this adoption");
            }

            AdoptionDetailsForAdminDto adoptionDetails = new()
            {
                Id = adoption.Id,
                AdopteeId = adoption.Adoptee.Id,
                AdopteeName = adoption.Adoptee.Name,
                AdopteeType = adoption.Adoptee.Type,
                AdoppteeMainImageURL = adoption.Adoptee.Main_Image_Url,
                AdopterEmail = adoption.Adopter.Email,
                AdopterFullName = adoption.Adopter.FirstName + " " + adoption.Adopter.LastName,
                CreatedAt = adoption.CreatedAt,
                UpdatedAt = adoption.UpdatedAt,
                StartProcessingAt = adoption.StartProcessingAt,
                CompletedAt = adoption.CompletedAt,
                CanceledAt = adoption.CanceledAt,
                NoteForAdministration = adoption.NoteForAdministration,
                NoteForAdopter = adoption.NoteForAdopter,
                State = adoption.CanceledAt != null ? "Canceled" : adoption.CompletedAt != null ? "Completed" : adoption.StartProcessingAt != null ? "Processing" : null,
                StateDate = adoption.CanceledAt != null ? adoption.CanceledAt : adoption.CompletedAt != null ? adoption.CompletedAt : adoption.StartProcessingAt != null ? adoption.StartProcessingAt : null
            };

            return adoptionDetails;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<AdoptionDetailsForAdminDto>> Delete(int id)
    {
        try
        {
            Adoption? adoption = await _adoptionRepo.Get(id);


            if (adoption == null)
                throw new Exception("No adoption found");

            adoption = await _adoptionRepo.Delete(id);

            AdoptionDetailsForAdminDto deletedAdoptionDetails = new()
            {
                Id = adoption.Id,
                AdopteeId = adoption.Adoptee.Id,
                AdopteeName = adoption.Adoptee.Name,
                AdopteeType = adoption.Adoptee.Type,
                AdoppteeMainImageURL = adoption.Adoptee.Main_Image_Url,
                AdopterEmail = adoption.Adopter.Email,
                AdopterFullName = adoption.Adopter.FirstName + " " + adoption.Adopter.LastName,
                CreatedAt = adoption.CreatedAt,
                UpdatedAt = adoption.UpdatedAt,
                StartProcessingAt = adoption.StartProcessingAt,
                CompletedAt = adoption.CompletedAt,
                CanceledAt = adoption.CanceledAt,
                NoteForAdministration = adoption.NoteForAdministration,
                NoteForAdopter = adoption.NoteForAdopter,
                State = adoption.CanceledAt != null ? "Canceled" : adoption.CompletedAt != null ? "Completed" : adoption.StartProcessingAt != null ? "Processing" : null,
                StateDate = adoption.CanceledAt != null ? adoption.CanceledAt : adoption.CompletedAt != null ? adoption.CompletedAt : adoption.StartProcessingAt != null ? adoption.StartProcessingAt : null
            };
            return deletedAdoptionDetails;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}