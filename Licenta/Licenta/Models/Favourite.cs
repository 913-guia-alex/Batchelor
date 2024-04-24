using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Models
{
    public class Favourite
    {
        public int idf { get; set; }
        public int idc { get; set; }
        public int idg { get; set; }
        public Gym Gym { get; set; }

    }
}
