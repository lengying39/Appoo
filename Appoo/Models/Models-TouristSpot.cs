namespace Appoo.Models;

public class TouristSpot
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string ImageFile { get; set; }
    public string OpenTime { get; set; }
    public string Location { get; set; }
    public List<string> NearbyFood { get; set; } = new();
    public bool IsFavorite { get; set; }
}