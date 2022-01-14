using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Status
    {
        public int Id { get; set; }
        public string StatusOwnerName { get; set; }
        public AppUser StatusOwner { get; set; }
        public string Content { get; set; }
        public DateTime StatusPostTime { get; set; } = DateTime.UtcNow;
        public bool StatusDeleted { get; set; }
    }
} 