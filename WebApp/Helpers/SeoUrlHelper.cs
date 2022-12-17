using WebApp.Models;

namespace WebApp.Helpers
{
    public static class SeoUrlHelper
    {
        public static string SeoUrl(string url)
        {
            var result = url.ToLower()
                .Replace("ə", "e")
                .Replace("ü", "u")
                .Replace("ı", "i")
                .Replace("ö", "o")
                .Replace("ç", "c")
                .Replace("ş", "s")
                .Replace("ğ", "g")
                .Replace(" ", "-")
                .Replace(",", "-")
                .Replace(".", "");

            return result;
        }
    }
}
