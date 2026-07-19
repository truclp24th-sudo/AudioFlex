using Microsoft.AspNetCore.Mvc;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Areas.Admin.Controllers
{
    /// <summary>
    /// Quản lý Đơn hàng trong khu vực Admin: xem danh sách, xem chi tiết,
    /// cập nhật trạng thái và xóa đơn hàng.
    /// </summary>
    [Area("Admin")]
    public class OrdersController : AdminBaseController
    {
        private readonly IOrderRepository _orderRepository;

        // Danh sách trạng thái đơn hàng hợp lệ
        private static readonly string[] StatusList = { "Đang xử lý", "Đang giao", "Đã giao", "Đã hủy" };

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Danh sách đơn hàng";
            ViewData["ActiveMenu"] = "Orders";
            var orders = await _orderRepository.GetAllWithDetailsAsync();
            return View(orders);
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewData["Title"] = "Chi tiết đơn hàng";
            ViewData["ActiveMenu"] = "Orders";

            var order = await _orderRepository.GetByIdWithDetailsAsync(id);
            if (order == null) return NotFound();

            ViewBag.StatusList = StatusList;
            return View(order);
        }

        // POST: Admin/Orders/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();

            order.Status = status;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();

            TempData["Success"] = "Cập nhật trạng thái đơn hàng thành công!";
            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();

            _orderRepository.Remove(order);
            await _orderRepository.SaveChangesAsync();

            TempData["Success"] = "Xóa đơn hàng thành công!";
            return RedirectToAction(nameof(Index));
        }
    }
}
