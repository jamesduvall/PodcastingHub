using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.ServiceModel.Syndication;

namespace PodcastingHub.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            //var feedUrl = "http://www.pcgamer.com/feed/rss2/?cat=29038";
            var feedUrl = "http://feeds.feedburner.com/filmcast";
            var reader = XmlReader.Create(feedUrl);
            var feed = SyndicationFeed.Load(reader);

            var episodes = from item in feed.Items.Take(1)
                           select new PodcastEpisode()
                           {
                               Title = item.Title.Text,
                               MP3Location = item.Links.Where(l => l.Length > 0).FirstOrDefault().Uri.AbsoluteUri
                           };

            ViewBag.Episodes = episodes.ToList();

            return View();
        }

        public class PodcastEpisode
        {
            public string Title { get; set; }
            public string MP3Location { get; set; }
            public string ImageLocation { get; set; }
        }

    }
}
