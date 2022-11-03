using ChatApp.Api.Data;
using ChatApp.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtTokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ChatAppDbContext _chatAppDbContext;

        public JwtTokenController(IConfiguration configuration, ChatAppDbContext chatAppDbContext)
        {
            _configuration = configuration;
            _chatAppDbContext = chatAppDbContext;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(string userName, string password)
        {
            var data = await _chatAppDbContext.user.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
          

            if (user != null && user.UserName != null && user.Password != null)
            {
                var userData = await GetUser(user.UserName, user.Password);
                var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.UserId.ToString()),
                        new Claim("UserName", user.UserName),
                        new Claim("Password",user.Password)

                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        jwt.Issuer,
                        jwt.Audience,
                        claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signIn
                        );
                    var userAdd = new User()
                    {
                        UserName = user.UserName,
                        Password = user.Password,
                    };
                    await _chatAppDbContext.user.AddAsync(userAdd);
                    await _chatAppDbContext.SaveChangesAsync();
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }

            }
            else
            {
                return BadRequest("Invalid Credentials ! Please Try Again !!!");
            }

            return Ok();
        }


    }
}
