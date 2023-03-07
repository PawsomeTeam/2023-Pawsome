
using System.ComponentModel.DataAnnotations;
using PawsomeProject.Server.Models;

namespace PawsomeProject.Shared.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public List<Image> Images { get; set; }
        
        public int Age { get; set; }
        public int Price { get; set; }
        public string Type { get; set; }

        public string? Main_Image_Url { get; set; }

        public DateTime? Reservation_Date { get; set; }

        public DateTime? Date_adopted { get; set; }
    }
}
