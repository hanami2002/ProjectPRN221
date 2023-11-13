using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN.Models;
using System.Text.Json;

namespace ProjectPRN.Pages.Common
{
    public class RegisterModel : PageModel
    {
        private readonly BackupContext _context;
        public RegisterModel()
        {
            _context = new BackupContext();
        }
        [BindProperty] public string FirstName { get; set; }
        [BindProperty] public string Lastname { get; set; }
        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        [BindProperty] public string? Message { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            User user = _context.Users.FirstOrDefault(x => x.Username == Username);
            if (user == null)
            {
                user = new User
                {
                    Username = Username,
                    FirstName = FirstName,
                    Password = Password,
                    LastName = Lastname,
                    Role="customer"
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                HttpContext.Session.SetString("user", JsonSerializer.Serialize(user));
                return Redirect("/home");
            }
            else
            {
                Message = "tai khoan da ton tai";
                return Page();
            }
        }
    }
}
