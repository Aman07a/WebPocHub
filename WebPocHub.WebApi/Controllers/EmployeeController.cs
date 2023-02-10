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
		private readonly ICommonRepository<Employee> _commonRepository;

		public EmployeeController(ICommonRepository<Employee> commonRepository)
		{
			_commonRepository = commonRepository;
		}

		// [HttpGet]
		// [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Employee>))]
		// [ProducesResponseType(StatusCodes.Status404NotFound)]
		// public IActionResult Get()
		// {
		// 	var employees = _commonRepository.GetAll();

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
		// 	var employee = _commonRepository.GetDetails(id);
		// 	return employee == null ? NotFound() : Ok(employee);
		// }

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<Employee>> Get()
		{
			var employees = _commonRepository.GetAll();

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
			var employee = _commonRepository.GetDetails(id);
			return employee == null ? NotFound() : Ok(employee);
		}
	}
}
