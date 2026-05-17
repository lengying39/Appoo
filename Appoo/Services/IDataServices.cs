using Appoo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appoo.Services;

public interface IDataService
{
    User? CurrentUser { get; }
    Task<bool> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(User user);
    Task<User?> GetCurrentUserAsync();

    // 景点收藏
    void AddFavoriteSpot(string spotName);
    void RemoveFavoriteSpot(string spotName);
    List<string> GetFavoriteSpots();

    // 美食收藏
    void AddFavoriteFood(string foodName);
    void RemoveFavoriteFood(string foodName);

    // 获取所有收藏（带前缀，用于统一展示）
    List<string> GetAllFavorites();

    // 兼容旧方法（仅景点）
    void AddFavorite(string spotName);
    void RemoveFavorite(string spotName);
    List<string> GetFavorites();
    void ClearFavorites();

    void Logout();
    List<TouristSpot> GetAllSpots();
    List<string> GetAllFoods();
    List<string> GetAllFacilities();
    TouristSpot? RecognizeAndMatchSpot(string? landmarkName);
}