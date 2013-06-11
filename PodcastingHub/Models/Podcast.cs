using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PodcastingHub.Models
{
    public class Podcast
    {
        public int PodcastId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ArtworkURI { get; set; }
        [Required]
        [Display(Name="Location")]
        public string RSSURI { get; set; }
        public virtual ICollection<PodcastEpisode> Episodes { get; set; }
        public virtual ICollection<UserProfile> Users { get; set; }
    }
}