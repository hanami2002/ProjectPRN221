using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN.Models;
using System.Text.Json;

namespace ProjectPRN.Pages.View
{
    public class CheckOutModel : PageModel
    {
        private readonly BackupContext context;

        public CheckOutModel()
        {
            context = new BackupContext();
        }
        public List<Genre> Genres { get; set; }
        public User Users { get; set; }
        public void OnGet()
        {
            Genres = context.Genres.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
            if (Users != null)
            {
                FirstName = Users.FirstName;
                LastName = Users.LastName;
                Email = Users.Email;
                Phone = Users.Phone;
                Address = Users.Address;
            }
            
        }
     
        public IActionResult OnPostChangeInfo()
        {
            Genres = context.Genres.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
            Users.FirstName= FirstName;
            Users.LastName= LastName;
            Users.Email= Email;
            Users.Phone= Phone;
            Users.Address= Address;
            context.Users.Update(Users);
            context.SaveChanges();
            HttpContext.Session.SetString("user", JsonSerializer.Serialize(Users));
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
            Event e = new Event
            {
                EventName = Users.Username + " change info",
                EventDate = DateTime.Now,
                EventType = "changeinfo",
                UserId = Users.UserId
            };
            Message = "Change info successfully!";
            context.Events.Add(e);
            context.SaveChanges();
            return Page();
        }
        public IActionResult OnPostChangePass()
        {
            Genres = context.Genres.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
            if (Users != null)
            {
                FirstName = Users.FirstName;
                LastName = Users.LastName;
                Email = Users.Email;
                Phone = Users.Phone;
                Address = Users.Address;
                if (Users.Password.Equals(Password))
                {
                    NewMessage = "Doi pass thanh cong";
                    Users.Password = NewPassword;
                    context.Users.Update(Users);
                    context.SaveChanges();
                    HttpContext.Session.SetString("user", JsonSerializer.Serialize(Users));
                    return Page();
                }
                
            }
            NewMessage = "kiem tra lai mat khau";
            return Page();
        }
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string? Message { get; set; }
        [BindProperty]
        public string? NewMessage { get; set; }
        [BindProperty]
        public string NewPassword { get; set; }
    }
}
