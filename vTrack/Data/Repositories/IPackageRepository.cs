using vTrack.Models;

namespace vTrack.Data.Repositories;

interface IPackageRepository
{
    Task<Package> AddAsync(Package package);
    Task<Package> GetLastAsync();
    Task<IEnumerable<Package>> GetAllAsync();
}