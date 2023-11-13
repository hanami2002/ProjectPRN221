using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN.Models;
using System.Text.Json;
using static NuGet.Packaging.PackagingConstants;

namespace ProjectPRN.Pages.View
{
    public class HomeModel : PageModel
    {
        private readonly BackupContext context;

        public HomeModel()
        {
            context = new BackupContext();
        }
        public List<Genre> Genres { get; set; }
        public List<Book> Books { get; set; }
        public User Users { get; set; }


        public void OnGet()
        {
            Genres = context.Genres.ToList();
            Books = context.Books.Take(8).ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
        }
    }
}
