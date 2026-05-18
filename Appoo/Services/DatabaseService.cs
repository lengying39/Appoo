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
        // 同步创建表，简单可靠
        try
        {
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<UserReview>().Wait();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"创建表失败: {ex.Message}");
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
}