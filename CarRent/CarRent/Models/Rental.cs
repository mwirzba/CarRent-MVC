using CarRent.Models.CarModels;
using CarRent.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models
{
    public class Rental
    {
        [Key]
        public long Id { get; set; }

        public Car Car { get; set; }

        [Required]
        public short CarId { get; set; }

        [Required]
        public DateTime? RentDate { get; set; }

        [Required]
        public DateTime? ReturnDate { get; set; }

        public long TotalPrice { get; set; }

        public User User { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public byte RentalStatusId { get; set; }
        public virtual RentalStatus RentalStatus{ get; set; }
    }
}
