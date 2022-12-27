using WebApp.Models;

namespace WebApp.ViewModels
{
    public class DetailVM
    {
        public Article Article { get; set; }
        public List<Article> Suggestions { get; set; }
    }
}
