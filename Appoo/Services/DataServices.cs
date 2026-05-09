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
            Name = "Dayan Pagoda",
            Description = "A famous Buddhist pagoda in Xi'an.",
            OpenTime = "8:00 - 17:30",
            Location = "Yanta District, Xi'an",
            ImageFile = "spot_dayanta.jpg",
            NearbyFood = new() { "Liangpi", "Yangroupaomo" },
            Latitude = 34.2136, Longitude = 108.9594
        },
        new TouristSpot
        {
            Name = "The Xi'an Circumvallation",
            Description = "The most complete ancient city wall in China.",
            OpenTime = "8:00 - 22:00",
            Location = "Downtown Xi'an",
            ImageFile = "spot_zhonglou.jpg",
            NearbyFood = new() { "Defachang Dumplings", "Muslim Street BBQ" },
            Latitude = 34.2583, Longitude = 108.9427
        },
        new TouristSpot
        {
            Name = "Emperor Qinshihuang's Mausoleum",
            Description = "The Terracotta Army is a collection of terracottas.",
            OpenTime = "8:00 - 18:00",
            Location = "Lintong District, Xi'an",
            ImageFile = "spot_bingmayong.jpg",
            NearbyFood = new() { "Biängbiäng Noodles", "Roujiamo" },
            Latitude = 34.3849, Longitude = 109.2731
        },
        new TouristSpot
        {
            Name = "Huaqing Palace",
            Description = "A royal hot spring palace from Tang Dynasty.",
            OpenTime = "9:00 - 17:30",
            Location = "Lintong District, Xi'an",
            ImageFile = "spot_huaqinggong.jpg",
            NearbyFood = new() { "Hot spring eggs", "Imperial snacks" },
            Latitude = 34.3812, Longitude = 109.2734
        }
    };

    public List<string> GetAllFoods() => new()
    { "Biängbiäng Noodles", "Roujiamo", "Liangpi", "Yangroupaomo", "Defachang Dumplings" };

    public List<string> GetAllFacilities() => new()
    {
        "Public Toilet - 50m - Open",
        "Charging Station - 200m - Available",
        "Accessible Passage - 100m - Open",
        "Ticket Vending Machine - 300m - Working",
        "Water Fountain - 150m - Open"
    };
}