using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodcastingHub.Models
{
    public class PodcastEpisode
    {
        public int PodcastEpisodeId { get; set; }
        public int PodcastId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public string FileLocation { get; set; }
        public string Summary { get; set; }
        public virtual Podcast Podcast { get; set; }
        public virtual List<UserProfilePodcastEpisode> UserProfilePodcastEpisodes { get; set; }
    }
}