
using System.ComponentModel.DataAnnotations;

namespace PawsomeProject.Shared.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
       
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int Age { get; set; }
        
        public string? Main_Image_Url { get; set; }

        public DateTime? Reservation_Date{ get; set; }
        
        public DateTime? date_adopted { get; set; }
    }
}
