using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ArticleController(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
                return NotFound();

            var article = _context.Articles
                                  .Include(x => x.User)
                                  .Include(x => x.Category)
                                  .Include(x => x.ArticleTags)
                                  .ThenInclude(x => x.Tag)
                                  .FirstOrDefault(x => x.Id == id);

            if (article == null)
                return NotFound();

            var cookie = _contextAccessor.HttpContext.Request.Cookies[$"Views"];
            string[] findCookie = {""}; // ["1","5"]
            if (cookie != null)
            {
                findCookie =  cookie.Split("-").ToArray();
            }

            if (!findCookie.Contains(article.Id.ToString()))
            {
                Response.Cookies.Append($"Views", $"{cookie}-{article.Id}", // 1-2
                    new CookieOptions
                    {
                        Secure = true,
                        HttpOnly = true,
                        Expires = DateTime.Now.AddDays(1)
                    }
                    );
                article.Views += 1;
                _context.Articles.Update(article);
                _context.SaveChanges();
            }

            DetailVM detailVM = new()
            {
                Article = article
            };
            return View(detailVM);
        }
    }
}
