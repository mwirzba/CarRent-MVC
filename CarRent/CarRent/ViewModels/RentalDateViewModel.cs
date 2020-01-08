using CarRent.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.ViewModels
{
    [RentDateValidation]
    public class RentalDateViewModel
    {
        public DateTime? RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
