
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
                new Car { Id = 1, NumberOfSeats = 5 , Brand = "Kia", Model = "Venga", CarCategoryId = 1, CarClassId = 1, CarFuelTypeId = 1, RentPrice =  120 ,ImageName="venga.png"},
                new Car { Id = 2, NumberOfSeats = 5 , Brand = "Kia", Model = "Sorento", CarCategoryId = 1, CarClassId = 2, CarFuelTypeId = 2, RentPrice =  140 , ImageName = "sorento.png" },
                new Car { Id = 3, NumberOfSeats = 6 , Brand = "Seat ", Model = "Ibiza", CarCategoryId = 3, CarClassId = 4, CarFuelTypeId = 4, RentPrice = 84, ImageName = "ibiza.png" },
                new Car { Id = 4, NumberOfSeats = 5 , Brand = "Suzuki", Model = "Celerio", CarCategoryId = 1, CarClassId = 1, CarFuelTypeId = 4, RentPrice = 150, ImageName = "Celerio.png" },
                new Car { Id = 5, NumberOfSeats = 4 , Brand = "Toyota ", Model = "Yaris", CarCategoryId = 1, CarClassId = 1, CarFuelTypeId = 3, RentPrice = 170, ImageName = "Yaris.png" },
                new Car { Id = 6, NumberOfSeats = 6 , Brand = "Opel ", Model = "Zafira", CarCategoryId = 3, CarClassId = 1, CarFuelTypeId = 2, RentPrice = 90, ImageName = "Zafira.png" },
                new Car { Id = 7, NumberOfSeats = 7 , Brand = "Opel ", Model = "Insignia", CarCategoryId = 4, CarClassId = 1, CarFuelTypeId = 4, RentPrice = 180, ImageName = "Insignia.png" },
                new Car { Id = 8, NumberOfSeats = 7 , Brand = "Suzuki ", Model = "SX4 S-Cross", CarCategoryId = 5, CarClassId = 6, CarFuelTypeId = 4, RentPrice = 170, ImageName = "Cross.png" }
             );
        }

    } 
}
