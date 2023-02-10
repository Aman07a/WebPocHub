using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebPocHub.Dal;
using WebPocHub.Models;

namespace WebPocHub.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EventsController : ControllerBase
	{
		private readonly ICommonRepository<Event> _eventRepository;

		public EventsController(ICommonRepository<Event> eventRepository)
		{
			_eventRepository = eventRepository;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<Event>> Get()
		{
			var events = _eventRepository.GetAll();

			if (events.Count == 0)
			{
				return NotFound();
			}

			return Ok(events);
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<Event> GetDetails(int id)
		{
			var events = _eventRepository.GetDetails(id);
			return events == null ? NotFound() : Ok(events);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult Create(Event events)
		{
			_eventRepository.Insert(events);

			var result = _eventRepository.SaveChanges();

			if (result > 0)
			{
				// actionName - The name of the action to use for generating the URL
				// routeValues - The route data to use for generating the URL
				// value - The content value to format in the entity body
				return CreatedAtAction("GetDetails", new { id = events.EventId }, events);
			}

			return BadRequest();
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult Update(Event events)
		{
			_eventRepository.Update(events);

			var result = _eventRepository.SaveChanges();

			if (result > 0)
			{
				return NoContent();
			}

			return BadRequest();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult Delete(int id)
		{
			var employee = _eventRepository.GetDetails(id);

			if (employee == null)
			{
				return NotFound();
			}
			else
			{
				_eventRepository.Delete(employee);
				_eventRepository.SaveChanges();
				return NoContent();
			}
		}
	}
}
