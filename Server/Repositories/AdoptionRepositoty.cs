using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories
{
    public class AdoptionRepository : IAdoptionRepository
    {
        private readonly PawsomeDBContext _dbContext;
        private readonly UserManager<User> _userManager;

        public AdoptionRepository(PawsomeDBContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Adoption>> GetAll()
        {
            return await _dbContext.Adoptions.ToListAsync();
        }

        public async Task<Adoption?> Get(int id)
        {
            Adoption? adoption = await _dbContext.Adoptions.FindAsync(id);
            if (adoption == null || adoption.Id != id)
                throw new Exception("Adoption not found");
            return adoption;
        }

        public async Task<Adoption?> Create(AdoptionSubmissionDto AdoptionSubmissionDto)
        {
            var animal = await _dbContext.Animals.FindAsync(AdoptionSubmissionDto.AdopteeId);
            var user = await _userManager.FindByEmailAsync(AdoptionSubmissionDto.AdopterEmail);
            Adoption adoption = new Adoption
            {
                Adoptee = animal,
                Adopter = user
            };

            _dbContext.Adoptions.Add(adoption);
            await _dbContext.SaveChangesAsync();
            return adoption;
        }

        // public async Task<Adoption?> Update(AdoptionAdminUpdateDto AdoptionAdminUpdateDto)
        // {
        //     Adoption? adoption = await _dbContext.Adoptions.FindAsync(AdoptionAdminUpdateDto.Id);
        //     if (adoption == null || adoption.Id != AdoptionAdminUpdateDto.Id)
        //         throw new Exception("Adoption not found");

        //     adoption.UpdatedAt = DateTime.Now;
        //     adoption.CanceledAt = AdoptionAdminUpdateDto.CanceledAt;
        //     adoption.CompletedAt = AdoptionAdminUpdateDto.CompletedAt;
        //     adoption.NoteForAdopter = AdoptionAdminUpdateDto.NoteForAdopter;
        //     adoption.NoteForAdministration = AdoptionAdminUpdateDto.NoteForAdministration;
        //     adoption.StartProcessingAt = AdoptionAdminUpdateDto.StartProcessingAt;

        //     _dbContext.Adoptions.Update(adoption);
        //     await _dbContext.SaveChangesAsync();
        //     return adoption;
        // }

        public async Task<Adoption?> Delete(int id)
        {
            Adoption? adoption = await _dbContext.Adoptions.FindAsync(id);
            if (adoption == null || adoption.Id != id)
                throw new Exception("Adoption not found");
            _dbContext.Adoptions.Remove(adoption);
            await _dbContext.SaveChangesAsync();
            return adoption;
        }
    }
}