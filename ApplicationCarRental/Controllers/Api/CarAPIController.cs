using ApplicationCarRental.Models;
using ApplicationCarRental.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApplicationCarRental.Controllers.Api
{
    public class CarAPIController : ApiController
    {
        private ApplicationDbContext db;
        public CarAPIController()
        {
            db = ApplicationDbContext.Create();
        }

        //return brand
        public IHttpActionResult Get(string query = null)
        {
            var carQuery = db.Cars.Where(b => b.Brand.ToLower().Contains(query.ToLower()));

            return Ok(carQuery.ToList());
        }

        //Price or Availability(price/avail)
        public IHttpActionResult Get(string type, string rego = null, string rentalDuration = null, string email = null)
        {
            if (type.Equals("price"))
            {
                Car CarQuery = db.Cars.Where(b => b.Rego.Equals(rego)).SingleOrDefault();
                var chargeRate = from u in db.Users
                                 join m in db.MembershipTypes on u.MembershipTypeId equals m.Id
                                 where u.Email.Equals(email)
                                 select new { m.ChargeRateOneMonth, m.ChargeRateTwoMonth };

                var price = Convert.ToDouble(CarQuery.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateOneMonth) / 100;

                if (rentalDuration == SD.TwoMonthCount)
                {
                    price = Convert.ToDouble(CarQuery.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateTwoMonth) / 100;
                }
                return Ok(price);
            }
            else
            {
                Car CarQuery = db.Cars.Where(b => b.Rego.Equals(rego)).SingleOrDefault();
                return Ok(CarQuery.Availability);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}
