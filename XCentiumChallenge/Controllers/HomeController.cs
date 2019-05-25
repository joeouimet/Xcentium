using System;
using System.Configuration;
using System.Web.Mvc;
using XcentiumSiteParser.Interfaces;
using XcentiumSiteParser.Parsers;
using XCentiumChallenge.Models;

namespace XCentiumChallenge.Controllers {
    public class HomeController : Controller
    {
        private readonly ISiteParser _siteParser;
        public HomeController(ISiteParser parser)
        {
            _siteParser = parser;
        }
        public ActionResult Index() {
            return View();
        }

        [HttpGet]
        public JsonResult GetResults(string siteURL) {
            var vmResult = new vmChallenge { URL = siteURL };
            try {
                _siteParser.Initialize(siteURL);
                vmResult.ImageUrl = _siteParser.GetImages();

                var tw = _siteParser.GetTopWords(Int32.Parse(ConfigurationManager.AppSettings["TopWordCount"]));
                vmResult.Words = tw.Words;
                vmResult.WordCount = tw.WordCount;

            }
            catch (Exception ex) {
                vmResult.ErrorMessage = String.Format("Could not parse website: {0}",ex.Message);
            }
            return Json(vmResult, JsonRequestBehavior.AllowGet);
        }

    }
}