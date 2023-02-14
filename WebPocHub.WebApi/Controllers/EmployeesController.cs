using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebPocHub.Dal;
using WebPocHub.Models;
using WebPocHub.WebApi.DTO;

namespace WebPocHub.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[EnableCors("PublicPolicy")]
	public class EmployeesController : ControllerBase
	{
		private readonly ICommonRepository<Employee> _employeeRepository;
		private readonly IMapper _mapper;

		public EmployeesController(ICommonRepository<Employee> employeeRepository, IMapper mapper)
		{
			_employeeRepository = employeeRepository;
			_mapper = mapper;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Roles = "Employee,Hr")]
		public ActionResult<IEnumerable<Employee>> Get()
		{
			var employees = _employeeRepository.GetAll();

			if (employees.Count <= 0)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<IEnumerable<EmployeeDTO>>(employees));
		}

		[HttpGet("{id:int}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Roles = "Employee,Hr")]
		public ActionResult<Employee> GetDetails(int id)
		{
			var employee = _employeeRepository.GetDetails(id);
			return employee == null ? NotFound() : Ok(_mapper.Map<EmployeeDTO>(employee));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "Employee,Hr")]
		public ActionResult Create(Employee employee)
		{
			var employeeModel = _mapper.Map<Employee>(employee);

			_employeeRepository.Insert(employeeModel);

			var result = _employeeRepository.SaveChanges();

			if (result > 0)
			{
				// actionName - The name of the action to use for generating the URL
				// routeValues - The route data to use for generating the URL
				// value - The content value to format in the entity body
				var employeeDetails = _mapper.Map<EmployeeDTO>(employeeModel);
				return CreatedAtAction("GetDetails", new { id = employeeDetails.EmployeeId }, employeeDetails);
			}

			return BadRequest();
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "Employee,Hr")]
		public ActionResult Update(Employee employee)
		{
			var employeeModel = _mapper.Map<Employee>(employee);

			_employeeRepository.Update(employeeModel);

			var result = _employeeRepository.SaveChanges();

			if (result > 0)
			{
				return NoContent();
			}

			return BadRequest();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[Authorize(Roles = "Hr")]
		public ActionResult<Employee> Delete(int id)
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
