using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simstagram2._0.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FollowedId { get; set; }

    }
}
