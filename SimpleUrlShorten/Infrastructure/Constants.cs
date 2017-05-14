using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleUrlShorten.Infrastructure
{
    public class Constants
    {
        /// <summary>
        /// Protocols supported in this application.
        /// </summary>
        public class Protocol
        {
            public const string HTTP = "http://";
            public const string HTTPS = "https://";
        }
    }
}
