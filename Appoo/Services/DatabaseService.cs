using SQLite;
using Appoo.Models;
using System.IO;          // 用于 Path
using System.Collections.Generic;  // 用于 IEnumerable
using System.Threading.Tasks;
using System;

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
            // 创建用户表、评价表、景点表（如果不存在）
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<UserReview>().Wait();
            _database.CreateTableAsync<TouristSpot>().Wait();   
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"创建SQLite表失败: {ex.Message}");
            throw;
        }
    }

    // ---------- 用户相关 ----------
    public Task<int> InsertUserAsync(User user)
    {
        return _database.InsertAsync(user);
    }

    public Task<User?> GetUserByUsernameAsync(string username)
    {
        return _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username);
    }

    public Task<User?> GetUserByCredentialsAsync(string username, string password)
    {
        return _database.Table<User>().FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    }

    public Task<int> UpdateUserAsync(User user)
    {
        return _database.UpdateAsync(user);
    }

    // ---------- 评价相关 ----------
    public Task<List<UserReview>> GetReviewsBySpotNameAsync(string spotName)
    {
        return _database.Table<UserReview>()
                        .Where(r => r.SpotName == spotName)
                        .OrderByDescending(r => r.DatePosted)
                        .ToListAsync();
    }

    public Task<List<UserReview>> SearchReviewsAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return Task.FromResult(new List<UserReview>());
        return _database.Table<UserReview>()
            .Where(r => r.Comment.Contains(keyword) || r.SpotName.Contains(keyword) || r.Username.Contains(keyword))
            .OrderByDescending(r => r.DatePosted)
            .ToListAsync();
    }

    public Task<List<UserReview>> GetReviewsByUsernameAsync(string username)
    {
        return _database.Table<UserReview>()
                        .Where(r => r.Username == username)
                        .OrderByDescending(r => r.DatePosted)
                        .ToListAsync();
    }

    public Task<int> AddReviewAsync(UserReview review)
    {
        return _database.InsertAsync(review);
    }

    // ---------- 景点相关（新增） ----------
    public Task<List<TouristSpot>> GetAllSpotsAsync()
    {
        return _database.Table<TouristSpot>().ToListAsync();
    }

    public Task<int> InsertSpotAsync(TouristSpot spot)
    {
        return _database.InsertAsync(spot);
    }

    public Task<int> InsertAllSpotsAsync(IEnumerable<TouristSpot> spots)
    {
        return _database.InsertAllAsync(spots);
    }
}