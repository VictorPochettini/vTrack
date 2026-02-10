using vTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace vTrack.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : base(options)
    {
    }
    
    public DbSet<Package> Packages {get; set;}
}