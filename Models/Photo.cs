using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public Album Album { get; set; }
        public Location Location { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string FilePath { get; set; }
        public ICollection<Interaction> Interactions { get; set; }
        public string WhoAdded { get; set; }
    }
}
