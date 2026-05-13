using SQLite;
using Appoo.Models;

namespace Appoo.Services;

public class DatabaseService
{
    private readonly SQLiteAsyncConnection _database;

    public DatabaseService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "travelapp.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        // 自动创建评价表（如果不存在）
        _database.CreateTableAsync<UserReview>().Wait();
    }

    // 根据景点名称获取所有评价（用于查看评价页面）
    public Task<List<UserReview>> GetReviewsBySpotNameAsync(string spotName)
    {
        return _database.Table<UserReview>()
                        .Where(r => r.SpotName == spotName)
                        .OrderByDescending(r => r.DatePosted)
                        .ToListAsync();
    }

    // 根据用户名获取所有评价（用于我的评价页面）
    public Task<List<UserReview>> GetReviewsByUsernameAsync(string username)
    {
        return _database.Table<UserReview>()
                        .Where(r => r.Username == username)
                        .OrderByDescending(r => r.DatePosted)
                        .ToListAsync();
    }

    // 添加一条评价
    public Task<int> AddReviewAsync(UserReview review)
    {
        return _database.InsertAsync(review);
    }
}