using CarRent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Controllers
{
    public class RentalController
    {
        private AppDbContext _context;

        public RentalController(AppDbContext context)
        {
            _context = context;
        }

        //public Task<IAsyncResult> GetRentalDate(short id)
        //{
        //
        //}
    }
}
