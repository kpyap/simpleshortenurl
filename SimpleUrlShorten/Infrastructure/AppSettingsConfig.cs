using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleUrlShorten.Infrastructure
{
    public class AppSettingsConfig
    {
        public int UrlLength { get; set; } = 8;
    }
}
