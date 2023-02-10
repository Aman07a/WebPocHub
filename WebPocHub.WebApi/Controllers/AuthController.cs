using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebPocHub.Dal;
using WebPocHub.Models;

namespace WebPocHub.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthenticationRepository _authenticationRepository;

		public AuthController(IAuthenticationRepository authenticationRepository)
		{
			_authenticationRepository = authenticationRepository;
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult Create(User user)
		{
			var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
			user.Password = passwordHash;
			var result = _authenticationRepository.RegisterUser(user);

			if (result > 0)
			{
				return Ok();
			}

			return BadRequest();
		}


		[HttpPost("CheckCredentials")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<AuthResponce> GetDetails(User user)
		{
			var authUser = _authenticationRepository.CheckCredentials(user);

			if (authUser == null)
			{
				return NotFound();
			}

			if(authUser != null && BCrypt.Net.BCrypt.Verify(user.Password, authUser.Password))
			{
				return BadRequest("Incorrect Password! Please check your password!");
			}

			var authResponse = new AuthResponce()
			{
				IsAuthenticated = true,
				Role = "Dummy Role",
				Token = "Dummy Token"
			};

			return Ok(authResponse);
		}
	}
}
