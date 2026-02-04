using vTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace vTrack.Data;

class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : base(options)
    {
    }
    
    public DbSet<Package> Packages {get; set;}
}