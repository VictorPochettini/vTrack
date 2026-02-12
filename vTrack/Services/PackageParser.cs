using System.Security.Cryptography.X509Certificates;
using vTrack.Models;

namespace vTrack.Services;

public class PackageParser : IPackageParser
{
    public Package Parser(string RawData)
    {
        Package p = new Package(RawData);
        //Implement parsing logic here later once I choose which tracker to use
        return p;
    }
}