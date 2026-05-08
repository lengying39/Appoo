namespace Appoo.Models;

public class TouristSpot
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string ImageFile { get; set; }  // 新的图片文件名（如 "spot_dayanta.jpg"）
}