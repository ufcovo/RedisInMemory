using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;
        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            _distributedCache.SetString("name", "mert", cacheEntryOptions);
            await _distributedCache.SetStringAsync("surname", "bulut", cacheEntryOptions);

            Product product = new Product { Id = 1, Name = "Pencil", Price = 100};
            string jsonProduct = JsonConvert.SerializeObject(product);
            await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            _distributedCache.Set("product:2", byteProduct, cacheEntryOptions);


            return View();
        }
        public IActionResult Show()
        {
            string name = _distributedCache.GetString("name");
            ViewBag.Name = name;
            string jsonProduct = _distributedCache.GetString("product:1");
            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);
            ViewBag.Product = product;

            Byte[] byteProduct = _distributedCache.Get("product:2");
            string jsonProduct2 = Encoding.UTF8.GetString(byteProduct);
            Product product2 = JsonConvert.DeserializeObject<Product>(jsonProduct2);
            ViewBag.Product2 = product2;
            return View();
        }
        public IActionResult Remove()
        {
            _distributedCache.Remove("name");
            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] imageByte = _distributedCache.Get("image");
            return File(imageByte, "image/png");
        }
        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/a.png");
            byte[] imagesByte = System.IO.File.ReadAllBytes(path);
            _distributedCache.Set("image", imagesByte);

            return View();
        }
    }
}
