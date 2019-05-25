using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XcentiumSiteParser.Models {
    public class TopWords {
        public int WordCount { get; set; }
        public List<WordCount> Words { get; set; }
    }
}