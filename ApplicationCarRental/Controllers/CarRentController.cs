using ApplicationCarRental.Models;
using ApplicationCarRental.Utility;
using ApplicationCarRental.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;

namespace ApplicationCarRental.Controllers
{
    [Authorize]
    public class CarRentController : Controller
    {
        private ApplicationDbContext db;

        public CarRentController()
        {
            db = ApplicationDbContext.Create();
        }

        //Get Method
        public ActionResult Create(string brand = null, string rego = null)
        {
            if (brand != null && rego != null)
            {
                CarRentalViewModel model = new CarRentalViewModel
                {
                    Brand = brand,
                    Rego = rego
                };
                return View(model);
            }
            return View(new CarRentalViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarRentalViewModel carRent)
        {
            if (ModelState.IsValid)
            {
                var email = carRent.Email;

                var userDetails = from u in db.Users
                                  where u.Email.Equals(email)
                                  select new { u.Id, u.FirstName, u.LastName, u.BirthDate };

                var Rego = carRent.Rego;

                Car carSelected = db.Cars.Where(b => b.Rego == Rego).FirstOrDefault();

                var rentalDuration = carRent.RentalDuration;

                var chargeRate = from u in db.Users
                                 join m in db.MembershipTypes
                                 on u.MembershipTypeId equals m.Id
                                 where u.Email.Equals(email)
                                 select new { m.ChargeRateOneMonth, m.ChargeRateTwoMonth };

                var oneMonthRental = Convert.ToDouble(carSelected.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateOneMonth) / 100;
                var TwoMonthRental = Convert.ToDouble(carSelected.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateTwoMonth) / 100;

                double rentalPr = 0;

                if (carRent.RentalDuration == SD.TwoMonthCount)
                {
                    rentalPr = TwoMonthRental;
                }
                else
                {
                    rentalPr = oneMonthRental;
                }

                //BookRentalViewModel model = new BookRentalViewModel
                //{
                //    BookId = bookSelected.Id,
                //    RentalPrice = rentalPr,
                //    Price = bookSelected.Price,
                //    Pages = bookSelected.Pages,
                //    FirstName = userDetails.ToList()[0].FirstName,
                //    LastName = userDetails.ToList()[0].LastName,
                //    BirthDate = userDetails.ToList()[0].BirthDate,
                //    ScheduledEndDate = bookRent.ScheduledEndDate,
                //    Author = bookSelected.Author,
                //    Avaibility = bookSelected.Avaibility,
                //    DateAdded = bookSelected.DateAdded,
                //    Description = bookSelected.Description,
                //    Email = email,
                //    GenreId = bookRent.GenreId,
                //    Genre = db.Genres.Where(g => g.Id.Equals(bookSelected.GenreId)).First(),
                //    ISBN = bookSelected.ISBN,
                //    ImageUrl = bookSelected.ImageUrl,
                //    ProductDimensions = bookSelected.ProductDimensions,
                //    PublicationDate = bookSelected.PublicationDate,
                //    Publisher = bookSelected.Publisher,
                //    RentalDuration = bookRent.RentalDuration,
                //    Status = BookRent.StatusEnum.Requested.ToString(),
                //    Title = bookSelected.Title,
                //    UserId = userDetails.ToList()[0].Id,
                //    RentalPriceOneMonth = oneMonthRental,
                //    RentalPriceSixMonth = sixMonthRental
                //};

                CarRent modelToAddToDb = new CarRent
                {
                    CarId = carSelected.Id,
                    RentalPrice = rentalPr,
                    ScheduledEndDate = carRent.ScheduledEndDate,
                    RentalDuration = carRent.RentalDuration,
                    Status = CarRent.StatusEnum.Approved,
                    UserId = userDetails.ToList()[0].Id
                };

                carSelected.Availability -= 1;
                db.CarRental.Add(modelToAddToDb);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }



        // GET: CarRent
        public ActionResult Index(int? pageNumber, string option = null, string search = null)
        {
            string userid = User.Identity.GetUserId();

            var model = from br in db.CarRental
                        join b in db.Cars on br.CarId equals b.Id
                        join u in db.Users on br.UserId equals u.Id
                        select new CarRentalViewModel
                        {
                            Id = br.Id,
                            CarId = b.Id,
                            RentalPrice = br.RentalPrice,
                            Price = b.Price,
                            Seats = b.Seats,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            BirthDate = u.BirthDate,
                            ScheduledEndDate = br.ScheduledEndDate,
                            Manufacturer = b.Manufacturer,
                            Availability = b.Availability,
                            DateAdded = b.DateAdded,
                            Description = b.Description,
                            Email = u.Email,
                            TypeId = b.TypeId,
                            Type = db.TypeCars.Where(g => g.Id.Equals(b.TypeId)).FirstOrDefault(),
                            Rego = b.Rego,
                            ImageUrl = b.ImageUrl,
                            Engine = b.Engine,
                            FuelType = b.FuelType,
                            RentalDuration = br.RentalDuration,
                            Status = br.Status.ToString(),
                            Brand = b.Brand,
                            UserId = u.Id,
                            StartDate = br.StartDate
                        };

            if (option == "email" && search.Length > 0)
            {
                model = model.Where(u => u.Email.Contains(search));
            }
            if (option == "name" && search.Length > 0)
            {
                model = model.Where(u => u.FirstName.Contains(search) || u.LastName.Contains(search));
            }
            if (option == "status" && search.Length > 0)
            {
                model = model.Where(u => u.Status.Contains(search));
            }

            if (!User.IsInRole(SD.AdminUserRole))
            {
                model = model.Where(u => u.UserId.Equals(userid));
            }

            return View(model.ToList().ToPagedList(pageNumber ?? 1, 5));
        }

        [HttpPost]
        public ActionResult Reserve(CarRentalViewModel car)
        {
            var userid = User.Identity.GetUserId();
            Car carToRent = db.Cars.Find(car.CarId);
            double rentalPr = 0;

            if (userid != null)
            {
                var chargeRate = from u in db.Users
                                 join m in db.MembershipTypes
                                 on u.MembershipTypeId equals m.Id
                                 where u.Id.Equals(userid)
                                 select new { m.ChargeRateOneMonth, m.ChargeRateTwoMonth };
                if (car.RentalDuration == SD.TwoMonthCount)
                {
                    rentalPr = Convert.ToDouble(carToRent.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateTwoMonth) / 100;
                }
                else
                {
                    rentalPr = Convert.ToDouble(carToRent.Price) * Convert.ToDouble(chargeRate.ToList()[0].ChargeRateOneMonth) / 100;
                }

                CarRent carRent = new CarRent
                {
                    CarId = carToRent.Id,
                    UserId = userid,
                    RentalDuration = car.RentalDuration,
                    RentalPrice = rentalPr,
                   
                   Status = CarRent.StatusEnum.Requested,
                };

                db.CarRental.Add(carRent);
                var carInDb = db.Cars.SingleOrDefault(c => c.Id == car.CarId);

                carInDb.Availability -= 1;

                db.SaveChanges();
                return RedirectToAction("Index", "CarRent");
            }
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(id);
            var model = getVMFromCarRent(carRent);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);

        }

        //Decline Get
        public ActionResult Decline(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(id);
            var model = getVMFromCarRent(carRent);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Decline(CarRentalViewModel model)
        {
            if (model.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(model.Id);
            carRent.Status = CarRent.StatusEnum.Rejected;

            Car carInDb = db.Cars.Find(carRent.CarId);
            carInDb.Availability += 1;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Approve Get
        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(id);
            var model = getVMFromCarRent(carRent);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View("Approve", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Approve(CarRentalViewModel model)
        {
            if (model.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(model.Id);
            carRent.Status = CarRent.StatusEnum.Approved;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Pickup Get
        public ActionResult PickUp(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(id);
            var model = getVMFromCarRent(carRent);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View("Approve", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PickUp(CarRentalViewModel model)
        {
            if (model.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(model.Id);
            carRent.Status = CarRent.StatusEnum.Rented;
            carRent.StartDate = DateTime.Now;
            if (carRent.RentalDuration == SD.TwoMonthCount)
            {
                carRent.ScheduledEndDate = DateTime.Now.AddMonths(Convert.ToInt32(SD.TwoMonthCount));
            }
            else
            {
                carRent.ScheduledEndDate = DateTime.Now.AddMonths(Convert.ToInt32(SD.OneMonthCount));
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Return Get
        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(id);
            var model = getVMFromCarRent(carRent);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View("Approve", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return(CarRentalViewModel model)
        {
            if (model.Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(model.Id);
            carRent.Status = CarRent.StatusEnum.Closed;
            carRent.AdditionalCharge = model.AdditionalCharge;

            Car carInDb = db.Cars.Find(carRent.CarId);
            carInDb.Availability += 1;

            carRent.ActualEndDate = DateTime.Now;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Delete Get
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(id);
            var model = getVMFromCarRent(carRent);
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);

        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            if (Id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRent carRent = db.CarRental.Find(Id);
            var carInDb = db.Cars.Where(b => b.Id.Equals(carRent.CarId)).FirstOrDefault();
            if (!carRent.Status.ToString().Equals("Rented"))
            {
                carInDb.Availability += 1;
            }
            db.CarRental.Remove(carRent);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        private CarRentalViewModel getVMFromCarRent(CarRent carRent)
        {
            Car carSelected = db.Cars.Where(b => b.Id == carRent.CarId).FirstOrDefault();
            var userDetails = from u in db.Users
                              where u.Id.Equals(carRent.UserId)
                              select new { u.Id, u.FirstName, u.LastName, u.Email };

            CarRentalViewModel model = new CarRentalViewModel
            {
                Id = carRent.Id,
                CarId = carSelected.Id,
                RentalPrice = carRent.RentalPrice,
                Price = carSelected.Price,
                Seats = carSelected.Seats,
                FirstName = userDetails.ToList()[0].FirstName,
                LastName = userDetails.ToList()[0].LastName,
                //BirthDate = userDetails.ToList()[0].bir
                Email = userDetails.ToList()[0].Email,
                UserId = userDetails.ToList()[0].Id,
                ScheduledEndDate = carRent.ScheduledEndDate,
                Manufacturer = carSelected.Manufacturer,
                StartDate = carRent.StartDate,
                Availability = carSelected.Availability,
                DateAdded = carSelected.DateAdded,
                Description = carSelected.Description,
                TypeId = carSelected.TypeId,
                Type = db.TypeCars.FirstOrDefault(g => g.Id.Equals(carSelected.TypeId)),
                Rego = carSelected.Rego,
                ImageUrl = carSelected.ImageUrl,
                FuelType = carSelected.FuelType,
                Engine = carSelected.Engine,
                RentalDuration = carRent.RentalDuration,
                Status = carRent.Status.ToString(),
                Brand = carSelected.Brand,
                AdditionalCharge = carRent.AdditionalCharge,

            };
            return model;
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