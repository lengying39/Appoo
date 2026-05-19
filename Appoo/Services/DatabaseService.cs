using SQLite;
using Appoo.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Appoo.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "travelapp.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        try
        {
            // 创建所有需要的表（如果不存在则创建，已存在则忽略）
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<UserReview>().Wait();
            _database.CreateTableAsync<TouristSpot>().Wait();   // 确保表存在
            _database.CreateTableAsync<FoodItem>().Wait();      // 确保表存在
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"数据库初始化失败: {ex.Message}");
            throw;
        }
    }

    // ---------- 用户相关 ----------
    public Task<int> InsertUserAsync(User user) => _database.InsertAsync(user);
    public Task<User?> GetUserByUsernameAsync(string username)
        => _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username);
    public Task<User?> GetUserByCredentialsAsync(string username, string password)
        => _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    public Task<int> UpdateUserAsync(User user) => _database.UpdateAsync(user);

    // ---------- 评价相关 ----------
    public Task<List<UserReview>> GetReviewsBySpotNameAsync(string spotName)
        => _database.Table<UserReview>().Where(r => r.SpotName == spotName).OrderByDescending(r => r.DatePosted).ToListAsync();
    public Task<List<UserReview>> GetReviewsByUsernameAsync(string username)
        => _database.Table<UserReview>().Where(r => r.Username == username).OrderByDescending(r => r.DatePosted).ToListAsync();
    public Task<int> AddReviewAsync(UserReview review) => _database.InsertAsync(review);

    // ---------- 景点相关 ----------
    public Task<List<TouristSpot>> GetAllSpotsAsync()
        => _database.Table<TouristSpot>().ToListAsync();

    // ---------- 美食相关 ----------
    public Task<List<FoodItem>> GetFoodItemsBySpotNameAsync(string spotName)
        => _database.Table<FoodItem>().Where(f => f.SpotName == spotName).ToListAsync();

    // 用于收藏页面获取所有美食（可选）
    public Task<List<FoodItem>> GetAllFoodItemsAsync()
        => _database.Table<FoodItem>().ToListAsync();
}