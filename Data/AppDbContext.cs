using Microsoft.EntityFrameworkCore;
using JobCardBackend.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<JobCard> JobCards { get; set; }
    public DbSet<User> Users { get; set; }
}
