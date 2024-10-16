using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Licenta.Models
{
    public class Coach
    {
        public int idco { get; set; }

        public string name { get; set; }

        public int age { get; set; }

        public string gender { get; set; }

        public string trainerType { get; set; }

        public string phoneNumber { get; set; }

        public string email { get; set; }
        public byte[] photo { get; set; } 

        public int idg { get; set; }
        public Gym Gym { get; set; }
    }
}
