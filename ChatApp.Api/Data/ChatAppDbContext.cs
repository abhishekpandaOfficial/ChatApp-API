using System;
using ChatApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Data
{
    public class ChatAppDbContext: DbContext
    {
        public ChatAppDbContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<RegisterUser> RegisteredUsers { get; set; }
        public DbSet<User> user { get; set; }

    }
}

