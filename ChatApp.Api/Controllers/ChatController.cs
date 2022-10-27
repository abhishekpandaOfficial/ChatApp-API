using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Api.Data;
using ChatApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatApp.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly ChatAppDbContext _chatAppDbContext;
        public ChatController(ChatAppDbContext chatAppDbContext)
        {
            _chatAppDbContext = chatAppDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
          var employeeLists =  await _chatAppDbContext.Employees.ToListAsync();
            return Ok(employeeLists);
        }


        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {
            employeeRequest.Id = Guid.NewGuid();
            await _chatAppDbContext.Employees.AddAsync(employeeRequest);
            await _chatAppDbContext.SaveChangesAsync();
            return Ok(employeeRequest);
        }
    }
}

