using vTrack.Data.Repositories;
using vTrack.Models;

namespace vTrack.Services;

public class PackageService : IPackageService
{
    private readonly PackageParser _parser;
    private readonly PackageRepository _repository;

    public PackageService(PackageParser parser, PackageRepository repository)
    {
        _parser = parser;
        _repository = repository;
    }
    public async Task StorePackageAsync(string RawData)
    {
        await _repository.AddAsync(_parser.Parser(RawData));
    }

    public async Task<Package> GetLatestPackageAsync()
    {
        //I might add logic here later
        return await _repository.GetLastAsync();
    }
}