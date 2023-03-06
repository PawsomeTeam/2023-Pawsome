using System.ComponentModel.DataAnnotations;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Shared.Models
{
    public class AdoptionDetailsForAdminDto
    {
        public int Id { get; set; }

        //Adoptee
        [Required, Display(Name = "AdopteeId")]
        public int AdopteeId { get; set; } = default!;

        [Required, Display(Name = "Adoptee Name")]
        public string AdopteeName { get; set; } = default!;

        [Required, Display(Name = "Adoptee Name")]
        public string AdopteeType { get; set; } = default!;

        [Required, Display(Name = "Adoptee Image")]
        public string? AdoppteeMainImageURL { get; set; } = default!;//TODO: set a default image

        //Adopter
        [Display(Name = "Adopter email")]
        public string AdopterEmail { get; set; } = default!;

        // [Display(Name = "Adopter First Name")]
        // public string? AdopterFisrtName { get; set; } = default!;

        // [Display(Name = "Adopter Last Name")]
        // public string? AdopterLastName { get; set; } = default!;

        [Display(Name = "Adopter Full Name")]
        public string? AdopterFullName { get; set; } = default!;

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

        //state
        [Display(Name = "Adoption State")]
        public string? State { get; set; } = default!;

        [Display(Name = "Adoption State")]
        public DateTime? StateDate { get; set; } = default!;

        //notes
        [Display(Name = "Note For Adopter")]
        public string? NoteForAdopter { get; set; } = null;

        [Display(Name = "Note For Administration")]
        public string? NoteForAdministration { get; set; } = null;
    }
}
