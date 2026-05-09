using Appoo.Models;

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
    List<TouristSpot> GetAllSpots();
    List<string> GetAllFoods();
    List<string> GetAllFacilities();
}