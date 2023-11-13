using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN.Models;
using System.Text.Json;
using static NuGet.Packaging.PackagingConstants;

namespace ProjectPRN.Pages.Common
{
    public class LoginModel : PageModel
    {
        private readonly BackupContext context;

        public LoginModel()
        {
            this.context = new BackupContext();
        }

        public void OnGet()
        {
            
        }
        public IActionResult OnGetLogout()
        {
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
                HttpContext.Session.Remove("user"); 
            }

            return Redirect("/home");
        }
        public IActionResult OnPostLogin()
        {
            User user = context.Users.Where(x => x.Username.Equals(Username)&& x.Password.Equals(Password)).FirstOrDefault();
            if (user != null)
            {
                HttpContext.Session.SetString("user", JsonSerializer.Serialize(user));
                if (user.Role.Equals("customer"))
                {
                    Event e = new Event
                    {
                        EventName = user.Username + " login",
                        EventDate = DateTime.Now,
                        EventType = "Login",
                        UserId = user.UserId
                    };
                    context.Events.Add(e);
                    context.SaveChanges();
                    return Redirect("/home");
                }
                else
                {
                    return Redirect("/register");
                }

            }
            else
            {
                Message = "Sai tk hoac mat khau";
                return Page();
            }

        }
        [BindProperty]
        public string? Message { get; set; }

        [BindProperty]
        public string? Username { get; set; }
        [BindProperty]
        public string? Password { get; set; }
        public User? Users { get;  set; }
    }
}
