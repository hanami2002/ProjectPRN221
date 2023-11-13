using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN.Models;
using System.Text.Json;
using X.PagedList;

namespace ProjectPRN.Pages.View
{
    public class AdminModel : PageModel
    {
        private readonly BackupContext context;

        public AdminModel()
        {
            this.context = new BackupContext();
        }
        public List<Book> Books { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Order> Orders { get; set; }
        public List<User> UsersList { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public int Check { get; set; }
        public User Users { get; set; }
        public int CountBook { get; set; }
        public int CurrentPage { get; set; }

        public IActionResult OnGet(string? handler, int? oid)
        {
            if(handler == null)
            {
                Check = 1;
            } else
            {
                if (handler.Equals("Dashboard"))
                {
                    Check = 1;
                }
                if (handler.Equals("Book"))
                {
                    Check = 2;
                }
                if (handler.Equals("User"))
                {
                    Check = 3;
                }
                if (handler.Equals("Order"))
                {
                    Check = 4;
                }
            }
          
            Count = context.Events.ToList().Count;
            CountLogin = context.Events.Where(x => x.EventType.Equals("Login")).ToList().Count;
            CountOrder = context.Events.Where(x => x.EventType.Equals("Order")).ToList().Count;
            Orders = context.Orders.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
            else { return Redirect("/accessdenied"); }
            Books = context.Books.OrderByDescending(x => x.BookId).ToList();
            UsersList = context.Users.ToList();
            Genres = context.Genres.ToList();
            Books.ForEach(book =>
            {
                book.Genre = context.Genres.FirstOrDefault(x => x.GenreId == book.GenreId);
            });
            Orders.ForEach(x =>
            {
                x.User = context.Users.FirstOrDefault(y => y.UserId == x.UserId);
            });
            if (oid != null)
            {
                OrderDetails = context.OrderDetails.Where(x=>x.OrderId== oid).ToList();
                OrderDetails.ForEach(x =>
                {
                    x.Book = context.Books.FirstOrDefault(y => y.BookId == x.BookId);
                }
                ); 
            }
            return Page();
        }
        public IActionResult OnPostAddNew()
        {
            Book book = new Book
            {
                Title = Title,
                Author = Author,
                Image = Image,
                GenreId = GenreId,
                Price = (decimal)Price,
                Description = Description

            };
            context.Books.Add(book);
            context.SaveChanges();
            Books = context.Books.OrderByDescending(x => x.BookId).ToList();
            Genres = context.Genres.ToList();
            Books.ForEach(book =>
            {
                book.Genre = context.Genres.FirstOrDefault(x => x.GenreId == book.GenreId);
            });
            return Redirect("/admin?handler=Book");
        }
        [BindProperty]
        public string Title { get; set; }
        [BindProperty]
        public string Author { get; set; }
        [BindProperty]
        public string Image { get; set; }
        [BindProperty]
        public int GenreId { get; set; }
        [BindProperty]
        public double Price { get; set; }
        [BindProperty]
        public string Description { get; set; }
        [BindProperty]
        public int CountLogin { get; set; }
        [BindProperty]
        public int CountOrder { get; set; }
        [BindProperty]
        public int Count { get; set; }
        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string Mess { get; set; }
        public IActionResult OnGetChart()
        {
            List<String> list = new List<string>();
            list.Add("Log in");
            list.Add("Order");
            list.Add("Other");
            List<float> list1 = new List<float>();

            Count = context.Events.ToList().Count;
            CountLogin = context.Events.Where(x => x.EventType.Equals("Login")).ToList().Count;
            CountOrder = context.Events.Where(x => x.EventType.Equals("Order")).ToList().Count;
            list1.Add(CountLogin);
            list1.Add(CountOrder);
            list1.Add(Count - CountLogin - CountOrder);

            var data = new
            {
                List1 = list,
                Num = list1,

            };
            return new JsonResult(data);
        }
        public IActionResult OnPostAddNewUser()
        {
            if (context.Users.FirstOrDefault(x => x.Username.Equals(UserName)) == null )
            {
                User user = new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Username = UserName,
                    Password = Password,
                    Role = "admin"
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
            else
            {
                Mess = "tai khoan da ton tai";
            }
            return Redirect("/admin?handler=User");
        }


    }
}