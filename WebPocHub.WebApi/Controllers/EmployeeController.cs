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

		[HttpGet]
		public IEnumerable<Employee> Get()
		{
			return _commonRepository.GetAll();
		}

		[HttpGet("{id:int}")]
		public Employee GetDetails(int id)
		{
			return _commonRepository.GetDetails(id);
		}
	}
}
