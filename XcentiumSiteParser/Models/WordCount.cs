using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XcentiumSiteParser.Models {
    public class WordCount {
        public WordCount(string w, int c) {
            Word = w;
            Count = c;
        }
        public string Word { get; set; }
        public int Count { get; set; }
    }
}