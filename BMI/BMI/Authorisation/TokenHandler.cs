using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using BMI.Models;

namespace BMI.Authorisation
{
    public class TokenHandler : ITokenHandler
    {
        public JwtSecurityToken GetJwtSecurityToken(string tokenId)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadToken(tokenId) as JwtSecurityToken;
        }

        public bool IsAuthorised(JwtSecurityToken jwtToken)
        {
            var access = GetClaim(jwtToken, "extension_Access");

            return access.Equals("True");
        }

        public UserDetails GetUserDetailsFromClaims(JwtSecurityToken jwtToken)
        {
            return new UserDetails
            {
                Height = double.Parse(GetClaim(jwtToken, "extension_Height")),
                Weight = double.Parse(GetClaim(jwtToken, "extension_Weight"))
            };
        }

        public string GetUserName(JwtSecurityToken jwtToken)
        {
           return GetClaim(jwtToken, "family_name");
        }

        private static string GetClaim(
            JwtSecurityToken jwtToken,
            string claimType)
        {
            return jwtToken.Claims.First(claim => claim.Type == claimType).Value;
        }
    }
}
