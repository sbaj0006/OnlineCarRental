using ApplicationCarRental.Models;
using ApplicationCarRental.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ApplicationCarRental.Controllers
{
    [Authorize(Roles = SD.AdminUserRole)]
    public class TypeCarController : Controller
    {
        private ApplicationDbContext db;

        public TypeCarController()
        {
            db = new ApplicationDbContext();
        }

        // GET: TypeCar
        public ActionResult Index()
        {
            return View(db.TypeCars.ToList());
        }

        //Get
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypeCar typeCar)
        {
            if (ModelState.IsValid)
            {
                db.TypeCars.Add(typeCar);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TypeCar typeCar = db.TypeCars.Find(id);
            if (typeCar == null)
            {
                return HttpNotFound();
            }
            return View(typeCar);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TypeCar typeCar = db.TypeCars.Find(id);
            if (typeCar == null)
            {
                return HttpNotFound();
            }
            return View(typeCar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TypeCar typeCar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeCar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TypeCar typeCar = db.TypeCars.Find(id);
            if (typeCar == null)
            {
                return HttpNotFound();
            }
            return View(typeCar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            TypeCar typeCar = db.TypeCars.Find(id);
            db.TypeCars.Remove(typeCar);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
        }
    }
}