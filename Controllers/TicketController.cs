using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TicketSys.Data;
using TicketSys.Models;
using TicketSys.Models.DTOs;

namespace TicketSys.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext context;
    
    public TicketsController(AppDbContext dbContext)
    {
        context = dbContext;
    }
    
    // Customer: Create ticket
    [HttpPost("CreateTicket")]
    public async Task<IActionResult> CreateTicket(CreateTicketRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var ticket = new Ticket
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Status = "Open",
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };
        
        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
    }
    
    // Customer: View own tickets | Admin: View all tickets
    [HttpGet("GetTickets")]
    public async Task<IActionResult> GetTickets()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        
        var tickets = role == "Admin"
            ? await context.Tickets.Include(t => t.User).ToListAsync()
            : await context.Tickets.Where(t => t.UserId == userId).ToListAsync();
        
        return Ok(tickets);
    }
    
    // Get single ticket
    [HttpGet("GetSingleTickets/{id}")]
    public async Task<IActionResult> GetTicket(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        
        var ticket = await context.Tickets.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
        
        if (ticket == null)
            return NotFound();
        
        // Customer can only view own tickets
        if (role != "Admin" && ticket.UserId != userId)
            return Forbid();
        
        return Ok(ticket);
    }
    
    // Admin: Update ticket status
    [Authorize(Roles = "Admin")]
    [HttpPatch("UpdateTicket/{id}")]
    public async Task<IActionResult> UpdateTicket(int id, UpdateTicketRequest request)
    {
        var ticket = await context.Tickets.FindAsync(id);
        
        if (ticket == null)
            return NotFound();
        
            ticket.Status = request.Status;
            ticket.Description = request.Description;
            ticket.Priority = request.Priority;
            ticket.Title = request.Title;
        
        ticket.UpdatedAt = DateTime.UtcNow;
        
        await context.SaveChangesAsync();
        
        return Ok(ticket);
    }
}
