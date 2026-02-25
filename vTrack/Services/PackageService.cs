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
        Package p = _parser.Parser(RawData);
        Console.WriteLine(p.Timestamp);
        await _repository.AddAsync(p);
    }

    public async Task<Package> GetLatestPackageAsync()
    {
        //I might add logic here later
        return await _repository.GetLastAsync();
    }
}