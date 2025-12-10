using Eventra.DTOs;
using Eventra.Services;
using Eventra.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eventra.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RegistrationsController(IRegistrationService registrationService) : ControllerBase
{
    private readonly IRegistrationService _registrationService = registrationService;

    // Participant registers for an event
    [HttpPost]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Create([FromBody] CreateRegistrationDTO dto)
    {
        try
        {
            var participantId = User.GetUserId();

            var registration = await _registrationService.CreateAsync(dto.EventId, participantId);
            
            // Return RegistrationResponseDTO
            var response = new RegistrationResponseDTO
            {
                Id = registration.Id,
                RegistrationDate = registration.RegistrationDate,
                EventId = registration.EventId,
                EventTitle = registration.Event.Title,
                EventDate = registration.Event.Date,
                EventPlaceId = registration.Event.PlaceId,
                ParticipantId = registration.ParticipantId,
                ParticipantName = registration.Participant.Name,
                ParticipantEmail = registration.Participant.Email
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = registration.Id },
                response
            );
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

    // Get all registrations (Admin view)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllRegistrations()
    {
        var registrations = await _registrationService.GetAllAsync();
        
        var response = registrations.Select(r => new RegistrationResponseDTO
        {
            Id = r.Id,
            RegistrationDate = r.RegistrationDate,
            EventId = r.EventId,
            EventTitle = r.Event.Title,
            EventDate = r.Event.Date,
            EventPlaceId = r.Event.PlaceId,
            ParticipantId = r.ParticipantId,
            ParticipantName = r.Participant.Name,
            ParticipantEmail = r.Participant.Email
        });

        return Ok(response);
    }

    // Get registration by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var registration = await _registrationService.GetByIdAsync(id);
        
        if (registration == null)
            return NotFound(new { message = $"Registration with ID {id} not found." });

        var response = new RegistrationResponseDTO
        {
            Id = registration.Id,
            RegistrationDate = registration.RegistrationDate,
            EventId = registration.EventId,
            EventTitle = registration.Event.Title,
            EventDate = registration.Event.Date,
            EventPlaceId = registration.Event.PlaceId,
            ParticipantId = registration.ParticipantId,
            ParticipantName = registration.Participant.Name,
            ParticipantEmail = registration.Participant.Email
        };

        return Ok(response);
    }

    // Get all registrations for a specific event
    [HttpGet("event/{eventId}")]
    public async Task<IActionResult> GetByEventId(int eventId)
    {
        var registrations = await _registrationService.GetByEventIdAsync(eventId);
        
        var response = registrations.Select(r => new ParticipantInfoDTO
        {
            Id = r.ParticipantId,
            Name = r.Participant.Name,
            Email = r.Participant.Email,
            RegistrationDate = r.RegistrationDate
        });

        return Ok(response);
    }

    // Get all registrations for a specific participant
    [HttpGet("participant/{participantId}")]
    public async Task<IActionResult> GetByParticipantId(int participantId)
    {
        var registrations = await _registrationService.GetByParticipantIdAsync(participantId);
        
        var response = registrations.Select(r => new EventInfoDTO
        {
            Id = r.EventId,
            Title = r.Event.Title,
            Date = r.Event.Date,
            PlaceId = r.Event.PlaceId,
            Status = r.Event.Status.ToString()
        });

        return Ok(response);
    }

    // Get current user's registrations
    [HttpGet("my-registrations")]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> GetMyRegistrations()
    {
        var participantId = User.GetUserId();
        var registrations = await _registrationService.GetByParticipantIdAsync(participantId);
        
        var response = registrations.Select(r => new EventInfoDTO
        {
            Id = r.EventId,
            Title = r.Event.Title,
            Date = r.Event.Date,
            PlaceId = r.Event.PlaceId,
            Status = r.Event.Status.ToString()
        });

        return Ok(response);
    }

    // Cancel registration
    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelRegistration(int id)
    {
        var registration = await _registrationService.GetByIdAsync(id);
        if (registration == null)
            return NotFound(new { message = $"Registration with ID {id} not found." });

        var userId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        // Only the participant who registered or Admin can cancel
        if (registration.ParticipantId != userId && !isAdmin)
            return Forbid(); // 403

        try
        {
            await _registrationService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
