using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN.Models;
using System.Text.Json;

namespace ProjectPRN.Pages.View
{
    public class CartModel : PageModel
    {
        private readonly BackupContext context;

        public CartModel()
        {
            context = new BackupContext();
        }
        public Book BookDetail { get; set; }
        public List<Book> Books { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Genre> GenreList { get; set; }
        public List<OrderDetail> OrderDetailList { get; set; }
        public decimal? Total { get; set; }

        public User Users { get; set; }
        public void OnGet()
        {
            Total = 0;
            Genres = context.Genres.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
                Order order = context.Orders.Where(x => x.UserId == Users.UserId && x.Status.Equals("process")).FirstOrDefault();
                if (order != null)
                {
                    Odid = order.OrderId;
                    OrderDetailList = context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                    if (OrderDetailList == null)
                    {
                        Mess = "Chua co don hang nao";
                    }
                    else
                    {
                        Mess = null;
                        OrderDetailList.ForEach(x =>
                        {
                            x.Book = context.Books.FirstOrDefault(y => y.BookId == x.BookId);
                            Total += x.Subtotal;
                        });
                    }
                }
                else
                {

                    Mess = "Hay tao moi don hang ";
                }
            }
            else
            {
                Mess = "Hay dang nhap de xem thong tin don hang";
            }
          
            
           
        }
        //public List<OrderDetail> GetOrderDetailsByOrder();
        
        public void OnGetDeleteCart(int? odid)
        {
            Total = 0;
            Genres = context.Genres.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }
            
            Order order = context.Orders.Where(x => x.UserId == Users.UserId && x.Status.Equals("process")).FirstOrDefault();
            if (order != null)
            {
                Odid = order.OrderId;
                OrderDetailList = context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
                OrderDetail orderDetail = context.OrderDetails.FirstOrDefault(x => x.OrderDetailId == odid);

                OrderDetailList.Remove(orderDetail);
                OrderDetailList.ForEach(x =>
                {
                    x.Book = context.Books.FirstOrDefault(y => y.BookId == x.BookId);
                    Total += x.Subtotal;
                });

                context.SaveChanges();
            }
            
        }
        public IActionResult OnPost(int[] bid, int[] quantity)
        {
            for(int i = 0;i<bid.Length;i++)
            {
                OrderDetail orderDetail = context.OrderDetails.FirstOrDefault(x => x.BookId == bid[i] && x.OrderId==Odid);
                orderDetail.Quantity = quantity[i];
                context.OrderDetails.Update(orderDetail);
                
            }
            context.SaveChanges();

            Total = 0;
            Genres = context.Genres.ToList();
            if (HttpContext.Session.GetString("user") != null)
            {
                string data = HttpContext.Session.GetString("user");
                Users = JsonSerializer.Deserialize<User>(data);
            }

            Order order = context.Orders.Where(x => x.UserId == Users.UserId && x.Status.Equals("process")).FirstOrDefault();
            Odid = order.OrderId;
            OrderDetailList = context.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();
            OrderDetailList.ForEach(x =>
            {
                x.Book = context.Books.FirstOrDefault(y => y.BookId == x.BookId);
                Total += x.Subtotal;
            });
            Mess = "Dat hang thanh cong";
            order.Status = "done";
            Event e = new Event
            {
                EventName = Users.Username + " Order ",
                EventDate = DateTime.Now,
                EventType = "Order",
                UserId = Users.UserId
            };
            context.Events.Add(e);  
            context.Orders.Update(order);
            context.SaveChanges();
            return Page();
        }
        [BindProperty]
        public int? Odid { get; set; }
        [BindProperty]
        public string? Mess { get; set; }
    }
}
