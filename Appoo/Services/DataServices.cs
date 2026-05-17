using Appoo.Models;
using Appoo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appoo.Services;

public class DataService : IDataService
{
    private readonly DatabaseService _dbService;
    public User? CurrentUser { get; private set; }

    public DataService(DatabaseService dbService)
    {
        _dbService = dbService;
        _ = EnsureTestUserAsync();
    }

    private async Task EnsureTestUserAsync()
    {
        var existing = await _dbService.GetUserByUsernameAsync("test");
        if (existing == null)
        {
            await _dbService.InsertUserAsync(new User
            {
                Username = "test",
                Password = "123",
                Nickname = "Tester",
                FavoriteSpotNames = new List<string>()
            });
        }
    }

    // ---------- 统一收藏存储（前缀区分）----------
    private string PrefixSpot(string name) => $"Spot:{name}";
    private string PrefixFood(string name) => $"Food:{name}";
    private bool IsSpotItem(string item) => item.StartsWith("Spot:");
    private bool IsFoodItem(string item) => item.StartsWith("Food:");
    private string Unwrap(string item) => item.Substring(item.IndexOf(':') + 1);

    private async Task SaveFavoritesAsync(List<string> list)
    {
        if (CurrentUser == null) return;
        CurrentUser.FavoriteSpotNames = list;
        await _dbService.UpdateUserAsync(CurrentUser);
    }

    // 景点收藏
    public async void AddFavoriteSpot(string spotName)
    {
        if (CurrentUser == null) return;
        var list = CurrentUser.FavoriteSpotNames;
        var prefixed = PrefixSpot(spotName);
        if (!list.Contains(prefixed))
        {
            list.Add(prefixed);
            await SaveFavoritesAsync(list);
        }
    }

    public async void RemoveFavoriteSpot(string spotName)
    {
        if (CurrentUser == null) return;
        var list = CurrentUser.FavoriteSpotNames;
        var prefixed = PrefixSpot(spotName);
        if (list.Contains(prefixed))
        {
            list.Remove(prefixed);
            await SaveFavoritesAsync(list);
        }
    }

    public List<string> GetFavoriteSpots()
    {
        return CurrentUser?.FavoriteSpotNames.Where(IsSpotItem).Select(Unwrap).ToList() ?? new List<string>();
    }

    // 美食收藏
    public async void AddFavoriteFood(string foodName)
    {
        if (CurrentUser == null) return;
        var list = CurrentUser.FavoriteSpotNames;
        var prefixed = PrefixFood(foodName);
        if (!list.Contains(prefixed))
        {
            list.Add(prefixed);
            await SaveFavoritesAsync(list);
        }
    }

    public async void RemoveFavoriteFood(string foodName)
    {
        if (CurrentUser == null) return;
        var list = CurrentUser.FavoriteSpotNames;
        var prefixed = PrefixFood(foodName);
        if (list.Contains(prefixed))
        {
            list.Remove(prefixed);
            await SaveFavoritesAsync(list);
        }
    }

    // 获取所有收藏（包括景点和美食）
    public List<string> GetAllFavorites()
    {
        return CurrentUser?.FavoriteSpotNames ?? new List<string>();
    }

    // 兼容旧方法（仅景点）
    public void AddFavorite(string spotName) => AddFavoriteSpot(spotName);
    public void RemoveFavorite(string spotName) => RemoveFavoriteSpot(spotName);
    public List<string> GetFavorites() => GetFavoriteSpots();
    public async void ClearFavorites()
    {
        if (CurrentUser == null) return;
        CurrentUser.FavoriteSpotNames = new List<string>();
        await SaveFavoritesAsync(CurrentUser.FavoriteSpotNames);
    }

    // ---------- 用户登录/注册/登出 ----------
    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _dbService.GetUserByCredentialsAsync(username, password);
        if (user != null)
        {
            CurrentUser = user;
            // 确保 FavoriteSpotNames 不为 null
            if (CurrentUser.FavoriteSpotNames == null)
                CurrentUser.FavoriteSpotNames = new List<string>();
            return true;
        }
        return false;
    }

    public async Task<bool> RegisterAsync(User user)
    {
        var existing = await _dbService.GetUserByUsernameAsync(user.Username!);
        if (existing != null) return false;
        user.FavoriteSpotNames ??= new List<string>();
        var inserted = await _dbService.InsertUserAsync(user);
        if (inserted > 0)
        {
            CurrentUser = user;
            return true;
        }
        return false;
    }

    public void Logout() => CurrentUser = null;
    public Task<User?> GetCurrentUserAsync() => Task.FromResult(CurrentUser);

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

    public List<string> GetAllFoods() => new()
    {
        "Rougamo", "Liangpi", "Pita Bread Soaked in Lamb Soup", "De Fa Long Dumplings",
        "Hui Min Street Barbecue", "Sour Plum Drink", "Lintong Pomegranate", "Dried Persimmon",
        "Bang Bang Rou", "Chinese Onsen Tamago", "Royal Dim Sum", "Pomegranate Juice",
        "Tang-style Pastries", "Mutton and Bread Soup"
    };

    public List<string> GetAllFacilities() => new()
    {
        "Public Toilet - 50m - Open", "Charging Station - 200m - Available",
        "Accessible Passage - 100m - Open", "Ticket Vending Machine - 300m - Working",
        "Water Fountain - 150m - Open"
    };

    public TouristSpot? RecognizeAndMatchSpot(string? landmarkName)
    {
        if (string.IsNullOrWhiteSpace(landmarkName))
            return null;

        // 去除括号内容，但保留主要名称（如“大雁塔(大慈恩寺)” → “大雁塔”）
        string cleanName = landmarkName.Split('(')[0].Trim();
        var allSpots = GetAllSpots();

        // 1. 精确匹配中文名
        var match = allSpots.FirstOrDefault(s => s.ChineseName == cleanName);
        if (match != null) return match;

        // 2. 精确匹配英文名（忽略大小写）
        match = allSpots.FirstOrDefault(s => s.Name.Equals(cleanName, StringComparison.OrdinalIgnoreCase));
        if (match != null) return match;

        // 3. 模糊匹配：cleanName 包含中文名（例如“大雁塔南广场”包含“大雁塔”）
        match = allSpots.FirstOrDefault(s => cleanName.Contains(s.ChineseName));
        if (match != null) return match;

        // 4. 模糊匹配：中文名包含 cleanName
        match = allSpots.FirstOrDefault(s => s.ChineseName.Contains(cleanName));
        if (match != null) return match;

        // 5. 最后尝试：英文名包含 cleanName 或 cleanName 包含英文名
        match = allSpots.FirstOrDefault(s => s.Name.Contains(cleanName, StringComparison.OrdinalIgnoreCase) ||
                                             cleanName.Contains(s.Name, StringComparison.OrdinalIgnoreCase));
        return match;
    }
}