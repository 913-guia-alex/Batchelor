using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta.Models
{
    public class Customer
    {
        public int idc { get; set; }
        public string surname { get; set; }
        public string lastname { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public List<GymCard> GymCards { get; set; }
        public List<Favourite> Favourites { get; set; }



    }
}