using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebPocHub.Dal;
using WebPocHub.Models;

namespace WebPocHub.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : ControllerBase
	{
		private readonly ICommonRepository<Employee> _employeeRepository;

		public EmployeeController(ICommonRepository<Employee> employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		// [HttpGet]
		// [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Employee>))]
		// [ProducesResponseType(StatusCodes.Status404NotFound)]
		// public IActionResult Get()
		// {
		// 	var employees = _employeeRepository.GetAll();

		// 	if (employees.Count == 0)
		// 	{
		// 		return NotFound();
		// 	}

		// 	return Ok(employees);
		// }

		// [HttpGet("{id:int}")]
		// [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
		// [ProducesResponseType(StatusCodes.Status404NotFound)]
		// public IActionResult GetDetails(int id)
		// {
		// 	var employee = _employeeRepository.GetDetails(id);
		// 	return employee == null ? NotFound() : Ok(employee);
		// }

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<Employee>> Get()
		{
			var employees = _employeeRepository.GetAll();

			if (employees.Count == 0)
			{
				return NotFound();
			}

			return Ok(employees);
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<Employee> GetDetails(int id)
		{
			var employee = _employeeRepository.GetDetails(id);
			return employee == null ? NotFound() : Ok(employee);
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult Create(Employee employee)
		{
			_employeeRepository.Insert(employee);

			var result = _employeeRepository.SaveChanges();

			if (result > 0)
			{
				// actionName - The name of the action to use for generating the URL
				// routeValues - The route data to use for generating the URL
				// value - The content value to format in the entity body
				return CreatedAtAction("GetDetails", new { id = employee.EmployeeId }, employee);
			}

			return BadRequest();
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult Update(Employee employee)
		{
			_employeeRepository.Update(employee);

			var result = _employeeRepository.SaveChanges();

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
			var employee = _employeeRepository.GetDetails(id);

			if (employee == null)
			{
				return NotFound();
			}
			else
			{
				_employeeRepository.Delete(employee);
				_employeeRepository.SaveChanges();
				return NoContent();
			}
		}
	}
}
