using CarRent.Models.CarModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models
{
    public class Rental
    {
        public long Id { get; set; }

        public Car Car { get; set; }

        public short CarId { get; set; }

        public DateTime? RentDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public long TotalPrice { get; set; }

        public string UserId { get; set; }

    }
}
