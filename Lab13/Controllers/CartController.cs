using Microsoft.AspNetCore.Mvc;
using AudioFlex.Extensions;
using AudioFlex.Models;
using AudioFlex.Repositories.Interfaces;

namespace AudioFlex.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart";
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        private List<CartItem> GetCart()
        {
            return HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetObject(CartSessionKey, cart);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // POST: /Cart/Add - Cập nhật: Nhận thêm imagePath để phân biệt màu
        [HttpPost]
        public async Task<IActionResult> Add(int productId, string imagePath, int quantity = 1)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return Json(new { success = false });

            // CHỈ lấy ảnh mặc định NẾU imagePath gửi lên bị trống
            var finalImg = string.IsNullOrEmpty(imagePath) ? product.ImagePath : imagePath;

            var cart = GetCart();
            var existing = cart.FirstOrDefault(c => c.ProductId == productId && c.ImagePath == finalImg);

            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ImagePath = finalImg, // Lưu đúng màu đã chọn
                    Price = product.Price,
                    Quantity = quantity
                });
            }
            SaveCart(cart);
            return Json(new { success = true, cartCount = cart.Sum(c => c.Quantity) });
        }

        // POST: /Cart/UpdateQuantity - Cập nhật để tìm đúng dòng dựa trên ID và Ảnh
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, string imagePath, int quantity)
        {
            var cart = GetCart();
            // Phải tìm theo cả 2 tiêu chí
            var item = cart.FirstOrDefault(c => c.ProductId == productId && c.ImagePath == imagePath);
            if (item != null)
            {
                if (quantity <= 0) cart.Remove(item);
                else item.Quantity = quantity;
                SaveCart(cart);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: /Cart/Remove - Cập nhật để xóa đúng màu đã chọn
        [HttpPost]
        public IActionResult Remove(int productId, string imagePath)
        {
            var cart = GetCart();
            // Tìm chính xác dòng cần xóa dựa trên ID và Ảnh
            var item = cart.FirstOrDefault(c => c.ProductId == productId && c.ImagePath == imagePath);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CartSessionKey);
            return RedirectToAction(nameof(Index));
        }
    }
}