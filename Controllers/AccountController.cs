using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ETicaret.Models;
using ETicaret.Data;
using ETicaret.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login() => View();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Geçersiz giriş.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register() => View();

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            return View(user);
        }

        public async Task<IActionResult> Addresses()
        {
            var user = await _userManager.GetUserAsync(User);
            var addresses = _context.Addresses.Where(a => a.UserId == user.Id).ToList();
            return View(addresses);
        }

        [HttpGet]
        public IActionResult AddAddress()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(Address address)
        {
            var user = await _userManager.GetUserAsync(User);
            address.UserId = user.Id;

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction("Addresses");
            }

            return View(address);
        }
        public async Task<IActionResult> CreditCards()
        {
            var user = await _userManager.GetUserAsync(User);
            var cards = _context.CreditCards.Where(c => c.UserId == user.Id).ToList();
            return View(cards);
        }

        [HttpGet]
        public IActionResult AddCard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCard(CreditCard card)
        {
            var user = await _userManager.GetUserAsync(User);
            card.UserId = user.Id;

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                _context.CreditCards.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction("CreditCards");
            }

            return View(card);
        }
        [HttpGet]
        public async Task<IActionResult> EditAddress(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var address = _context.Addresses.FirstOrDefault(a => a.Id == id && a.UserId == user.Id);
            if (address == null) return NotFound();
            return View(address);
        }
        [HttpPost]
        public async Task<IActionResult> EditAddress(Address address)
        {
            var user = await _userManager.GetUserAsync(User);
            address.UserId = user.Id;

            ModelState.Remove("User");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                _context.Addresses.Update(address);
                await _context.SaveChangesAsync();
                return RedirectToAction("Addresses");
            }

            return View(address);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var address = _context.Addresses.FirstOrDefault(a => a.Id == id && a.UserId == user.Id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Addresses");
        }
        [HttpGet]
        public async Task<IActionResult> EditCard(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var card = _context.CreditCards.FirstOrDefault(c => c.Id == id && c.UserId == user.Id);
            if (card == null) return NotFound();
            return View(card);
        }
        [HttpPost]
        public async Task<IActionResult> EditCard(CreditCard card)
        {
            var user = await _userManager.GetUserAsync(User);
            card.UserId = user.Id;

            ModelState.Remove("User");
            ModelState.Remove("UserId");

            if (ModelState.IsValid)
            {
                _context.CreditCards.Update(card);
                await _context.SaveChangesAsync();
                return RedirectToAction("CreditCards");
            }

            return View(card);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCard(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var card = _context.CreditCards.FirstOrDefault(c => c.Id == id && c.UserId == user.Id);
            if (card != null)
            {
                _context.CreditCards.Remove(card);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("CreditCards");
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Success"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var model = new EditProfileViewModel
            {
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Bilgileriniz güncellendi.";
                return RedirectToAction("Profile");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);

            var orders = _context.Orders
                .Include(o => o.Address)
                .Include(o => o.Card)
                .Where(o => o.UserId == user.Id)
                .ToList();

            return View(orders);
        }
        [Authorize]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Include(o => o.Address)
                .Include(o => o.Card)
                .FirstOrDefault(o => o.Id == id && o.UserId == user.Id);

            if (order == null) return NotFound();

            return View(order);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = _context.Orders.FirstOrDefault(o => o.Id == id && o.UserId == user.Id);

            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            TempData["OrderCancelled"] = "Sipariş iptal edildi.";
            return RedirectToAction("Orders");
        }
    }
}
