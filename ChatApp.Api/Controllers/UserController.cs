using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatApp.Api.Models;
using ChatApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly ChatAppDbContext _chatAppDbContext;
        public UserController(ChatAppDbContext chatAppDbContext)
        {
            _chatAppDbContext = chatAppDbContext;
        }
        

        [HttpPost("Registration")]
        
        public async Task<IActionResult> Registration([FromBody] RegisterUser registerUsers)
        {
            var users = new RegisterUser()
            {
                FullName = registerUsers.FullName,
                UserName = registerUsers.UserName,
                Email = registerUsers.Email,
                Mobile = registerUsers.Mobile,
                Password = registerUsers.Password,
                ConfirmPassword = registerUsers.ConfirmPassword,
               

            };
            await _chatAppDbContext.RegisteredUsers.AddAsync(users);
            await _chatAppDbContext.SaveChangesAsync();
            return Ok(registerUsers);
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var _currentUser = await _chatAppDbContext.RegisteredUsers.FirstOrDefaultAsync(a=>a.UserName == userName && a.Password == password);
            if (_currentUser != null)
            {
                return Ok(_currentUser);
            }

                return NotFound();
        }
    }
}