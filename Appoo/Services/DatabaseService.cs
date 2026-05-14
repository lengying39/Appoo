using SQLite;
using Appoo.Models;   // 注意：使用 Appoo.Models，不要写成 Appo.Models

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
            // 创建用户表和评价表（如果不存在）
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<UserReview>().Wait();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"创建SQLite表失败: {ex.Message}");
            throw;  // 如果创建失败，抛出异常以便调试
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
}