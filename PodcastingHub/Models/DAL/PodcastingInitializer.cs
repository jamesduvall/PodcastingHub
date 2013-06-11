using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PodcastingHub.Models;

namespace PodcastingHub.DAL
{
    public class PodcastingInitializer : DropCreateDatabaseIfModelChanges<PodcastingContext>
    {   
        protected override void Seed(PodcastingContext context)
        {
            var podcasts = new List<Podcast>
            {
                new Podcast { Name = "PC Gamer » Podcasts", ArtworkURI = "http://media.pcgamer.com/files/2010/06/new_podlogo141.jpg", Description = "Podcast For the People by the people.", RSSURI="http://www.pcgamer.com/feed/rss2/?cat=29038"},
                new Podcast { Name = "The /Filmcast", ArtworkURI = "http://www.slashfilm.com/slashfilmpodcastblack144.jpg", Description = "A Film / Movie Podcast for the Masses", RSSURI="http://feeds.feedburner.com/filmcast"}
            };

            podcasts.ForEach(p => context.Podcasts.Add(p));
            context.SaveChanges();
        }
    }    
}