using Appoo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appoo.Services;

public class DataService : IDataService
{
    private readonly DatabaseService _dbService;
    public User? CurrentUser { get; private set; }
    private List<TouristSpot>? _allSpotsCache;

    public DataService(DatabaseService dbService)
    {
        _dbService = dbService;
        _ = EnsureTestUserAsync();
        _ = InitializeSpotsAsync();
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

    private async Task InitializeSpotsAsync()
    {
        var spotsInDb = await _dbService.GetAllSpotsAsync();
        if (spotsInDb == null || spotsInDb.Count == 0)
        {
            var initialSpots = GetInitialSpots();
            await _dbService.InsertAllSpotsAsync(initialSpots);
        }
        _allSpotsCache = await _dbService.GetAllSpotsAsync();
    }

    private List<TouristSpot> GetInitialSpots()
    {
        return new List<TouristSpot>
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
    }

    // 同步获取景点列表（兼容原有代码）
    public List<TouristSpot> GetAllSpots()
    {
        if (_allSpotsCache == null)
        {
            // 同步等待初始化完成（仅首次调用）
            Task.Run(async () =>
            {
                if (_allSpotsCache == null)
                {
                    var spots = await _dbService.GetAllSpotsAsync();
                    if (spots == null || spots.Count == 0)
                        await InitializeSpotsAsync();
                    else
                        _allSpotsCache = spots;
                }
            }).GetAwaiter().GetResult();
        }
        return _allSpotsCache ?? new List<TouristSpot>();
    }

    // ---------- 用户相关 ----------
    public async Task<bool> LoginAsync(string username, string password)
    {
        var user = await _dbService.GetUserByCredentialsAsync(username, password);
        if (user != null)
        {
            CurrentUser = user;
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

    public void Logout()
    {
        CurrentUser = null;
    }

    public Task<User?> GetCurrentUserAsync() => Task.FromResult(CurrentUser);

    // ---------- 统一收藏（景点 + 美食）----------
    private string PrefixSpot(string name) => $"Spot:{name}";
    private string PrefixFood(string name) => $"Food:{name}";
    private bool IsSpotItem(string item) => item.StartsWith("Spot:");
    private bool IsFoodItem(string item) => item.StartsWith("Food:");
    private string Unwrap(string item) => item.Substring(item.IndexOf(':') + 1);

    private async void SaveFavoritesAsync(List<string> list)
    {
        if (CurrentUser == null) return;
        CurrentUser.FavoriteSpotNames = list;
        await _dbService.UpdateUserAsync(CurrentUser);
    }

    public void AddFavoriteSpot(string spotName)
    {
        if (CurrentUser == null) return;
        var list = CurrentUser.FavoriteSpotNames;
        var prefixed = PrefixSpot(spotName);
        if (!list.Contains(prefixed))
        {
            list.Add(prefixed);
            SaveFavoritesAsync(list);
        }
    }

    public void RemoveFavoriteSpot(string spotName)
    {
        if (CurrentUser == null) return;
        var list = CurrentUser.FavoriteSpotNames;
        var prefixed = PrefixSpot(spotName);
        if (list.Contains(prefixed))
        {
            list.Remove(prefixed);
            SaveFavoritesAsync(list);
        }
    }

    public void AddFavoriteFood(string foodName)
    {
        if (CurrentUser == null) return;
        var list = CurrentUser.FavoriteSpotNames;
        var prefixed = PrefixFood(foodName);
        if (!list.Contains(prefixed))
        {
            list.Add(prefixed);
            SaveFavoritesAsync(list);
        }
    }

    public void RemoveFavoriteFood(string foodName)
    {
        if (CurrentUser == null) return;
        var list = CurrentUser.FavoriteSpotNames;
        var prefixed = PrefixFood(foodName);
        if (list.Contains(prefixed))
        {
            list.Remove(prefixed);
            SaveFavoritesAsync(list);
        }
    }

    public List<string> GetFavoriteSpots() =>
        CurrentUser?.FavoriteSpotNames.Where(IsSpotItem).Select(Unwrap).ToList() ?? new List<string>();

    public List<string> GetAllFavorites() => CurrentUser?.FavoriteSpotNames ?? new List<string>();

    public void ClearAllFavorites()
    {
        if (CurrentUser == null) return;
        CurrentUser.FavoriteSpotNames = new List<string>();
        SaveFavoritesAsync(CurrentUser.FavoriteSpotNames);
    }

    // 兼容旧版方法（仅返回景点收藏）
    public List<string> GetFavorites() => GetFavoriteSpots();

    public void AddFavorite(string spotName) => AddFavoriteSpot(spotName);
    public void RemoveFavorite(string spotName) => RemoveFavoriteSpot(spotName);
    public void ClearFavorites() => ClearAllFavorites();

    // ---------- 固定数据 ----------
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
        if (string.IsNullOrWhiteSpace(landmarkName)) return null;
        string cleanName = landmarkName.Split('(')[0].Trim();
        var allSpots = GetAllSpots();

        var match = allSpots.FirstOrDefault(s => s.ChineseName == cleanName);
        if (match != null) return match;
        match = allSpots.FirstOrDefault(s => s.Name.Equals(cleanName, StringComparison.OrdinalIgnoreCase));
        if (match != null) return match;
        match = allSpots.FirstOrDefault(s => cleanName.Contains(s.ChineseName));
        if (match != null) return match;
        match = allSpots.FirstOrDefault(s => s.ChineseName.Contains(cleanName));
        if (match != null) return match;
        match = allSpots.FirstOrDefault(s => s.Name.Contains(cleanName, StringComparison.OrdinalIgnoreCase) ||
                                             cleanName.Contains(s.Name, StringComparison.OrdinalIgnoreCase));
        return match;
    }
}