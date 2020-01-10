using CarRent.Data;
using CarRent.Infrastructure;
using CarRent.Models.CarModels;
using CarRent.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

  
namespace CarRent.Controllers
{

    public class CarController : Controller
    {
        private readonly AppDbContext _dbContext;
        private const string _carImagePath = "/CarRent/CarRent/wwwroot/carImages";
        private static byte _pageSize = 4;
        private readonly DateSession _dateSession;

        public CarController(AppDbContext appDbContext, DateSession dateSession)
        {
            _dbContext = appDbContext;
            _dateSession = dateSession;
        }
        public async Task<IActionResult> Index(RentalDateViewModel rentalDate, int page=1)
        {           
            IEnumerable<Car> cars;
            if (User.IsInRole("Admin"))
            {
                cars = await _dbContext.Cars.ToListAsync();
                return View("List", cars);
            }
            else if (DateIsNull(rentalDate))
             {
                rentalDate = _dateSession.date;
                if (!DateIsNull(rentalDate))
                 {
                     cars = await Search(rentalDate);
                 }
                 else
                 {
                     cars = await _dbContext.Cars
                    .Include(c => c.CarClass)
                    .Include(c => c.CarFuelType)
                    .Include(c => c.CarCategory)
                    .ToListAsync();
                 }
             }
             else
             {
                _dateSession.SetDate(rentalDate);
                 cars = await Search(rentalDate);
             }
            ModelState.Clear();
            TryValidateModel(rentalDate);
            if (!ModelState.IsValid)
            {
                cars = await _dbContext.Cars
                   .Include(c => c.CarClass)
                   .Include(c => c.CarFuelType)
                   .Include(c => c.CarCategory)
                   .ToListAsync();
            }

            cars = cars.OrderBy(c => c.Brand)
                       .Skip((page - 1) * _pageSize)
                       .Take(_pageSize)
                       .ToList();

            ViewBag.FolderPath = Path.GetDirectoryName(
                Path.GetDirectoryName(Directory.GetCurrentDirectory())) + _carImagePath;

            var viewModel = new ListViewModel<Car>
            {
                Items = cars,
                DateSession = _dateSession,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = (byte)page,
                    ItemsPerPage = _pageSize,
                    TotalItems = (byte)_dbContext.Cars.Count()
                }
            };   
            return View("ReadOnlyList", viewModel);
        }

        private static bool DateIsNull(RentalDateViewModel rentalDate)
        {
            if (rentalDate == null)
                return true;
            return rentalDate.RentDate == null || rentalDate.ReturnDate == null;
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> NewAsync()
        {
            var carFormViewModel = new CarFormViewModel
            {
                Car = new Car(),
                CarCategories = await _dbContext.CarCategories.ToListAsync(),
                CarClasses = await _dbContext.CarClasses.ToListAsync(),
                CarFuelTypes = await _dbContext.CarFuelTypes.ToListAsync()
            };
            return View("CarForm",carFormViewModel);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var carInDb = await _dbContext.Cars.FirstOrDefaultAsync(c=> c.Id == id);
            if (carInDb == null)
                return RedirectToAction(nameof(Index));
            var carFormViewModel = new CarFormViewModel
            {
                Car = carInDb,
                CarCategories = await _dbContext.CarCategories.ToListAsync(),
                CarClasses = await _dbContext.CarClasses.ToListAsync(),
                CarFuelTypes = await _dbContext.CarFuelTypes.ToListAsync()
            };
            return View("CarForm", carFormViewModel);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var carInDb = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (carInDb == null)
                return RedirectToAction(nameof(Index));
            _dbContext.Cars.Remove(carInDb);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Save(CarFormViewModel carFormViewModel)
        {
            var uniqueName = "";
            bool saveImageSuccess = true;
            if (!ModelState.IsValid)
            {
                carFormViewModel.CarCategories = await _dbContext.CarCategories.ToListAsync();
                carFormViewModel.CarClasses = await _dbContext.CarClasses.ToListAsync();
                carFormViewModel.CarFuelTypes = await _dbContext.CarFuelTypes.ToListAsync();
                return View("CarForm", carFormViewModel);
            }
            var carInDb = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == carFormViewModel.Car.Id);
            if(carInDb!=null)
            {
                uniqueName = carInDb.ImageName;
                carInDb.CarCategoryId = carFormViewModel.Car.CarCategoryId;
                carInDb.CarClassId = carFormViewModel.Car.CarClassId;
                carInDb.CarFuelTypeId = carFormViewModel.Car.CarFuelTypeId;
                carInDb.NumberOfSeats = carFormViewModel.Car.NumberOfSeats;
                carInDb.Model = carFormViewModel.Car.Model;
                carInDb.Brand = carFormViewModel.Car.Brand;

                if(Request.Form.Files.Any())
                    saveImageSuccess = await SaveCarImageToDirectory(uniqueName);
                if (saveImageSuccess == false)
                    return View("Error");

                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            if (!Request.Form.Files.Any())
            {
                carFormViewModel.CarCategories = await _dbContext.CarCategories.ToListAsync();
                carFormViewModel.CarClasses = await _dbContext.CarClasses.ToListAsync();
                carFormViewModel.CarFuelTypes = await _dbContext.CarFuelTypes.ToListAsync();
                ModelState.AddModelError("Car.Image", "Image is required");
                return View("CarForm", carFormViewModel);
            }
            uniqueName = await GetUniqueFileName();
            carFormViewModel.Car.ImageName = Path.GetFileNameWithoutExtension(uniqueName)
                +Path.GetExtension(carFormViewModel.Car.Image.FileName);
            saveImageSuccess = await SaveCarImageToDirectory(carFormViewModel.Car.ImageName);
            if (saveImageSuccess == false)
                return View("Error");
            await _dbContext.Cars.AddAsync(carFormViewModel.Car);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = Roles.Admin)]
        private async Task<bool> SaveCarImageToDirectory(string fileName)
        {
            IFormFile file = Request.Form.Files.First();
            string pathSrc = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            pathSrc += _carImagePath;
            using (var stream = new FileStream(Path.Combine(pathSrc, fileName), FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return true;
        }

        private async Task<string> GetUniqueFileName()
        {
            string fileName ="";
            await Task.Run(() =>
            {
                fileName = Path.GetRandomFileName();
                string path = Path.Combine("~/Data/Images", fileName);
                while (System.IO.File.Exists(path))
                {
                    fileName = Path.GetRandomFileName();
                    path = Path.Combine("~/Data/Images", fileName);
                }
            });

            return fileName;
        }

        private async Task<IEnumerable<Car>> Search(RentalDateViewModel rental)
        {
            var unavailableCars = _dbContext.Rentals.Where(r=> 
            (r.RentalStatus.Id == (int)RentalStatus.Reservation || 
            r.RentalStatus.Id == (int)RentalStatus.Checked) 
            &&  (rental.RentDate >= r.RentDate && rental.RentDate <= r.ReturnDate ||
            rental.RentDate < r.ReturnDate && rental.ReturnDate >= r.RentDate)).Select(r=>r.Car);

            return await _dbContext.Cars
                .Except(unavailableCars)
               .Include(c => c.CarCategory)
               .Include(p => p.CarClass)
               .Include(p => p.CarFuelType)
               .ToListAsync();
        }
    }
}