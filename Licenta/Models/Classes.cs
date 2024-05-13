using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Models
{
    public class Classes
    {
        public int idcl { get; set; }
        public int idco { get; set; } // Foreign key for the coach
        public int idct { get; set; } // Foreign key for the class type
        public string name { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public TimeSpan time { get; set; }

        // Navigation properties
        public Coach Coach { get; set; } // Represents the related Coach entity
        public ClassType ClassType { get; set; } // Represents the related ClassType entity

        // Additional properties
        public string CoachName { get; set; } // Stores the coach's name
        public string ClassTypeType { get; set; } // Stores the class type's type
    }
}
