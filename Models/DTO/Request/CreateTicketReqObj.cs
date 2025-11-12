using System.ComponentModel.DataAnnotations;

namespace TicketSys.Models.DTOs;

public class CreateTicketRequest
{
    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public string Priority { get; set; } = string.Empty;
    
    public string Status { get; set; } = "Requested";
}