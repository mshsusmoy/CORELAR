using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class StatusDto
    {
        public int Id { get; set; }
        public string StatusOwnerName { get; set; }
        public string StatusOwnerPhotoUrl { get; set; }
        public string Content { get; set; }
        public DateTime StatusPostTime { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public bool StatusDeleted { get; set; }
    }
}