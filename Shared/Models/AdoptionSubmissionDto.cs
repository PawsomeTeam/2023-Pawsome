using System.ComponentModel.DataAnnotations;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Shared.Models
{
    public class AdoptionSubmissionDto
    {
        public int Id { get; set; }

        [Required, Display(Name = "Adoptee")]
        public int AdopteeId { get; set; } = default!;

        [Display(Name = "Adopter")]
        public string AdopterId { get; set; } = default!;
    }
}
