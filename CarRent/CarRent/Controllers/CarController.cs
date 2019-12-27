
using CarRent.Data;
using CarRent.Models.CarModels;
using CarRent.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Controllers
{

    public class CarController : Controller
    {
        private readonly AppDbContext _dbContext;
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
            if (!ModelState.IsValid)
                return RedirectToAction("AddCar", carFormViewModel);
            await _dbContext.Cars.AddAsync(carFormViewModel.Car);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}