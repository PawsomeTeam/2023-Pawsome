using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Repositories
{
    public interface IAdoptionRepository
    {
        Task<Adoption?> Get(int id);
        Task<IEnumerable<Adoption>> GetAll();
        Task<Adoption?> Create(AdoptionSubmissionDto AdoptionSubmissionDto);
        Task<Adoption?> Update(AdoptionDetailsForAdminDto AdoptionAdminUpdateDto);
        Task<Adoption?> Delete(int id);
        Task<IEnumerable<Adoption>> GetAllByUser(User user);
    }
}
