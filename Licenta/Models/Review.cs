using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta.Models
{
    public class Review
    {
        public int idrev { get; set; }
        public int idc { get; set; } // Customer ID (foreign key)
        public int idg { get; set; } // Gym ID (foreign key)
        public string description { get; set; }
        public float rating { get; set; }
        public string customerEmail { get; set; }
        public Gym Gym { get; set; }
        public Customer Customer { get; set; }

    }


}