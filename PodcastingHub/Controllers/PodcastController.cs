using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using PodcastingHub.Models;

namespace PodcastingHub.Controllers
{
    public class PodcastController : Controller
    {
        private PodcastingContext db = new PodcastingContext();

        //
        // GET: /Podcast/

        public ActionResult Index()
        {
            return View(db.Podcasts.ToList());
        }

        //
        // GET: /Podcast/Details/5

        public ActionResult Details(int id = 0)
        {
            Podcast podcast = db.Podcasts.Find(id);
            if (podcast == null)
            {
                return HttpNotFound();
            }
            return View(podcast);
        }

        //
        // GET: /Podcast/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Podcast/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Podcast podcast)
        {
            if (ModelState.IsValid)
            {
                podcast = PopulatePodcastWithRSS(podcast.RSSURI);

                if (podcast != null)
                {
                    db.Podcasts.Add(podcast);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(podcast);
        }

        //
        // GET: /Podcast/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Podcast podcast = db.Podcasts.Find(id);
            if (podcast == null)
            {
                return HttpNotFound();
            }
            return View(podcast);
        }

        //
        // POST: /Podcast/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Podcast podcast)
        {
            if (ModelState.IsValid)
            {
                db.Entry(podcast).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(podcast);
        }

        //
        // GET: /Podcast/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Podcast podcast = db.Podcasts.Find(id);
            if (podcast == null)
            {
                return HttpNotFound();
            }
            return View(podcast);
        }

        //
        // POST: /Podcast/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Podcast podcast = db.Podcasts.Find(id);
            db.Podcasts.Remove(podcast);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult MyPodcasts ()
        {
            UserProfile currentUser = db.UserProfiles.Where(up => up.UserName == User.Identity.Name).FirstOrDefault();
            if (currentUser == null)
                return View(new List<Podcast>());


            return View(currentUser.Podcasts);
        }

        public ActionResult Subscribe(int podcastId)
        {
            UserProfile currentUser = db.UserProfiles.Where(up => up.UserName == User.Identity.Name).FirstOrDefault();
            if (currentUser == null)
                return null;

            Podcast podcast = db.Podcasts.Find(podcastId);
            if (podcast == null)
                return null;

            currentUser.Podcasts.Add(podcast);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Unsubscribe(int podcastId)
        {
            UserProfile currentUser = db.UserProfiles.Where(up => up.UserName == User.Identity.Name).FirstOrDefault();
            if (currentUser == null)
                return null;

            Podcast podcast = db.Podcasts.Find(podcastId);
            if (podcast == null)
                return null;

            currentUser.Podcasts.Remove(podcast);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult FetchEpisodes(int podcastId)
        {
            FetchPodcastEpisodes(podcastId);
            return RedirectToAction("Details");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        private void FetchPodcastEpisodes(int podcastId)
        {
            Podcast podcast = db.Podcasts.Find(podcastId);
            if (podcast == null)
                return;

            var reader = XmlReader.Create(podcast.RSSURI);
            var feed = SyndicationFeed.Load(reader);
            bool saveChanges = false;

            foreach (SyndicationItem episode in feed.Items)
            {
                // We assume if we find a match we're done updating
                if (db.PodcastEpisodes.Where(pe => pe.PodcastId == podcastId && pe.Name == episode.Title.Text).FirstOrDefault() != null)
                    break;

                SyndicationLink fileLocation = episode.Links.Where(l => l.Length > 0).FirstOrDefault();
                if (fileLocation == null)
                    continue;

                PodcastEpisode podcastEpisode = new PodcastEpisode
                {
                    Name = episode.Title.Text,
                    FileLocation = fileLocation.Uri.AbsoluteUri,
                    PublishDate = episode.PublishDate,
                    Summary = episode.Summary.Text
                };

                podcast.Episodes.Add(podcastEpisode);
                saveChanges = true;
            }

            if (saveChanges)
            {
                db.SaveChanges();
            }
        }


        private Podcast PopulatePodcastWithRSS(string rssURL)
        {
            Podcast podcast = null;

            try
            {
                var reader = XmlReader.Create(rssURL);
                var feed = SyndicationFeed.Load(reader);

                podcast = new Podcast 
                { 
                    RSSURI = rssURL,
                    Name = feed.Title.Text,
                    ArtworkURI = feed.ImageUrl.AbsoluteUri,
                    Description = feed.Description.Text
                };
            }
            catch { }

            return podcast;
        }
    }
}