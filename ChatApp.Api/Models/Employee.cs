using System;
using System.ComponentModel.DataAnnotations;

namespace ChatApp.Api.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public long Phone { get; set; }
        [Required]
        public long Salary { get; set; }
        [Required]
        public string? Department { get; set; }

        public string? isActive { get; set; }
    }
}

