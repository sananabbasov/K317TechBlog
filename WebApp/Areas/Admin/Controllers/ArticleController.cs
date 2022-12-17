using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _env;

        public ArticleController(AppDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _env = env;
        }

        public IActionResult Index()
        {
            var articles = _context.Articles
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.ArticleTags)
                .ThenInclude(x => x.Tag)
                .Where(data => data.IsDeleted == false).ToList();
            return View(articles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            var tags = _context.Tags.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewData["Tags"] = tags;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Article article, List<int> Tags, IFormFile Photo)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            article.UserId = userId;
            var categories = _context.Categories.ToList();
            var tags = _context.Tags.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewData["Tags"] = tags;

            var path = "/uploads/" + Guid.NewGuid() + Photo.FileName;
            using (var fileStream = new FileStream(_env.WebRootPath + path, FileMode.Create))
            {
                Photo.CopyTo(fileStream);
            }

            var seo_url = article.Title.ToLower()
                .Replace("ə", "e").Replace("ü", "u").Replace("ş", "s").Replace("ğ", "g").Replace(" ","-").Replace(".", "");


            article.CreatedDate= DateTime.Now;
            article.UpdatedDate= DateTime.Now;
            article.SeoUrl = seo_url;
            article.PhotoUrl = path;
            article.Views = 0;

            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
            List<ArticleTag> tagList = new();

            for (int i = 0; i < Tags.Count; i++)
            {
                ArticleTag articleTag = new()
                {
                    ArticleId = article.Id,
                    TagId = Tags[i]
                };
                tagList.Add(articleTag);
            }

            await _context.ArticleTags.AddRangeAsync(tagList);
            await _context.SaveChangesAsync();

            return View();
        }


        public IActionResult Edit(int id)
        {
            return View();
        }
    }
}
