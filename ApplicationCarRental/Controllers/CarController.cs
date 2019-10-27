using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationCarRental.Models;
using ApplicationCarRental.Utility;
using ApplicationCarRental.ViewModel;

namespace ApplicationCarRental.Controllers
{
    [Authorize(Roles = SD.AdminUserRole)]
    public class CarController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Car
        public ActionResult Index()
        {
            var cars = db.Cars.Include(c => c.Type);


            var list = cars.ToList();
            List<int> repartitions = new List<int>();
            var typeofCars = list.Select(x => x.Type).Distinct();
            
            foreach (var item in typeofCars)
            {
                repartitions.Add(list.Count(x => x.Type == item));
            }
            var rep = repartitions;
            ViewBag.Types = typeofCars;
            ViewBag.REP = repartitions.ToList();


            return View(cars.ToList());
        }

        // GET: Car/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            var model = new CarViewModel
            {
                Car = car,
                Types = db.TypeCars.ToList()
            };
            return View(model);
        }

        // GET: Car/Create
        public ActionResult Create()
        {
            var typeCar = db.TypeCars.ToList();
            var model = new CarViewModel
            {
                Types = typeCar
            };
            return View(model);
        }

        // POST: Car/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarViewModel carVM)
        {
            var car = new Car
            {
                Manufacturer = carVM.Car.Manufacturer,
                Description = carVM.Car.Description,
                Availability = carVM.Car.Availability,
                ImageUrl = carVM.Car.ImageUrl,
                Price = carVM.Car.Price,
                DateAdded = carVM.Car.DateAdded,
                TypeId = carVM.Car.TypeId,
                Type = carVM.Car.Type,
                FuelType = carVM.Car.FuelType,
                Seats = carVM.Car.Seats,
                Engine = carVM.Car.Engine,
                Brand = carVM.Car.Brand,
                Rego = carVM.Car.Rego
            };

            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            carVM.Types = db.TypeCars.ToList();
            return View(carVM);
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }

            var model = new CarViewModel
            {
                Car = car,
                Types = db.TypeCars.ToList()
            };
            return View(model);
        }

        // POST: Car/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(CarViewModel carVM)
        {
            var car = new Car
            {
                Id = carVM.Car.Id,
                Manufacturer = carVM.Car.Manufacturer,
                Description = carVM.Car.Description,
                Availability = carVM.Car.Availability,
                ImageUrl = carVM.Car.ImageUrl,
                Price = carVM.Car.Price,
                DateAdded = carVM.Car.DateAdded,
                TypeId = carVM.Car.TypeId,
                Type = carVM.Car.Type,
                FuelType = carVM.Car.FuelType,
                Seats = carVM.Car.Seats,
                Engine = carVM.Car.Engine,
                Brand = carVM.Car.Brand,
                Rego = carVM.Car.Rego
            };

            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            carVM.Types = db.TypeCars.ToList();
            return View(car);
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            var model = new CarViewModel
            {
                Car = car,
                Types = db.TypeCars.ToList()
            };
            return View(model);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
