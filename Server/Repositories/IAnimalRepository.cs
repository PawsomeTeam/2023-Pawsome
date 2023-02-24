using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories
{
	public interface IAnimalRepository
	{
		Task<List<Animal>> GetAllAnimals();
		Task<Animal> GetAnimalById(int id);
		Task AddAnimal(Animal animal);
		Task UpdateAnimal(Animal animal);
		Task DeleteAnimal(int id);
	}
}
