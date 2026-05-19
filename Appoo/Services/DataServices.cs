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
        // 预加载景点缓存（同步等待，确保首次调用时数据已就绪）
        _allSpotsCache = Task.Run(async () => await _dbService.GetAllSpotsAsync()).Result;
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

    // 从数据库获取景点列表（使用缓存）
    public List<TouristSpot> GetAllSpots()
    {
        if (_allSpotsCache == null)
        {
            _allSpotsCache = Task.Run(async () => await _dbService.GetAllSpotsAsync()).Result;
        }
        return _allSpotsCache ?? new List<TouristSpot>();
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

    public List<string> GetAllFavorites()
    {
        return CurrentUser?.FavoriteSpotNames ?? new List<string>();
    }

    // 兼容旧方法
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

    // 其他数据（不变）
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