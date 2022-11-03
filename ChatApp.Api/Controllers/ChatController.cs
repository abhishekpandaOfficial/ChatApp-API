using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.Api.Data;
using ChatApp.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChatApp.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ChatAppDbContext _chatAppDbContext;
        public ChatController(ChatAppDbContext chatAppDbContext)
        {
            _chatAppDbContext = chatAppDbContext;
        }

        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
          var employeeLists =  await _chatAppDbContext.Employees.ToListAsync();
            
            return Ok(employeeLists);
        }


        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {
            employeeRequest.Id = Guid.NewGuid();
            employeeRequest.isActive = "true";
            await _chatAppDbContext.Employees.AddAsync(employeeRequest);
            await _chatAppDbContext.SaveChangesAsync();
            return Ok(employeeRequest);
        }

        [HttpGet("GetEmployeeById/{id:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute]Guid id)
        {
           var employee= await _chatAppDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if(employee == null)
            {
                return NotFound();
            }
            return Ok(employee);


        }

        [HttpPut("UpdateEmployee/{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute]Guid id, Employee updateEmployee)
        {
           var employee = await _chatAppDbContext.Employees.FindAsync(id);
           // var activeCheck = await _chatAppDbContext.Employees.FindAsync(employee?.isActive);

            if(employee == null)
            {
                return NotFound();
            }
            employee.Name = updateEmployee.Name;
            employee.Email = updateEmployee.Email;
            employee.Salary = updateEmployee.Salary;
            employee.Phone = updateEmployee.Phone;
            employee.Department = updateEmployee.Department;

            await _chatAppDbContext.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpDelete("DeleteEmployee/{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await _chatAppDbContext.Employees.FindAsync(id);
            if(employee == null)
            {
                return NotFound();
            }
            employee.isActive = "false";
             _chatAppDbContext.Remove(employee);
            await _chatAppDbContext.SaveChangesAsync();
            return Ok(employee);
        }
    }
}

