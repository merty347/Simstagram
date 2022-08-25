using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
