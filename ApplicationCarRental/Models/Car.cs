using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApplicationCarRental.Models
{
    public class Car
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Rego { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Manufacturer { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [Required]
        [Range(0,10)]
        public int Availability { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [Required]
       // [DisplayFormat(DataFormatString = "{0: MMM dd yyyy}")]
        public DateTime? DateAdded { get; set; }

        [Required]
        public int TypeId { get; set; }

        public TypeCar Type { get; set; }

        [Required]
        public string FuelType { get; set; }

        [Required]
        public int Seats { get; set; }

        [Required]
        public string Engine { get; set; }
    }
}