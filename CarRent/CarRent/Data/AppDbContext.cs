
using CarRent.Models;
using CarRent.Models.CarModels;
using CarRent.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarCategory> CarCategories { get; set; }
        public DbSet<CarClass> CarClasses { get; set; }
        public DbSet<CarFuelType> CarFuelTypes { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarCategory>().HasData(
                    new CarCategory { Id = 1, Name = "Small" },
                    new CarCategory { Id = 2, Name = "Family" },
                    new CarCategory { Id = 3, Name = "Premium" },
                    new CarCategory { Id = 4, Name = "Crossover" },
                    new CarCategory { Id = 5, Name = "Van" }
            );

            modelBuilder.Entity<CarClass>().HasData(
                new CarClass { Id = 1, Name = "A" },
                new CarClass { Id = 2, Name = "B" },
                new CarClass { Id = 3, Name = "C" },
                new CarClass { Id = 4, Name = "D" },
                new CarClass { Id = 5, Name = "E" },
                new CarClass { Id = 6, Name = "SUV" },
                new CarClass { Id = 7, Name = "M" },
                new CarClass { Id = 8, Name = "P" },
                new CarClass { Id = 9, Name = "R" }
            );

            modelBuilder.Entity<CarFuelType>().HasData(
                 new CarFuelType{Id=1,Name = "EV"},
                 new CarFuelType{Id=2,Name = "PB"},
                 new CarFuelType{Id=3,Name = "ON"},
                 new CarFuelType{Id=4,Name = "PB/ON"},
                 new CarFuelType{Id=5,Name = "PB/LPG"}
            );

            modelBuilder.Entity<Models.RentalStatus>().HasData(
                new Models.RentalStatus { Id = 1,Status = "Reservation" },
                new Models.RentalStatus { Id = 2, Status = "Checked" },
                new Models.RentalStatus { Id = 3, Status = "Archival" }
            );
            
            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, Brand = "Kia", Model = "Venga", CarCategoryId = 1, CarClassId = 1, CarFuelTypeId = 1, RentPrice = (short) 120.99 },
                new Car { Id = 2, Brand = "Kia", Model = "Sorento", CarCategoryId = 1, CarClassId = 2, CarFuelTypeId = 2, RentPrice = (short) 140 }
             );
        }

    } 
}
