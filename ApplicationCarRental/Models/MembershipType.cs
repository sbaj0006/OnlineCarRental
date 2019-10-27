using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApplicationCarRental.Models
{
    public class MembershipType
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public byte SignUpFee { get; set; }

        [Required]
        [DisplayName("Rental Rate per Month")]
        public Byte ChargeRateOneMonth { get; set; }

        [Required]
        public Byte ChargeRateTwoMonth { get; set; }
    }
}