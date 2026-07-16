using KoShop.Web.Models;

namespace KoShop.Web.Models.ViewModels;

public class AdminTicketsIndexViewModel
{
    public List<SupportTicket> Tickets { get; set; } = new();
    public string? StatusFilter { get; set; }
}

public class AdminTicketDetailViewModel
{
    public SupportTicket Ticket { get; set; } = null!;
}
