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

    // 实现 IDataService 的异步登录/注册/登出
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
        if (existing != null)
            return false;
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

    // 实现 IDataService 要求的同步收藏方法（内部调用异步版本，使用 async void）
    public void AddFavorite(string spotName)
    {
        _ = AddFavoriteInternalAsync(spotName);
    }

    public void RemoveFavorite(string spotName)
    {
        _ = RemoveFavoriteInternalAsync(spotName);
    }

    public void ClearFavorites()
    {
        _ = ClearFavoritesInternalAsync();
    }

    // 异步实际实现（修正版：触发序列化）
    private async Task AddFavoriteInternalAsync(string spotName)
    {
        if (CurrentUser == null) return;
        var list = CurrentUser.FavoriteSpotNames;
        if (!list.Contains(spotName))
        {
            list.Add(spotName);
            CurrentUser.FavoriteSpotNames = list;  // 触发 setter 序列化
            await _dbService.UpdateUserAsync(CurrentUser);
        }
    }

    private async Task RemoveFavoriteInternalAsync(string spotName)
    {
        if (CurrentUser == null) return;
        var list = CurrentUser.FavoriteSpotNames;
        if (list.Contains(spotName))
        {
            list.Remove(spotName);
            CurrentUser.FavoriteSpotNames = list;  // 触发 setter 序列化
            await _dbService.UpdateUserAsync(CurrentUser);
        }
    }

    private async Task ClearFavoritesInternalAsync()
    {
        if (CurrentUser == null) return;
        CurrentUser.FavoriteSpotNames = new List<string>();  // 赋新空列表，触发序列化
        await _dbService.UpdateUserAsync(CurrentUser);
    }

    public List<string> GetFavorites()
    {
        return CurrentUser?.FavoriteSpotNames ?? new List<string>();
    }

    // 如果需要 GetCurrentUserAsync（接口中可能已有）
    public Task<User?> GetCurrentUserAsync() => Task.FromResult(CurrentUser);

    // ---------- 以下为固定数据方法，与原代码相同 ----------
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
        string cleanName = landmarkName.Split('(')[0].Trim();
        var allSpots = GetAllSpots();

        return null; // 占位
    }
}