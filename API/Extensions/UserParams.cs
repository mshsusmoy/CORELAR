using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;

namespace API.Extensions
{
    public class UserParams : PaginationParams
    {
        public string CurrentUserName { get; set; }
        public string Gender{get;set;}
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;
        public string OrderBy { get; set; } = "lastActive";
    }
}