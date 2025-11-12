using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketSys.Models;

public class Ticket
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Column("title")]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [Column("description")]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Column("status")]
    public string Status { get; set; } = "Open"; // Open, InProgress, Closed
    
    [Column("priority")]
    public string Priority { get; set; } = "Medium"; // Low, Medium, High
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign Key
    [Column("user_id")]
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}