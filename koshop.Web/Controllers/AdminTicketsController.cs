using KoShop.Web.Data;
using KoShop.Web.Models;
using KoShop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KoShop.Web.Controllers;

[Authorize(AuthenticationSchemes = "AdminCookieAuth", Roles = "Admin")]
public class AdminTicketsController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminTicketsController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index(string? status)
    {
        var query = _db.SupportTickets.Include(t => t.User).AsQueryable();
        if (!string.IsNullOrEmpty(status) && status != "all")
            query = query.Where(t => t.Status == status);

        var model = new AdminTicketsIndexViewModel
        {
            Tickets = await query.OrderByDescending(t => t.CreatedAt).ToListAsync(),
            StatusFilter = status
        };
        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var ticket = await _db.SupportTickets
            .Include(t => t.User)
            .Include(t => t.Messages).ThenInclude(m => m.Sender)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null) return NotFound();
        return View(new AdminTicketDetailViewModel { Ticket = ticket });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reply(int ticketId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            TempData["Error"] = "متن پاسخ را وارد کنید";
            return RedirectToAction("Details", new { id = ticketId });
        }

        var ticket = await _db.SupportTickets.FindAsync(ticketId);
        if (ticket == null) return NotFound();

        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        _db.TicketMessages.Add(new TicketMessage
        {
            TicketId = ticketId,
            SenderType = "Admin",
            SenderId = adminId,
            Message = message.Trim(),
            CreatedAt = DateTime.UtcNow
        });
        ticket.Status = "Answered";
        await _db.SaveChangesAsync();

        TempData["Success"] = "پاسخ ارسال شد";
        return RedirectToAction("Details", new { id = ticketId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Close(int ticketId)
    {
        var ticket = await _db.SupportTickets.FindAsync(ticketId);
        if (ticket == null) return NotFound();

        ticket.Status = "Closed";
        await _db.SaveChangesAsync();

        TempData["Success"] = "تیکت بسته شد";
        return RedirectToAction("Details", new { id = ticketId });
    }
}
