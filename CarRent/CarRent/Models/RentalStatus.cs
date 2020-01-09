using System.ComponentModel.DataAnnotations;

namespace CarRent.Models
{
    public class RentalStatus
    {
        [Key]
        public byte Id { get; set; }

        [Required]
        public string Status { get; set; }
    }
}