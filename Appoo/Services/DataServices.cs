using Appoo.Models;

namespace Appoo.Services;

public class DataService : IDataService
{
    private readonly List<User> _users = new();
    public User? CurrentUser { get; private set; }

    public DataService()
    {
        _users.Add(new User { Username = "test", Password = "123", Nickname = "Tester" });
    }

    public Task<bool> LoginAsync(string username, string password)
    {
        var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user != null) { CurrentUser = user; return Task.FromResult(true); }
        return Task.FromResult(false);
    }

    public Task<bool> RegisterAsync(User user)
    {
        if (_users.Any(u => u.Username == user.Username)) return Task.FromResult(false);
        _users.Add(user);
        CurrentUser = user;
        return Task.FromResult(true);
    }

    public void AddFavorite(string spotName) => CurrentUser?.FavoriteSpotNames.Add(spotName);
    public void RemoveFavorite(string spotName) => CurrentUser?.FavoriteSpotNames.Remove(spotName);
    public List<string> GetFavorites() => CurrentUser?.FavoriteSpotNames ?? new();
    public void ClearFavorites() => CurrentUser?.FavoriteSpotNames.Clear();

    public List<TouristSpot> GetAllSpots() => new()
    {
        new TouristSpot
        {
            Name = "大雁塔",
            Description = "唐代古塔，大慈恩寺内。",
            OpenTime = "8:00 - 17:30",
            Location = "雁塔区，西安",
            ImageFile = "dyt.png",          
            NearbyFood = new() { "肉夹馍", "凉皮", "羊肉泡馍" },
            Latitude = 34.2136, Longitude = 108.9594
        },
        new TouristSpot
        {
            Name = "钟楼",
            Description = "西安市中心地标。",
            OpenTime = "8:00 - 22:00",
            Location = "市中心，西安",
            ImageFile = "zl.jpg",           
            NearbyFood = new() { "德发长饺子", "回民街烤肉", "酸梅汤" },
            Latitude = 34.2583, Longitude = 108.9427
        },
        new TouristSpot
        {
            Name = "兵马俑",
            Description = "世界第八大奇迹。",
            OpenTime = "8:00 - 18:00",
            Location = "临潼区，西安",
            ImageFile = "bmy.png",          
            NearbyFood = new() { "临潼石榴", "柿子饼", "梆梆肉" },
            Latitude = 34.3849, Longitude = 109.2731
        },
        new TouristSpot
        {
            Name = "华清宫",
            Description = "唐代皇家温泉行宫。",
            OpenTime = "9:00 - 17:30",
            Location = "临潼区，西安",
            ImageFile = "hqg.jpg",        
            NearbyFood = new() { "温泉蛋", "御膳点心", "石榴汁" },
            Latitude = 34.3812, Longitude = 109.2734
        }
    };

    public List<string> GetAllFoods() => new()
    { "肉夹馍", "凉皮", "羊肉泡馍", "德发长饺子", "回民街烤肉", "酸梅汤", "临潼石榴", "柿子饼", "梆梆肉" };

    public List<string> GetAllFacilities() => new()
    {
        "公共厕所 - 50m - 开放",
        "充电站 - 200m - 可用",
        "无障碍通道 - 100m - 开放",
        "自动售票机 - 300m - 工作中",
        "饮水处 - 150m - 开放"
    };
}