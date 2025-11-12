using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSys.Models;

public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required, EmailAddress]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [Column("role")]
    public string Role { get; set; } = "Customer"; 

    [Column("full_name")]
    public string? FullName { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}