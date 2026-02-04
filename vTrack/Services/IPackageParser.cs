using vTrack.Models;

namespace vTrack.Services;

interface IPackageParser
{
    Package Parser(string RawData);
}