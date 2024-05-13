using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Licenta.Models
{
    public class Gym
    {
        public int idg { get; set; }
        public string name { get; set; }
        public string adress { get; set; }
        public string openHour { get; set; }
        public string closeHour { get; set; }
        public float rating { get; set; }

        [DataType(DataType.Password)]
        public string password { get; set; }

        public List<GymCard> GymCards { get; set; }
        public List<Favourite> Favourites { get; set; }



    }
}