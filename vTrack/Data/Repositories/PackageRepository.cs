using Microsoft.EntityFrameworkCore;
using vTrack.Models;

namespace vTrack.Data.Repositories;

class PackageRepository : IPackageRepository
{
    private ApplicationDbContext _db;
    public async Task<Package> AddAsync(Package package)
    {
        _db.Packages.Add(package);
        await _db.SaveChangesAsync();
        return package;
    }

    public async Task<Package> GetLastAsync()
    {
        return await _db.Packages.OrderByDescending(p => p.Timestamp).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Package>> GetAllAsync()
    {
        return await _db.Packages.OrderByDescending(p => p.Timestamp).ToListAsync();
    }
}