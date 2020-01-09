

using System.ComponentModel.DataAnnotations;

namespace CarRent.Models.CarModels
{
    public class CarCategory
    {
        [Key]
        public byte Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
