using Microsoft.EntityFrameworkCore;

namespace Kvali.AuditTrail.Tests;

public class TestDbContext(DbContextOptions options) : AuditableDbContext(options)
{
    public DbSet<Entities> Products { get; set; }
    public DbSet<User> Users { get; set; }
}