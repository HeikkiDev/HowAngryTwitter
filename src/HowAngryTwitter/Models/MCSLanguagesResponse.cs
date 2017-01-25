using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HowAngryTwitter.Models
{
    public class DetectedLanguage
    {
        public string name { get; set; }
        public string iso6391Name { get; set; }
        public double score { get; set; }
    }

    public class LanguageDocument
    {
        public string id { get; set; }
        public List<DetectedLanguage> detectedLanguages { get; set; }
    }

    public class MCSLanguagesResponse
    {
        public List<LanguageDocument> documents { get; set; }
        public List<object> errors { get; set; }
    }
}
