using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationCarRental.Models
{
    public class ThumbnailModel
    {
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
    }
}