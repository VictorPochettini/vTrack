using vTrack.Models;
using System.Text;

namespace vTrack.Services;

public class PackageParser : IPackageParser
{
    public Package Parser(string RawData)
    {
        Package p = new Package(RawData);
        //Implement parsing logic here later once I choose which tracker to use

        // Validate that RawData has even length (each byte = 2 hex characters)
        if (RawData.Length % 2 != 0)
            throw new Exception("Invalid raw data: hex string must have even length");

        byte[] data = new byte[RawData.Length / 2];

        // Loop through pairs of hex characters
        for(int i = 0; i < RawData.Length; i += 2)
        {
            data[i / 2] = Convert.ToByte(RawData.Substring(i, 2), 16);
        }

        // Validate minimum data length for header check
        if (data.Length < 2)
            throw new Exception($"Invalid packet: data too short ({data.Length} bytes). Expected at least 2 bytes for header. Received: {RawData}");

        int packageLength = 0;
        //PackageLengthLength
        int pLL = 0;
        int index = 0;

        if(data[0] == 0x78 && data[1] == 0x78)
            //1 byte
            pLL = 1;
        else if(data[0] == 0x79 && data[1] == 0x79)
            //2 bytes
            pLL = 2;
        else
            throw new Exception($"Invalid packet header: expected 0x78 0x78 or 0x79 0x79, got {data[0]:X2} {data[1]:X2}. Full data: {RawData}");
        
        index = 2;

        if(pLL == 1)
        {
            packageLength = data[index];
            index = 3;
        }
        else if(pLL == 2)
        {
            packageLength = (((ushort)data[index]) << 8) | data[index + 1];
            index = 4;
        }

        //Just using Positioning Data now
        if(data[index++] != 0x22)
            return p;

        int Year = 2000 + data[index++];
        int Month = data[index++];
        int Day = data[index++];
        int Hour = data[index++];
        int Minute = data[index++];
        int Second = data[index++];

        byte satInfo = data[index++];

        int gpsInfoLength = satInfo >> 4;
        int satelliteCount = satInfo & 0x0F;

        int LatByte = (data[index] << 24) | (data[index + 1] << 16) | (data[index + 2] << 8) | data[index + 3];
        int LonByte = (data[index + 4] << 24) | (data[index + 5] << 16) | (data[index + 6] << 8) | data[index + 7];

        double Latitude = LatByte / 1800000.0;
        index += 4;
        double Longitude = LonByte / 1800000.0;
        index += 4;

        int Speed = data[index++];

        p.Timestamp = new DateTime(Year, Month, Day, Hour, Minute, Second, DateTimeKind.Utc);
        p.Lat = Latitude;
        p.Lon = Longitude;

        //Stop byte is of value 0x0D0A

        return p;
    }
}