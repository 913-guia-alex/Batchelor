using MySqlX.XDevAPI;
using System;

namespace Licenta.Models
{
    public class Reservation
    {
        public int idr { get; set; }
        public int idc { get; set; } // Foreign key for the client
        public int idcl { get; set; } // Foreign key for the gym class
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        // Navigation properties
        public Customer Customer { get; set; } // Represents the related Client entity
        public Classes Class { get; set; } // Represents the related GymClass entity

        public string CustomerEmail { get; set; } // Stores the coach's name
        public string ClassName { get; set; } // Stores the class type's type
    }
}
