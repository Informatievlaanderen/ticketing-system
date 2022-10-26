namespace TicketingService.Monitoring;

using System.ComponentModel.DataAnnotations;

public class AppOptions
{
    [Required]
    public string ConnectionString { get; set; }
}
