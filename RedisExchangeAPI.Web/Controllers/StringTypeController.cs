using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }
        public IActionResult Index()
        {
            db.StringSet("name", "Mert Bulut");
            db.StringSet("visitor", 100);
            return View();
        }

        public IActionResult Show()
        {
            var name = db.StringGet("name");
            db.StringIncrement("visitor", 1);
            //var visitorDecr = db.StringDecrementAsync("visitor", 1).Result;
            //db.StringDecrementAsync("visitor", 1).Wait();
            var visitor = db.StringGet("visitor");

            if (name.HasValue)
                ViewBag.name = name.ToString();

            if (visitor.HasValue)
                ViewBag.visitor = visitor.ToString();
            

            return View();
        }
    }
}
