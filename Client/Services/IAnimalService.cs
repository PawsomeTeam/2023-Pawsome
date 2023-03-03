using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services;

public interface IAnimalService
{
    Task<List<AnimalDto>> GetAll();
    Task<AnimalDto> AddAnimal(AnimalDto animal);
    Task DeleteAnimal(int id);
    Task<AnimalDto> GetAnimalById(int id);
    Task<AnimalDto> UpdateAnimal(AnimalDto animalDto);

}
