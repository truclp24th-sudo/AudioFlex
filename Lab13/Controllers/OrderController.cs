using Microsoft.AspNetCore.Mvc;
using AudioFlex.Extensions;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace AudioFlex.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGenericRepository<OrderDetail> _orderDetailRepo;

        public OrderController(IOrderRepository orderRepository, IGenericRepository<OrderDetail> orderDetailRepo)
        {
            _orderRepository = orderRepository;
            _orderDetailRepo = orderDetailRepo;
        }

        // GET: /Order/Checkout
        [Authorize]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            return View();
        }

        // POST: /Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Checkout(Order model)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart");
            if (cart == null || !cart.Any()) return RedirectToAction("Index", "Cart");

            model.OrderDate = DateTime.Now;
            model.TotalAmount = cart.Sum(c => c.Total);
            model.Status = "Đang xử lý";
            model.AccountId = HttpContext.Session.GetInt32("UserId");

            await _orderRepository.AddAsync(model);
            await _orderRepository.SaveChangesAsync();

            foreach (var item in cart)
            {
                var detail = new OrderDetail
                {
                    OrderId = model.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                await _orderDetailRepo.AddAsync(detail);
            }
            await _orderDetailRepo.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        // ============================================================
        // MỚI: LỊCH SỬ ĐƠN HÀNG CỦA KHÁCH HÀNG
        // ============================================================
        // Trong file Controllers/OrderController.cs
        [Authorize]
        public async Task<IActionResult> History()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            // Thay đổi ở đây: Dùng hàm mới lấy đủ dữ liệu ảnh
            var userOrders = await _orderRepository.GetOrdersByUserIdWithDetailsAsync(userId.Value);

            return View(userOrders);
        }
    }
}