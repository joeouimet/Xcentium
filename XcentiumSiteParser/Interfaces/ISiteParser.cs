using System.Collections.Generic;
using XcentiumSiteParser.Models;

namespace XcentiumSiteParser.Interfaces {
    public interface ISiteParser
    {
        void Initialize(string url);
        List<string> GetImages();
        TopWords GetTopWords(int cnt);
    }
}