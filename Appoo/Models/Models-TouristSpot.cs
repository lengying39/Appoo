namespace Appoo.Models
{
    public class TouristSpot
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ImageFile { get; set; }
        public List<string> NearbyFood { get; set; }
        public string OpenTime { get; set; }      // 新增
        public string Location { get; set; }      // 新增
        public bool IsFavorite { get; set; }      // 本地收藏状态（运行时）
    }
}