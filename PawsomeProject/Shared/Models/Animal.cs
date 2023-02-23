
using System.ComponentModel.DataAnnotations;

namespace PawsomeProject.Shared.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public int Age { get; set; }
        
        [Required]
        public string? Main_Image_Url { get; set; }

        public DateTime? Reservation_Date{ get; set; }
        
        public DateTime? date_adopted { get; set; }

        
        
    }
}
