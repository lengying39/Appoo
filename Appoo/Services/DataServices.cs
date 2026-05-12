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

    public void Logout()
    {
        CurrentUser = null;
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

    // ---------- 景点数据 ----------
    public List<TouristSpot> GetAllSpots() => new()
    {
        new TouristSpot
        {
            Name = "Giant Wild Goose Pagoda",
            ChineseName = "大雁塔",
            Description = "Tang Dynasty ancient pagoda, inside the Dacien Temple.",
            OpenTime = "8:00 - 17:30",
            Location = "Yanta District，Xi'an",
            ImageFile = "dyt.png",
            NearbyFood = new() { "Rougamo", "LiangPi", "Pita bread soaked in Lamb Soup" },
            Latitude = 34.2136, Longitude = 108.9594
        },
        new TouristSpot
        {
            Name = "Bell Tower",
            ChineseName = "钟楼",
            Description = "Xi'an city center landmark.",
            OpenTime = "8:30 - 21:30",
            Location = "City center，Xi'an",
            ImageFile = "zl.jpg",
            NearbyFood = new() { "De Fa Long Dumplings", "Hui Min Street Barbecue", "Sour Plum Drink" },
            Latitude = 34.2583, Longitude = 108.9427
        },
        new TouristSpot
        {
            Name = "Terra Cotta Warriors",
            ChineseName = "兵马俑",
            Description = "The eighth wonder of the world.",
            OpenTime = "8:00 - 18:00",
            Location = "Lintong District，Xi'an",
            ImageFile = "bmy.jpg",
            NearbyFood = new() { "Lintong Pomegranate", "Dried Persimmon", "BangBangRou" },
            Latitude = 34.3849, Longitude = 109.2731
        },
        new TouristSpot
        {
            Name = "Huaqing Palace",
            ChineseName = "华清宫",
            Description = "The Tang Dynasty Royal Hot Spring Palace.",
            OpenTime = "9:00 - 17:30",
            Location = "Lintong District，Xi'an",
            ImageFile = "hqg.jpg",
            NearbyFood = new() { "Chinese Onsen Tamago", "Royal Dim sum", "Pomegranate Juice" },
            Latitude = 34.3812, Longitude = 109.2734
        },
        new TouristSpot
        {
            Name = "Shaanxi History Museum",
            ChineseName = "陕西历史博物馆",
            Description = "One of China's most significant comprehensive museums, featuring over 370,000 historical relics.",
            OpenTime = "9:00 - 17:30 (Tue-Sun, closed Mon)",
            Location = "Yanta District, Xi'an",
            ImageFile = "shanbo.jpg",
            NearbyFood = new() { "Liangpi", "Roujiamo" },
            Latitude = 34.2152, Longitude = 108.9492
        },
        new TouristSpot
        {
            Name = "Tang Paradise (Datang Furong Garden)",
            ChineseName = "大唐芙蓉园",
            Description = "A large-scale Tang Dynasty style cultural theme park showcasing imperial garden architecture.",
            OpenTime = "9:00 - 21:00",
            Location = "Yanta District, Xi'an",
            ImageFile = "dtfry.jpg",
            NearbyFood = new() { "Tang-style pastries", "Mutton and Bread Soup" },
            Latitude = 34.2144, Longitude = 108.9771
        },
    };

    // ---------- 美食与设施 ----------
    public List<string> GetAllFoods() => new()
    {
        "Rougamo",
        "Liangpi",
        "Pita Bread Soaked in Lamb Soup",
        "De Fa Long Dumplings",
        "Hui Min Street Barbecue",
        "Sour Plum Drink",
        "Lintong Pomegranate",
        "Dried Persimmon",
        "Bang Bang Rou",
        "Chinese Onsen Tamago",
        "Royal Dim Sum",
        "Pomegranate Juice",
        "Tang-style Pastries",
        "Mutton and Bread Soup"
    };

    public List<string> GetAllFacilities() => new()
    {
        "Public Toilet - 50m - Open",
        "Charging Station - 200m - Available",
        "Accessible Passage - 100m - Open",
        "Ticket Vending Machine - 300m - Working",
        "Water Fountain - 150m - Open"
    };

    // ---------- 云端识别 → 本地景点匹配 ----------
    public TouristSpot? RecognizeAndMatchSpot(string? landmarkName)
    {
        if (string.IsNullOrWhiteSpace(landmarkName))
            return null;

        // 去除百度返回结果中括号里的内容，如“大雁塔(大慈恩寺)” → “大雁塔”
        string cleanName = landmarkName.Split('(')[0].Trim();

        var allSpots = GetAllSpots();

        // 1. 精确匹配中文名
        var chineseMatch = allSpots.FirstOrDefault(s =>
            !string.IsNullOrEmpty(s.ChineseName) &&
            s.ChineseName.Equals(cleanName, StringComparison.OrdinalIgnoreCase));
        if (chineseMatch != null) return chineseMatch;

        // 2. 精确匹配英文名
        var englishMatch = allSpots.FirstOrDefault(s =>
            s.Name.Equals(cleanName, StringComparison.OrdinalIgnoreCase));
        if (englishMatch != null) return englishMatch;

        // 3. 模糊匹配：百度返回的名词包含本地中文名
        var fuzzyChinese = allSpots.FirstOrDefault(s =>
            !string.IsNullOrEmpty(s.ChineseName) &&
            cleanName.Contains(s.ChineseName, StringComparison.OrdinalIgnoreCase));
        if (fuzzyChinese != null) return fuzzyChinese;

        // 4. 模糊匹配：百度返回的名词包含本地英文名
        var fuzzyEnglish = allSpots.FirstOrDefault(s =>
            cleanName.Contains(s.Name, StringComparison.OrdinalIgnoreCase));
        if (fuzzyEnglish != null) return fuzzyEnglish;

        return null;
    }
}