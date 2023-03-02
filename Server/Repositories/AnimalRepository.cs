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

        public async Task<Animal> AddAnimal(AnimalDto animalDto)
        {
            var newAnimal = new Animal
            {
                Name = animalDto.Name,
                Description = animalDto.Description,
                Age = animalDto.Age,
                Main_Image_Url = "",
            };
            var result = await _dbContext.Animals.AddAsync(newAnimal);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task UpdateAnimal(AnimalDto animal)
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