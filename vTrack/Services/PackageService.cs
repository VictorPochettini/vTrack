using vTrack.Data.Repositories;
using vTrack.Models;

namespace vTrack.Services;

class PackageService : IPackageService
{
    private readonly PackageParser _parser = new PackageParser();
    private readonly PackageRepository _repository = new PackageRepository();

    public async Task StorePackageAsync(string RawData)
    {
        await _repository.AddAsync(_parser.Parser(RawData));
    }

    public async Task<Package> GetLatestPackageAsync()
    {
        return await _repository.GetLastAsync();
    }
}