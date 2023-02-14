using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebPocHub.Dal;
using WebPocHub.Models;
using WebPocHub.WebApi.DTO;

namespace WebPocHub.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[EnableCors("PublicPolicy")]
	public class EventsController : ControllerBase
	{
		private readonly ICommonRepository<Event> _eventRepository;
		private readonly IMapper _mapper;

		public EventsController(ICommonRepository<Event> eventRepository, IMapper mapper)
		{
			_eventRepository = eventRepository;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Roles = "Employee,Hr")]
		public async Task<ActionResult<IEnumerable<EventDTO>>> Get()
		{
			var events = await _eventRepository.GetAll();

			if (events == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<IEnumerable<EventDTO>>(events));
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Roles = "Employee,Hr")]
		public async Task<ActionResult<EventDTO>> GetDetails(int id)
		{
			var events = await _eventRepository.GetDetails(id);
			return events == null ? NotFound() : Ok(_mapper.Map<EventDTO>(events));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "Employee,Hr")]
		public async Task<ActionResult> Create(NewEventDTO? events)
		{
			var eventModel = _mapper.Map<Event>(events);

			var result = await _eventRepository.Insert(eventModel);

			if (result != null)
			{
				var eventsDetails = _mapper.Map<EventDTO>(eventModel);
				return CreatedAtAction("GetDetails", new { id = eventsDetails.EventId }, eventsDetails);
			}

			return BadRequest();
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "Employee,Hr")]
		public async Task<ActionResult> Update(UpdateEventDTO events)
		{
			var eventModel = _mapper.Map<Event>(events);

			var result = await _eventRepository.Update(eventModel);

			if (result != null)
			{
				return NoContent();
			}

			return BadRequest();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Roles = "Hr")]
		public async Task<ActionResult> Delete(int id)
		{
			var events = await _eventRepository.GetDetails(id);

			if (events == null)
			{
				return NotFound();
			}
			else
			{
				await _eventRepository.Delete(events.EventId);
				return NoContent();
			}
		}
	}
}
