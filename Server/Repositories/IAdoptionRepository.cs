using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories
{
    public interface IAdoptionRepository
    {
        Task<Adoption?> Get(int id);
        Task<IEnumerable<Adoption>> GetAll();
        Task<Adoption?> Create(AdoptionSubmissionDto AdoptionSubmissionDto);
        // Task<Adoption?> Update(AdoptionAdminUpdateDto AdoptionAdminUpdateDto);
        Task<Adoption?> Delete(int id);
    }
}
