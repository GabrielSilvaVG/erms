using Eventra.DTOs;
using Eventra.Services;
using Eventra.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventra.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(IEventService eventService) : ControllerBase
{   
    private readonly IEventService _eventService = eventService;

    [HttpPost]
    [Authorize(Roles = "Organizer,Admin")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO dto)
    {
        var requestingUserId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        // Organizer can only create events for themselves, Admin can create for anyone
        if (!isAdmin && dto.OrganizerId != requestingUserId)
            return Forbid();

        try
        {
            var createdEvent = await _eventService.CreateAsync(dto);
            return StatusCode(201, createdEvent);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEventById(int id)
    {
        var eventItem = await _eventService.GetByIdAsync(id);
        if (eventItem == null)
            return NotFound(new { message = $"Event with ID {id} not found." });
        return Ok(eventItem);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEvents([FromQuery] int page = 1)
    {
        if (page < 1)
            return BadRequest(new { message = "Page must be greater than or equal to 1." });

        const int pageSize = 20;
        var pagedEvents = await _eventService.GetAllPagedAsync(page, pageSize);
        return Ok(pagedEvents);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Organizer,Admin")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDTO dto)
    {
        var requestingUserId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        // Check if event exists and user is the organizer
        var eventToUpdate = await _eventService.GetByIdAsync(id);
        if (eventToUpdate == null)
            return NotFound(new { message = $"Event with ID {id} not found." });

        // Only the organizer who created the event or Admin can update it
        if (eventToUpdate.OrganizerId != requestingUserId && !isAdmin)
            return Forbid();

        try
        {
            await _eventService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Organizer,Admin")]
    public async Task<IActionResult> DeleteEvent(int id)    
    {
        var requestingUserId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        // Check if event exists and user is the organizer
        var eventToDelete = await _eventService.GetByIdAsync(id);
        if (eventToDelete == null)
            return NotFound(new { message = $"Event with ID {id} not found." });

        // Only the organizer who created the event or Admin can delete it
        if (eventToDelete.OrganizerId != requestingUserId && !isAdmin)
            return Forbid();

        try
        {
            await _eventService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    

}
