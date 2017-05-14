using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleUrlShorten.Models.Context;
using SimpleUrlShorten.Infrastructure;
using System.Net;

namespace SimpleUrlShorten.Models
{
    public class UrlRepository
    {
        private ApplicationDBContext context;

        public UrlRepository(ApplicationDBContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Url> Urls => context.Urls;

        public void SaveUrl(Url urlData)
        {
            context.Urls.Add(urlData);
            context.SaveChanges();
        }

        // Generate random short URL
        public static string GenerateRandomShortUrl(int urlLength)
        {
            string shortUrl = "";
            int j;
            Random random = new Random();

            for (int i = 0; i < urlLength; i++)
            {
                j = random.Next(0, 35);
                if (j < 10)
                    j += 48; // char code 0-9
                else
                    j += 87; // char code a-z
                shortUrl = shortUrl + char.ConvertFromUtf32(j);
            }
            return shortUrl;
        }

        
        // Check if provided URL contains protocol scheme
        public static bool HasHTTPProtocol(string url)
        {
            url = url.ToLower();
            if (url.Length > 7)
            {
                if (url.StartsWith(Constants.Protocol.HTTP) || url.StartsWith(Constants.Protocol.HTTPS))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        // Check whether provided URL exists by requesting and waiting for response.
        public static async Task<bool> IsExistOriginalUrl(string url)
        {
            int linkLength = url.Length;
            if (!HasHTTPProtocol(url))
                url = Constants.Protocol.HTTP + url;

            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                 HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

    }
}
