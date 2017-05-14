using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleUrlShorten.Models;

namespace SimpleUrlShorten.Models.ViewModels
{
    public class UrlsListViewModel
    {
        public IEnumerable<Url> Url { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentSearch { get; set; }
    }
}
