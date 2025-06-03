using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ETicaret.Models;
using ETicaret.Data;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<Cart> GetOrCreateCart()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(c => c.UserId == user.Id);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = user.Id
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await GetOrCreateCart();
            return View(cart.Items);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var cart = await GetOrCreateCart();
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = id,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var cart = await GetOrCreateCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item != null)
            {
                cart.Items.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            var cart = await GetOrCreateCart();
            cart.Items.Clear();
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Increase(int id)
        {
            var cart = await GetOrCreateCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item != null)
            {
                item.Quantity++;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Decrease(int id)
        {
            var cart = await GetOrCreateCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    cart.Items.Remove(item);
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var user = await _userManager.GetUserAsync(User);

            var addresses = _context.Addresses.Where(a => a.UserId == user.Id).ToList();
            var cards = _context.CreditCards.Where(c => c.UserId == user.Id).ToList();

            ViewBag.Addresses = addresses;
            ViewBag.Cards = cards;

            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CompleteOrder(int SelectedAddressId, int SelectedCardId)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await GetOrCreateCart();

            if (!cart.Items.Any())
                return RedirectToAction("Index");

            var order = new Order
            {
                UserId = user.Id,
                AddressId = SelectedAddressId,
                CardId = SelectedCardId,
                OrderDate = DateTime.Now,
                Total = cart.Items.Sum(i => i.Product.Price * i.Quantity),
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);

            cart.Items.Clear();

            await _context.SaveChangesAsync();

            TempData["OrderSuccess"] = "Siparişiniz başarıyla oluşturuldu!";
            return RedirectToAction("Index");
        }
    }
}
