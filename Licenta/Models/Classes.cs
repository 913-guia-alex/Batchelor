using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Licenta.Models
{
    public class Classes
    {
        public int idcl { get; set; }
        public int idco { get; set; } 
        public int idct { get; set; } 
        public string name { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public TimeSpan time { get; set; }

        public Coach Coach { get; set; } 
        public ClassType ClassType { get; set; } 

        public string CoachName { get; set; } 
        public string ClassTypeType { get; set; } 
    }
}
