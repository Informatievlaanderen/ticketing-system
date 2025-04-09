namespace TicketingService.Monitoring;

using System.ComponentModel.DataAnnotations;

public class ConnectionStrings
{
    [Required] public string Ticketing { get; set; } = null!;
}
