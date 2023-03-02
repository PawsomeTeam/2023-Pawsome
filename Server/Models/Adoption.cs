using System.ComponentModel.DataAnnotations;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Models
{
    public class Adoption
    {
        public int Id { get; set; }

        [Required, Display(Name = "Adoptee")]
        public Animal? Adoptee { get; set; }

        [Display(Name = "Adopter")]
        public User? Adopter { get; set; }

        [Required, Display(Name = "Date Submitted")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Start Processing Date")]
        public DateTime? StartProcessingAt { get; set; } = null;

        [Display(Name = "Date Last Updated")]
        public DateTime? UpdatedAt { get; set; } = null;

        [Display(Name = "Date Completed Adoption")]
        public DateTime? CompletedAt { get; set; } = null;

        [Display(Name = "Date Cancelled Adoption")]
        public DateTime? CanceledAt { get; set; } = null;

        [Display(Name = "Notes For Adopter")]
        List<string>? NotesForAdopter { get; set; } = new List<string>();

        [Display(Name = "Notes For Administration")]
        List<string>? NotesForAdministration { get; set; } = new List<string>();
    }
}
