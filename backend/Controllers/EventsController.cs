using Eventra.DTOs;
using Eventra.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eventra.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(IEventService eventService) : ControllerBase
{   
    private readonly IEventService _eventService = eventService;

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventDTO dto)
    {
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
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _eventService.GetAllAsync();
        return Ok(events);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventDTO dto)
    {
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
    public async Task<IActionResult> DeleteEvent(int id)    
    {
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
