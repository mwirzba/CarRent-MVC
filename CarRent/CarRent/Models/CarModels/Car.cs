using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.Models.CarModels
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short Id { get; set; }

        [Required]
        [Display(Name = "Brand")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string Brand { get; set; }

        [Required]

        [Display(Name = "Model")]
        [StringLength(20,ErrorMessage ="Model has to have at least 2 letters",MinimumLength = 2)]
        public string Model { get; set; }


        [Required]
        [Range(1, 20000)]
        [Display(Name = "Rent price for one day")]
        public short RentPrice { get; set; }

        [Display(Name = "Category")]
        public byte CarCategoryId { get; set; }
        public virtual CarCategory CarCategory { get; set; }

        [Display(Name = "Class")]
        public byte CarClassId { get; set; }
        public virtual CarClass CarClass { get; set; }

        [Display(Name = "Fuel Type")]
        public byte CarFuelTypeId { get; set; }
        public virtual CarFuelType CarFuelType { get; set; }


        [Range(1, 20)]
        [Display(Name = "Number of Seats")]
        public virtual int CarNumberOfSeats { get; set; }
        //public string CarImage { get; set; }

    }
}
