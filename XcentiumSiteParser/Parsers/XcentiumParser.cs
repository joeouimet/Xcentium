using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using XcentiumSiteParser.Interfaces;
using XcentiumSiteParser.Models;

namespace XcentiumSiteParser.Parsers {
    public class XcentiumParser : ISiteParser {
        private Uri Uri { get; set; }
        HtmlDocument Document { get; set; }

        public void Initialize(string url) {
            var getHtmlWeb = new HtmlWeb();
            Document = getHtmlWeb.Load(url);
            Uri = new Uri(url);
        }

        public List<string> GetImages() {
            var urls = Document.DocumentNode.Descendants("img")
                                 .Select(e => FullPathToImage(e.GetAttributeValue("src", null)))
                                 .Where(s => !String.IsNullOrEmpty(s));
            return urls.ToList();
        }

        private string FullPathToImage(string img) {
            if (!ShouldGetFullPathtoImage(img))
                return img;

            string path = Uri.Scheme + "://" + Uri.DnsSafeHost;
            if (!img.StartsWith("/"))
                path += "/";

            path += img;
            return path;
        }

        private bool ShouldGetFullPathtoImage(string img)
        {
            if (img == null || img.StartsWith("//") || img.ToLower().StartsWith("http://") ||
                img.ToLower().StartsWith("https://")) // cool, good to go            
                return false;

            return true;
        }
        public TopWords GetTopWords(int cnt) {
            var tw = new TopWords();

            var allText = GetAllText();
            var allWords = SplitWords(allText);
            tw.WordCount = allWords.Length;

            var wc = allWords.GroupBy(x => x).Select(x => new WordCount(x.Key, x.Count()));
            tw.Words = wc.OrderByDescending(x => x.Count).Take(cnt).ToList();

            return tw;

        }
        static string[] SplitWords(string s) {
            return Regex.Split(s, @"\W+");
        }
        private String GetAllText() {
            var ret = new StringBuilder();

            foreach (var node in Document.DocumentNode.DescendantsAndSelf()) {
                if (!CheckValidNode(node))
                    continue;

                var htmlDecode = System.Net.WebUtility.HtmlDecode(node.InnerText);
                string text = htmlDecode?.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                if (!string.IsNullOrWhiteSpace(text))
                    ret.Append(text + " ");     // make sure we don't run words together
            }
            return ret.ToString();
        }

        private static bool CheckValidNode(HtmlNode node) {
            // bypass scripts/styles/comments
            if (node.Name == "head" || node.Name == "link" || node.Name == "script" || node.Name == "style" ||
                node.Name == "#comment" || (node.ParentNode != null && node.ParentNode.Name == "script"))
                return false;

            if (node.HasChildNodes)
                return false;

            return true;
        }

    }
}