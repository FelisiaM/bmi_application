using System.IdentityModel.Tokens.Jwt;
using BMI.Models;

namespace BMI.Authorisation
{
    public interface ITokenHandler
    {
        JwtSecurityToken GetJwtSecurityToken(string tokenId);
        bool IsAuthorised(JwtSecurityToken jwtToken);
        UserDetails GetUserDetailsFromClaims(JwtSecurityToken jwtToken);
        string GetUserName(JwtSecurityToken jwtToken);
    }
}