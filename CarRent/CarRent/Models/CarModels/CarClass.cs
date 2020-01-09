using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.CarModels
{
    public class CarClass
    {
        [Key]
        public byte Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
