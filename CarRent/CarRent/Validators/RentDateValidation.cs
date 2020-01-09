using System;
using System.ComponentModel.DataAnnotations;
using CarRent.Models;
using CarRent.ViewModels;

namespace CarRent.Validators
{
    public class RentDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var rental = (RentalDateViewModel)validationContext.ObjectInstance;

            if (rental.RentDate == null)
                return new ValidationResult("Pick up date is required");
            if (rental.ReturnDate == null)
                return new ValidationResult("Date returned is required");

            if (DateTime.Compare(rental.RentDate.Value.Date, DateTime.Now.Date) < 0)
                return new ValidationResult("You can not choose rental date that is earlier than today's.");

            if (DateTime.Compare(rental.ReturnDate.Value.Date, DateTime.Now.Date) <= 0)
                return new ValidationResult("You can not choose return car date that is today or tomorrow.");

            if (rental.RentDate >= rental.ReturnDate)
                return new ValidationResult("Date of car pick up can not be earlier than date of car return");

            return ValidationResult.Success;
        }
    }
}
