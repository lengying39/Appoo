using SQLite;
using System.Collections.Generic;

namespace Appoo.Models;

[SQLite.Table("TouristSpots")]
public class TouristSpot
{
    [SQLite.PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; }
    public string ChineseName { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string ImageFile { get; set; }
    public string OpenTime { get; set; }
    public string Location { get; set; }

    [SQLite.Ignore]
    public List<string> NearbyFood { get; set; } = new();

    public bool IsFavorite { get; set; }
}