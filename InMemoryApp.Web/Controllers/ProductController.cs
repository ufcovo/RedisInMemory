using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public IActionResult Index()
        {
            // First Way
            //if(String.IsNullOrEmpty(_memoryCache.Get<string>("time")))
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString());

            // Second Way
            //if (!_memoryCache.TryGetValue("time", out string timeCache))
            //{
            //    _memoryCache.Set<string>("time", DateTime.Now.ToString(), options);
            //}

            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddSeconds(10);
            //options.SlidingExpiration = TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.High;

            options.RegisterPostEvictionCallback((key, value, reason, state) => { 
                _memoryCache.Set("callback", $"{key}-->{value} => Reason:{reason}");
            });


            _memoryCache.Set<string>("time", DateTime.Now.ToString(), options);

            Product product = new Product { Id = 1, Name = "Pencil", Price = 200 };
            _memoryCache.Set<Product>("product:1", product);


            return View();
        }

        public IActionResult Show()
        {
            //_memoryCache.GetOrCreate<string>("time", entry =>
            //{
            //    return DateTime.Now.ToString();
            //});

            //_memoryCache.Remove("time");

            _memoryCache.TryGetValue("time", out string timeCache);
            _memoryCache.TryGetValue("callback", out string callback);
            ViewBag.timeForCache = timeCache;
            ViewBag.callback = callback;

            ViewBag.time = _memoryCache.Get<string>("time");

            ViewBag.product = _memoryCache.Get<Product>("product:1");

            return View();
        }
    }
}
