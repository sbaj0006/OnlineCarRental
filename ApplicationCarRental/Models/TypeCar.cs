using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApplicationCarRental.Models
{
    public class TypeCar
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("Type of Car")]
        public string Name { get; set; }
    }
}