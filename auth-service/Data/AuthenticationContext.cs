using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace auth_service.Data
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new User
                {
                    UserId = 1,
                    UserName = "mikebikename",
                    Password = "password",
                    CreatedDate = DateTime.UtcNow
                }
            });
        }
    }
}