using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PodcastingHub.Models
{
    public class UserProfilePodcastEpisode
    {
        public int UserProfilePodcastEpisodeId { get; set; }
        public int UserProfileId { get; set; }
        public int PodcastEpisodeId { get; set; }
        public bool Listened { get; set; }
        
        public virtual UserProfile UserProfile { get; set; }
        public virtual PodcastEpisode PodcastEpisode { get; set; }        
    }
}