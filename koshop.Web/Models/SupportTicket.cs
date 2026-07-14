using System.ComponentModel.DataAnnotations;

namespace KoShop.Web.Models;

public class SupportTicket
{
    public int Id { get; set; }

    [Required, StringLength(20)]
    public string TicketCode { get; set; } = string.Empty; // e.g. T-204

    public int UserId { get; set; }
    public User? User { get; set; }

    [Required, StringLength(300)]
    public string Subject { get; set; } = string.Empty;

    [StringLength(20)]
    public string Priority { get; set; } = "متوسط"; // بالا / متوسط / پایین

    // Open, Answered, Closed
    public string Status { get; set; } = "Open";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<TicketMessage> Messages { get; set; } = new();
}

public class TicketMessage
{
    public int Id { get; set; }

    public int TicketId { get; set; }
    public SupportTicket? Ticket { get; set; }

    [Required, StringLength(10)]
    public string SenderType { get; set; } = "User"; // User / Admin

    public int SenderId { get; set; }
    public User? Sender { get; set; }

    [Required]
    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
