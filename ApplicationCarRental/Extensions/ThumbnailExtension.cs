using ApplicationCarRental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationCarRental.Extensions
{
    public static class ThumbnailExtension
    {
        public static IEnumerable<ThumbnailModel> GetCarThumbnail(this List<ThumbnailModel> thumbnails, ApplicationDbContext db = null, string search = null)
        {
            try
            {
                if (db == null)
                {
                    db = ApplicationDbContext.Create();
                }

                thumbnails = (from b in db.Cars
                              select new ThumbnailModel
                              {
                                  CarId = b.Id,
                                  Brand = b.Brand,
                                  Description = b.Description,
                                  ImageUrl = b.ImageUrl,
                                  Link = "/CarDetail/Index/" + b.Id
                              }).ToList();
                if (search != null)
                {
                    return thumbnails.Where(t => t.Brand.ToLower().Contains(search.ToLower())).OrderBy(t => t.Brand);
                }
            }
            catch (Exception ex)
            {

            }

            return thumbnails.OrderBy(b => b.Brand);
        }

    }
}