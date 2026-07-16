namespace KoShop.Web.Models.ViewModels;

public class SupportIndexViewModel
{
    public List<SupportTicket> Tickets { get; set; } = new();
}

public class SupportDetailsViewModel
{
    public SupportTicket Ticket { get; set; } = null!;
}
