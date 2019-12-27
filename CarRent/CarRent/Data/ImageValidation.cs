using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace CarRent.Data
{
    public class ImageValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null)
            {
                return new ValidationResult("Image is required");
            }
            try
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (string.Equals(fileExtension, ".jpg", StringComparison.OrdinalIgnoreCase))
                    return ValidationResult.Success;
                else if (string.Equals(fileExtension, ".png", StringComparison.OrdinalIgnoreCase))
                    return ValidationResult.Success;
            }
            catch { }
            return new ValidationResult("Wrong type of file.File must be in jpg or png format.");
        }
    }
}
