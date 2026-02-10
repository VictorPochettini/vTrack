namespace vTrack.Models.ViewModels;

public class IndexViewModel
{
    public Package ReceivedPackage {get; set;}
    public IndexViewModel(Package package)
    {
        ReceivedPackage = package;
    }
}