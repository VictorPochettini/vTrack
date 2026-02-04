namespace vTrack.Services;
using vTrack.Models;

interface IPackageService
{
    Task StorePackageAsync(string RawData);
    Task<Package> GetLatestPackageAsync();
}