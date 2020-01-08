using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CarRent.ViewModels;

namespace CarRent.Infrastructure
{
    public class DateSession
    {
        [Newtonsoft.Json.JsonIgnore] public ISession Session { get; set; }
        public RentalDateViewModel date = 
            new RentalDateViewModel { RentDate = DateTime.Today, ReturnDate = DateTime.Today.AddDays(1) };
        public static DateSession GetDateSession(IServiceProvider service)
        {
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            DateSession dataSession = session?.GetJson<DateSession>("Data") ?? new DateSession();

            dataSession.Session = session;

            return dataSession;
        }

        public void SetDate(RentalDateViewModel Newdate)
        {
            date = Newdate;
            Session.SetJson("Data", this);
        }

    }
}
