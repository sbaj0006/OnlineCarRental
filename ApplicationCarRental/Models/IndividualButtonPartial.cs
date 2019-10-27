using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ApplicationCarRental.Models
{
    public class IndividualButtonPartial
    {
        public string ButtonType { get; set; }
        public string Action { get; set; }
        public string Glyph { get; set; }
        public string Text { get; set; }

        public int? TypeId { get; set; }
        public int? CarId { get; set; }
        public int? CustomerId { get; set; }
        public int? MembershiTypeId { get; set; }
        public string UserId { get; set; }
        public int? CarRentalId { get; set; }

        public string ActionParameter
        {
            get
            {
                var param = new StringBuilder(@"/");
                if (CarId != null && CarId > 0)
                {
                    param.Append(String.Format("{0}", CarId));
                }
                if (TypeId != null && TypeId > 0)
                {
                    param.Append(String.Format("{0}", TypeId));
                }
                if (MembershiTypeId != null && MembershiTypeId > 0)
                {
                    param.Append(String.Format("{0}", MembershiTypeId));
                }
                if (CustomerId != null && CustomerId > 0)
                {
                    param.Append(String.Format("{0}", CustomerId));
                }
                if (UserId != null && UserId.Trim().Length > 0)
                {
                    param.Append(String.Format("{0}", UserId));
                }
                if (CarRentalId != null && CarRentalId > 0)
                {
                    param.Append(String.Format("{0}", CarRentalId));
                }
                return param.ToString();
            }
        }
    }
}