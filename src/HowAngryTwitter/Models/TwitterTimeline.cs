using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowAngryTwitter.Models
{
    public class TwitterTimeline
    {
        public string created_at { get; set; }
        public object id { get; set; }
        public string id_str { get; set; }
        public string text { get; set; }
        // more useless properties...

        public string language { get; set; }
    }
}
