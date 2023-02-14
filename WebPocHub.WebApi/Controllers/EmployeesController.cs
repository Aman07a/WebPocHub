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
		public async Task<ActionResult<IEnumerable<EmployeeDTO>>> Get()
		{
			var employees = await _employeeRepository.GetAll();

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
		public async Task<ActionResult<EmployeeDTO>> GetDetails(int id)
		{
			var employee = await _employeeRepository.GetDetails(id);
			return employee == null ? NotFound() : Ok(_mapper.Map<EmployeeDTO>(employee));
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "Employee,Hr")]
		public async Task<ActionResult> Create(NewEmployeeDTO? employee)
		{
			var employeeModel = _mapper.Map<Employee>(employee);

			var result = await _employeeRepository.Insert(employeeModel);

			if (result != null)
			{
				var employeeDetails = _mapper.Map<EmployeeDTO>(employeeModel);
				return CreatedAtAction("GetDetails", new { id = employeeDetails.EmployeeId }, employeeDetails);
			}

			return BadRequest();
		}

		[HttpPut]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[Authorize(Roles = "Employee,Hr")]
		public async Task<ActionResult> Update(UpdateEmployeeDTO employee)
		{
			var employeeModel = _mapper.Map<Employee>(employee);

			var result = await _employeeRepository.Update(employeeModel);

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
			var employee = await _employeeRepository.GetDetails(id);

			if (employee == null)
			{
				return NotFound();
			}
			else
			{
				await _employeeRepository.Delete(employee.EmployeeId);
				return NoContent();
			}
		}
	}
}
