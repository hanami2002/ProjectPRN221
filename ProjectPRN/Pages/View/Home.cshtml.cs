using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN.Models;
using System.Security.Cryptography;
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
        public IActionResult OnGetTest()
        {
            User user= context.Users.FirstOrDefault(x=>x.UserId==4);
            return new JsonResult(user);
        }
        public IActionResult OnGetAddToCart(int bid)
        {
            Genres = context.Genres.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
            else
            {
                return Redirect("/cart");
            }
            Order order = context.Orders.Where(x => x.UserId == Users.UserId && x.Status.Equals("process")).FirstOrDefault();
            Book BookDetail = context.Books.FirstOrDefault(x => x.BookId == bid);
            if (order == null)
            {
                order = new Order
                {
                    UserId = Users.UserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    Status = "process"
                };
                context.Orders.Add(order);
                context.SaveChanges();
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = context.Orders.FirstOrDefault(x => x.UserId == Users.UserId && x.Status.Equals("process")).OrderId,
                    BookId = bid,
                    Quantity = 1,
                    Subtotal = BookDetail.Price * 1

                };
                context.OrderDetails.Add(orderDetail);
                context.SaveChanges();
            }
            else
            {
                OrderDetail orderDetail = context.OrderDetails.FirstOrDefault(x => x.OrderId == order.OrderId && x.BookId == bid);
                if (orderDetail == null)
                {
                    OrderDetail detail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        BookId = bid,
                        Quantity = 1,
                        Subtotal = 1 * BookDetail.Price
                    };
                    context.OrderDetails.Add(detail);
                    context.SaveChanges();
                }
                else
                {
                    orderDetail.Quantity = 1;
                    context.OrderDetails.Update(orderDetail);
                    context.SaveChanges();
                }
            }



            return Redirect("/cart");
        }
    }
}
