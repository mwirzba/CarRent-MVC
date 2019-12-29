
using CarRent.Data;
using CarRent.Models.CarModels;
using CarRent.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace CarRent.Controllers
{

    public class CarController : Controller
    {
        private readonly AppDbContext _dbContext;
        private const string carImagePath = "/CarRent/CarRent/Data/Images/";

        public CarController(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public IActionResult Index()
        {
            var cars = _dbContext.Cars.ToList();
            return View("List",cars);
        }

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

        public async Task<IActionResult> Delete(int id)
        {
            var carInDb = await _dbContext.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (carInDb == null)
                return RedirectToAction(nameof(Index));
            _dbContext.Cars.Remove(carInDb);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

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
                carInDb.CarNumberOfSeats = carFormViewModel.Car.CarNumberOfSeats;
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
                ModelState.AddModelError("Image", "Image is required");
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

        private async Task<bool> SaveCarImageToDirectory(string fileName)
        {
            IFormFile file = Request.Form.Files.First();
            string pathSrc = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            pathSrc += carImagePath;
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
    }
}