using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleUrlShorten.Models;
using SimpleUrlShorten.Models.ViewModels;
using SimpleUrlShorten.Infrastructure;

namespace SimpleUrlShorten.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettingsConfig appSettings;
        private UrlRepository repository;
        private int PageSize = 4;

        public HomeController(IOptions<AppSettingsConfig> settingsAccessor, UrlRepository repo)
        {
            appSettings = settingsAccessor.Value;
            repository = repo;
        }

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("list-page{page}")]
        public IActionResult List(string search, int page = 1)
        {
            search = search?.ToLower();
            var searchResult = repository.Urls
                               .Where(u => string.IsNullOrWhiteSpace(search) || u.OriginalUrl.ToLower().Contains(search));
            if (((page - 1) * PageSize) >= searchResult.Count())
            {
                page = 1; 
            }

            var viewResult = searchResult
                             .OrderByDescending(u => u.GeneratedDate)
                             .Skip((page - 1) * PageSize).Take(PageSize);

            return View(new UrlsListViewModel
            {
                Url = viewResult,
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = searchResult.Count()
                },
                CurrentSearch = search
            });
        }

        [Route("{shortUrl}")]
        public IActionResult GoToOriginal(string shortUrl)
        {
            if (string.IsNullOrEmpty(shortUrl))
                return RedirectToAction(nameof(UrlNotFound));
            else
            {
                Url url = repository.Urls.Where(u => u.ShortUrl.Equals(shortUrl, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (url == null)
                    return RedirectToAction(nameof(UrlNotFound));
                else
                {
                    // redirects to the original URL
                    return RedirectPermanent(url.OriginalUrl);
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> ShortenURL(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
                return Json(new { status = false, message = "Please provide URL." });
            else
            {
                if (! UrlRepository.HasHTTPProtocol(originalUrl))
                    originalUrl = "http://" + originalUrl;

                // check whether the URL entered exists
                if (!await UrlRepository.IsExistOriginalUrl(originalUrl))
                {
                    return Json(new { status = false, message = "Could not reach the URL. Please ensure you have enter a valid URL." });
                }

                // Check if original URL already exists in the database
                Url existingURL = repository.Urls.Where(u => u.OriginalUrl.Equals(originalUrl, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (existingURL == null)
                {
                    Url newUrl = new Url()
                    {
                        OriginalUrl = originalUrl,
                        GeneratedDate = DateTime.UtcNow
                    };

                    string randomUrl = "";

                    do // make sure the random short URL does not exist in db 
                    {
                        randomUrl = UrlRepository.GenerateRandomShortUrl(appSettings.UrlLength);
                    } while (repository.Urls.Where(u => u.ShortUrl.Equals(randomUrl, StringComparison.OrdinalIgnoreCase)).Count() > 0);

                    newUrl.ShortUrl = randomUrl;

                    try
                    {
                        repository.SaveUrl(newUrl);
                        newUrl.ShortUrl = Request.Scheme + "://" + Request.Host + "/" + newUrl.ShortUrl;

                        return Json(new { status = true, url = newUrl });
                    }
                    catch (Exception exc)
                    {
                        return Json(new { status = false, message = exc.Message });
                    }
                       
                }
                else
                {
                    existingURL.ShortUrl = Request.Scheme + "://" + Request.Host + "/" + existingURL.ShortUrl;
                    return Json(new { status = true, url = existingURL });
                }
            }
        }

        [Route("url-not-found")]
        public IActionResult UrlNotFound()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("error")]
        public IActionResult Error()
        {
            return View();
        }

    }
}
