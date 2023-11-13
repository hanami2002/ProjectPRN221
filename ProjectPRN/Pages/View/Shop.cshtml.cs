using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN.Models;
using System.Text.Json;

namespace ProjectPRN.Pages.View
{
    public class ShopModel : PageModel
    {
        private readonly BackupContext context;

        public ShopModel()
        {
            context = new BackupContext();
        }
        public List<Genre> Genres { get; set; }
        public List<Book> Books { get; set; }

        public User Users { get; set; }
        public void OnGet(int? gid=1)
        {
            Genres = context.Genres.ToList();
            Books = context.Books.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
        }
    }
}
