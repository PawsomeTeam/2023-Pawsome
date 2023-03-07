using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories
{
    public interface IAnimalRepository
    {
        Task<List<Animal>> GetAllAnimals();
        Task<Animal> GetAnimalById(int id);
        Task<List<Image>> GetImages(int id);
        Task<Animal> AddAnimal(AnimalDto animalDto);
        Task<Animal> UpdateAnimal(int id, AnimalDto animal);
        Task DeleteAnimal(int id);
        Task DeleteImage(int id);
    }
}
