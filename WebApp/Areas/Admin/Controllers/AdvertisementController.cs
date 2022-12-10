using Microsoft.AspNetCore.Mvc;
using WebApp.Data;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertisementController : Controller
    {
        private readonly AppDbContext _context;

        public AdvertisementController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ads = _context.Advertisements.ToList();
            return View(ads);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
