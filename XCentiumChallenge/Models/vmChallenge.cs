using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using XcentiumSiteParser.Models;

namespace XCentiumChallenge.Models {
    public class vmChallenge
    {
        public vmChallenge()
        {
            ImageUrl = new List<string>();
            Words = new List<WordCount>();
        }

        public string URL { get; set; }
        public List<string> ImageUrl { get; set; }
        public List<WordCount> Words { get; set; }
        public int WordCount { get; set; }

        public string ErrorMessage { get; set; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}