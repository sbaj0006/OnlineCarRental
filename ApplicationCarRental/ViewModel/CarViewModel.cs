using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationCarRental.Models;

namespace ApplicationCarRental.ViewModel
{
    public class CarViewModel
    {
        public IEnumerable<TypeCar> Types { get; set; }
        public Car Car { get; set; }
    }
}