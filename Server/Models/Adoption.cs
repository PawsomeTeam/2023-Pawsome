using System.ComponentModel.DataAnnotations;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Models
{
    public class Adoption
    {
        public int Id { get; set; }

        //relationships
        [Display(Name = "Adoptee")]// make required later
        public Animal Adoptee { get; set; } = default!;

        public User Adopter { get; set; } = default!;

        //dates data
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


        //notes
        [Display(Name = "Note For Adopter")]
        public string? NoteForAdopter { get; set; } = null;

        [Display(Name = "Note For Administration")]
        public string? NoteForAdministration { get; set; } = null;
    }
}
