using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ETicaret.Data;
using ETicaret.Models;

public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CommentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(int productId, string text)
    {
        var user = await _userManager.GetUserAsync(User);

        var comment = new Comment
        {
            ProductId = productId,
            Text = text,
            UserId = user.Id,
            CreatedAt = DateTime.Now
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Product", new { id = productId });
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = _context.Comments.Include(c => c.User).FirstOrDefault(c => c.Id == id);
        if (comment == null) return NotFound();

        var currentUser = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("Admin");

        if (comment.UserId != currentUser.Id && !isAdmin)
        {
            return Forbid();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Product", new { id = comment.ProductId });
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var comment = _context.Comments.Include(c => c.User).FirstOrDefault(c => c.Id == id);
        if (comment == null) return NotFound();

        var currentUser = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("Admin");

        if (comment.UserId != currentUser.Id && !isAdmin)
        {
            return Forbid();
        }

        return View(comment);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(Comment updated)
    {
        var comment = _context.Comments.Include(c => c.User).FirstOrDefault(c => c.Id == updated.Id);
        if (comment == null) return NotFound();

        var currentUser = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("Admin");

        if (comment.UserId != currentUser.Id && !isAdmin)
        {
            return Forbid();
        }

        comment.Text = updated.Text;
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Product", new { id = comment.ProductId });
    }

}
