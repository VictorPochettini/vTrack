namespace vTrack.Models;

class Package
{
    public int ID {get; set;}
    public double Lat {get; set;}
    public double Lon {get; set;}
    public DateTimeOffset Timestamp {get; set;}
    public string RawData {get; set;}

}