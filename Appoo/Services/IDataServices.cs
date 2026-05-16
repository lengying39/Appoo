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
    void Logout();

    // 景点收藏
    void AddFavoriteSpot(string spotName);
    void RemoveFavoriteSpot(string spotName);
    List<string> GetFavoriteSpots();

    // 美食收藏
    void AddFavoriteFood(string foodName);
    void RemoveFavoriteFood(string foodName);

    // 统一收藏
    List<string> GetAllFavorites();
    void ClearAllFavorites();

    // 兼容旧版（仅景点）
    void AddFavorite(string spotName);
    void RemoveFavorite(string spotName);
    List<string> GetFavorites();
    void ClearFavorites();

    // 数据方法
    List<TouristSpot> GetAllSpots();
    List<string> GetAllFoods();
    List<string> GetAllFacilities();
    TouristSpot? RecognizeAndMatchSpot(string? landmarkName);
}