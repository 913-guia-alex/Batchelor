using MySqlX.XDevAPI;
using System;

namespace Licenta.Models
{
    public class Reservation
    {
        public int idr { get; set; }
        public int idc { get; set; } 
        public int idcl { get; set; } 
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public Customer Customer { get; set; } 
        public Classes Class { get; set; } 

        public string CustomerEmail { get; set; }
        public string ClassName { get; set; } 
        public DateTime ClassDate { get; set; }
        public TimeSpan ClassTime { get; set; }
    }
}
