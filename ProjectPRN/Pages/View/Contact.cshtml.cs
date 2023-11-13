using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN.Models;
using System.Text.Json;

namespace ProjectPRN.Pages.View
{
    public class ContactModel : PageModel
    {
        private readonly BackupContext context;

        public ContactModel()
        {
            context = new BackupContext();
        }
        public List<Genre> Genres { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public User Users { get; set; }
        [BindProperty]
        public string? Message { get; set; }

        public void OnGet(int? odid)
        {
            Genres = context.Genres.ToList();
            if (odid == null)
            {
                if (HttpContext.Session.GetString("user") != null)
                {
                    string data = HttpContext.Session.GetString("user");
                    Users = JsonSerializer.Deserialize<User>(data);
                    Orders = context.Orders.Where(x => x.UserId == Users.UserId).ToList();
                }
            }
            else
            {
                if (HttpContext.Session.GetString("user") != null)
                {
                    string data = HttpContext.Session.GetString("user");
                    Users = JsonSerializer.Deserialize<User>(data);
                    OrderDetails =context.OrderDetails.Where(x=>x.OrderId==odid).ToList();
                    OrderDetails.ForEach(x=>
                    x.Book=context.Books.FirstOrDefault(y=>y.BookId==x.BookId));
                }
            }

            //}
            //public IActionResult OnGetOrderDetailByOrder(int orderId)
            //{
            //    List<OrderDetail > list= context.OrderDetails.Where(x=>x.OrderId== orderId).ToList();
            //    list.ForEach(x => x.Book = context.Books.FirstOrDefault(y => y.BookId == x.BookId));
            //    return new JsonResult(list);
            //}

        }
    } }
