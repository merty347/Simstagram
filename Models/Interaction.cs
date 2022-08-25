using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Models
{
    public class Interaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PhotoId { get; set; }
        public bool Heart { get; set; }
        public string Comment { get; set; }
        public User User { get; set; }
        public Photo Photo { get; set; }
    }
}
