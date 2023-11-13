using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN.Models;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace ProjectPRN.Pages.View
{
    public class DetailModel : PageModel
    {
        private readonly BackupContext context;

        public DetailModel()
        {
            context = new BackupContext();
        }
        public Book BookDetail { get; set; }
        public List<Book> Books { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Genre> GenreList { get; set; }
          
        public User Users { get; set; }

        public void OnGet(int bid=1)
        {
            Genres = context.Genres.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
            Bid=bid;
            BookDetail = context.Books.FirstOrDefault(x => x.BookId == bid);
            // GenreList = BookDetail.Genres.ToList();
            //Books = context.Books.Where(x=>x.Genres.Where(y=>y.))
            //Books = context.Books.Include(x => x.Genres).Where(x => x.Genres == BookDetail.)
        }
        public IActionResult OnPostAddToOrderDetail()
        {
            Genres = context.Genres.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
            
            Order order = context.Orders.Where(x => x.UserId == Users.UserId&&x.Status.Equals("process")).FirstOrDefault();
            BookDetail = context.Books.FirstOrDefault(x => x.BookId == Bid);
            if (order == null)
            {
                order= new Order
                {
                    UserId= Users.UserId,
                    OrderDate=DateTime.Now,
                    TotalAmount=0,
                    Status="process"
                };
                context.Orders.Add(order);
                context.SaveChanges();
                OrderDetail orderDetail = new OrderDetail
                {
                    OrderId = context.Orders.FirstOrDefault(x => x.UserId == Users.UserId && x.Status.Equals("process")).OrderId,
                    BookId = Bid,
                    Quantity = Quantities,
                    Subtotal = BookDetail.Price * Quantities

                };
                context.OrderDetails.Add(orderDetail);
                context.SaveChanges();
            }
            else
            {
                OrderDetail orderDetail = context.OrderDetails.FirstOrDefault(x => x.OrderId == order.OrderId && x.BookId == Bid);
                if(orderDetail == null)
                {
                    OrderDetail detail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        BookId = Bid,
                        Quantity = Quantities,
                        Subtotal = Quantities * BookDetail.Price
                    };
                    context.OrderDetails.Add(detail);
                    context.SaveChanges();
                }
                else
                {
                    orderDetail.Quantity = Quantities;
                    context.OrderDetails.Update(orderDetail);
                    context.SaveChanges();
                }
            }
            
            return Redirect("/cart");

        }
        [BindProperty]
        public int Quantities { get; set; }
        [BindProperty]
        public int Bid { get; set; }

    }

}
