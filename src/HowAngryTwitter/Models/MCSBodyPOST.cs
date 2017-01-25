using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowAngryTwitter.Models
{
    public class Document
    {
        public string language { get; set; }
        public string id { get; set; }
        public string text { get; set; }
    }

    public class MCSBodyPOST
    {
        public List<Document> documents { get; set; }
    }
}
