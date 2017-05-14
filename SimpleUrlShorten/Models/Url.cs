using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SimpleUrlShorten.Models
{
    public class Url
    {
        public int UrlId { get; set; }

        [Required]
        public string OriginalUrl { get; set; }

        [Required]
        public string ShortUrl { get; set; }
        
        [Required]
        public DateTime GeneratedDate { get; set; }
        

        

    }
}
