using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tags = _context.Tags.ToList();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            var checkTag = _context.Tags.FirstOrDefault(x => x.Name == tag.Name);

            if (checkTag != null)
            {
                ViewBag.TagExist = "Tag is exist";
                return View();
            }
            _context.Tags.Add(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            var tag = _context.Tags.FirstOrDefault(x=>x.Id == id);
            return View(tag);
        }

        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            var checkTag = _context.Tags.FirstOrDefault(x => x.Name == tag.Name);

            if (checkTag != null)
            {
                ViewBag.TagExist = "Tag is exist";
                return View(tag);
            }
            _context.Tags.Update(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var removedTag = _context.Tags.FirstOrDefault(x=>x.Id==id);
            return View(removedTag);
        }


        [HttpPost]
        public IActionResult Delete(Tag tag)
        {
            _context.Tags.Remove(tag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
