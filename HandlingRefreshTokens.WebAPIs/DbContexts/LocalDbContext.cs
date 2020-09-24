using System;
using Microsoft.EntityFrameworkCore;
using HandlingRefreshTokens.WebAPIs.DbContexts.Entities;

namespace HandlingRefreshTokens.WebAPIs.DbContexts
{
    public class LocalDbContext : DbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Person> People { get; set; }

        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }
    }
}
