using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ApplicationCarRental.Models;
using Microsoft.AspNet.Identity;
using ApplicationCarRental.Utility;
using ApplicationCarRental.ViewModel;

namespace ApplicationCarRental.Controllers
{
    public class CarDetailController : Controller
    {

        private ApplicationDbContext db;
        public CarDetailController()
        {
            db = ApplicationDbContext.Create();
        }

        // GET: CarDetail
        public ActionResult Index(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(u => u.Id == userId);

            var carModel = db.Cars.Include(b => b.Type).SingleOrDefault(b => b.Id == id);

            var rentalPrice = 0.0;
            var oneMonthRental = 0.0;
            var TwoMonthRental = 0.0;

            if (user != null && !User.IsInRole(SD.AdminUserRole))
            {
                var chargeRate = from u in db.Users
                                 join m in db.MembershipTypes on u.MembershipTypeId equals m.Id
                                 where u.Id.Equals(userId)
                                 select new { m.ChargeRateOneMonth, m.ChargeRateTwoMonth };

                oneMonthRental = Convert.ToDouble(carModel.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateOneMonth) / 100;
                TwoMonthRental = Convert.ToDouble(carModel.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateTwoMonth) / 100;
            }
            CarRentalViewModel model = new CarRentalViewModel
            {
                CarId = carModel.Id,
                Rego = carModel.Rego,
                Availability = carModel.Availability,
                DateAdded = carModel.DateAdded,
                Description = carModel.Description,
                Type = db.TypeCars.FirstOrDefault(g => g.Id.Equals(carModel.TypeId)),
                TypeId = carModel.TypeId,
                ImageUrl = carModel.ImageUrl,
                Seats = carModel.Seats,
                Price = carModel.Price,
                FuelType = carModel.FuelType,
                Engine = carModel.Engine,
                Brand = carModel.Brand,
                Manufacturer = carModel.Manufacturer,
                UserId = userId,
                RentalPrice = rentalPrice,
                rentalPriceTwoMonth = TwoMonthRental,
                rentalPriceOneMonth = oneMonthRental

            };

            return View(model);
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