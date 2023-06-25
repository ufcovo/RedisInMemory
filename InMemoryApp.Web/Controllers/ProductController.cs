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
            if(String.IsNullOrEmpty(_memoryCache.Get<string>("time")))
                _memoryCache.Set<string>("time", DateTime.Now.ToString());

            // Second Way
            if(!_memoryCache.TryGetValue("time", out string timeCache))
                _memoryCache.Set<string>("time", DateTime.Now.ToString());
            
            return View();
        }

        public IActionResult Show()
        {
            _memoryCache.GetOrCreate<string>("time", entry =>
            {
                return DateTime.Now.ToString();
            });

            _memoryCache.Remove("time");
            ViewBag.time = _memoryCache.Get<string>("time");
            return View();
        }
    }
}
