using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta.Models
{
        public class GymCard
        {
            public int idgc { get; set; }
            public int idc { get; set; } // Customer ID (foreign key)
            public int idg { get; set; } // Gym ID (foreign key)
            public int price { get; set; }
            public DateTime madeDate { get; set; }
            public DateTime expirationDate { get; set; }
            public Gym Gym { get; set; }

    }


}