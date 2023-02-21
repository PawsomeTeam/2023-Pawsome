using PawsomeProject.Shared.Models;

namespace PawsomeProject.Client.Services
{
    public interface IAnimalService
    {
        Task<List<Animal>> GetAll();
    }
}
