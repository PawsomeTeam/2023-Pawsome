using Microsoft.EntityFrameworkCore;
using PawsomeProject.Server.Data;
using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly PawsomeDBContext _dbContext;

        public AnimalRepository(PawsomeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Animal>> GetAllAnimals()
        {
            return await _dbContext.Animals.ToListAsync();
        }

        public async Task<Animal> GetAnimalById(int id)
        {
            return await _dbContext.Animals.FindAsync(id);
        }

        public async Task AddAnimal(Animal animal)
        {
            _dbContext.Animals.Add(animal);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAnimal(Animal animal)
        {
            _dbContext.Entry(animal).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAnimal(int id)
        {
            var animal = await _dbContext.Animals.FindAsync(id);
            _dbContext.Animals.Remove(animal);
            await _dbContext.SaveChangesAsync();
        }
    }
}