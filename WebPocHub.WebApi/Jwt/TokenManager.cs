using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebPocHub.Models;

namespace WebPocHub.WebApi.Jwt
{
	public class TokenManager : ITokenManager
	{
		private readonly SymmetricSecurityKey _key;

		public TokenManager(IConfiguration configuration)
		{
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["WebPocHubJWT:Secret"]!));
		}

		public string GenerateToken(User user, string roleName)
		{
			throw new NotImplementedException();
		}
	}
}
