using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowAngryTwitter.Models
{
    public class SentimentDocument
    {
        public double score { get; set; }
        public string id { get; set; }
    }

    public class MCSSentimentResponse
    {
        public List<SentimentDocument> documents { get; set; }
        public List<object> errors { get; set; }
    }
}
