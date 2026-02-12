namespace vTrack.Models;

public class Package
{
    public int ID {get; set;}
    public double Lat {get; set;}
    public double Lon {get; set;}
    public DateTime Timestamp {get; set;}
    public string RawData {get; set;} = string.Empty;

    public Package() {}
    public Package(string RawData)
    {
        this.RawData = RawData;
    }
    public Package(int ID, double Lat, double Lon, DateTime Timestamp, string RawData)
    {
        this.ID = ID;
        this.Lat = Lat;
        this.Lon = Lon;
        this.Timestamp = Timestamp;
        this.RawData = RawData;
    }
}