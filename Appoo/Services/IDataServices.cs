using Appoo.Models;
using Appoo.Models;   // 只使用 Appoo.Models，不要混用 Appo.Models

namespace Appoo.Services;

public interface IDataService
{
    User? CurrentUser { get; }
    Task<bool> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(User user);
    void AddFavorite(string spotName);
    void RemoveFavorite(string spotName);
    List<string> GetFavorites();
    void ClearFavorites();
    void Logout();
    List<TouristSpot> GetAllSpots();
    List<string> GetAllFoods();
    List<string> GetAllFacilities();

    // 识别结果匹配本地景点
    TouristSpot? RecognizeAndMatchSpot(string? landmarkName);
}