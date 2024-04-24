using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta.Models
{
    public class CaloriesCalculator
    {
        public int Age { get; set; }
        public string Gender { get; set; } // You can use enum for gender (e.g., Male, Female)
        public double Height { get; set; } // Height in centimeters
        public double Weight { get; set; } // Weight in kilograms
        public string Goal { get; set; } // Variants: "Lose Weight", "Maintain Weight", "Gain Weight"
        public double Calories { get; set; }
    }
}