using CarRent.Models.CarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.ViewModels
{
    public class CarFormViewModel
    {
        public Car Car { get; set; }
        public IEnumerable<CarCategory> CarCategories { get; set; }
        public IEnumerable<CarClass> CarClasses { get; set; }
        public IEnumerable<CarFuelType> CarFuelTypes { get; set; }
    }
}
