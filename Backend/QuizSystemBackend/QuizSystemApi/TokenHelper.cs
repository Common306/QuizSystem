using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using QuizSystemApi.Models;
using QuizSystemApi.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizSystemApi
{
    public class TokenHelper
    {

        public static User GetUserFromToken(HttpContext context)
        {
            string token = context.Request.Headers["Authorization"].ToString().Substring("Bearer ".Length);


            var _config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Issuer"]
            };

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                string userIdClaim = claimsPrincipal.FindFirst("UserId").Value;
                string userUsernameClaim = claimsPrincipal.FindFirst("Username").Value;
                string userRoleIdClaim = claimsPrincipal.FindFirst("RoleId").Value;
                string userRoleClaim = claimsPrincipal.FindFirst(ClaimTypes.Role).Value;
                string userFullNameClaim = claimsPrincipal.FindFirst("Fullname").Value;
                string userPhoneNumberClaim = claimsPrincipal.FindFirst("PhoneNumber").Value;
                string? userCreateAtClaim = claimsPrincipal.FindFirst("CreateAt").Value;
                string userUpdateAtClaim = claimsPrincipal.FindFirst("UpdateAt").Value;

                if (userIdClaim != null)
                {
                    User user = new User
                    {
                        UserId = Convert.ToInt32(userIdClaim),
                        Username = userUsernameClaim,
                        Password = "",
                        RoleId = Convert.ToInt32(userRoleIdClaim),
                        FullName = userFullNameClaim,
                        PhoneNumber = userPhoneNumberClaim,
                        CreateAt = userCreateAtClaim != "" ? Convert.ToDateTime(userCreateAtClaim) : null,
                        UpdateAt = userUpdateAtClaim != "" ? Convert.ToDateTime(userUpdateAtClaim) : null,
                        IsEnable = true
                    };
                    Role role = new Role
                    {
                        RoleId = Convert.ToInt32(userRoleIdClaim),
                        RoleName = userRoleClaim
                    };
                    user.Role = role;
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
