using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta.Models
{
    public class CaloriesCalculator
    {
        public int Age { get; set; }
        public string Gender { get; set; } 
        public double Height { get; set; } 
        public double Weight { get; set; } 
        public string Goal { get; set; } 
        public double Calories { get; set; }
    }
}