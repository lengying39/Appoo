using Appoo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appoo.Services
{
    public class DataService : IDataService
    {
        private List<User> _users = new List<User>();
        public User CurrentUser { get; set; }

        public DataService()
        {
            // 预置一个测试用户
            _users.Add(new User { Username = "test", Password = "123", Nickname = "Tester" });
        }

        public Task<bool> LoginAsync(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                CurrentUser = user;
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> RegisterAsync(User user)
        {
            if (_users.Any(u => u.Username == user.Username))
                return Task.FromResult(false);
            _users.Add(user);
            CurrentUser = user;
            return Task.FromResult(true);
        }

        public void AddFavorite(string spotName)
        {
            if (CurrentUser != null && !CurrentUser.FavoriteSpotNames.Contains(spotName))
                CurrentUser.FavoriteSpotNames.Add(spotName);
        }

        public void RemoveFavorite(string spotName)
        {
            CurrentUser?.FavoriteSpotNames.Remove(spotName);
        }

        public List<string> GetFavorites()
        {
            return CurrentUser?.FavoriteSpotNames ?? new List<string>();
        }

        public void ClearFavorites()
        {
            CurrentUser?.FavoriteSpotNames.Clear();
        }

        // 预置景点数据
        public List<TouristSpot> GetAllSpots()
        {
            return new List<TouristSpot>
            {
                new TouristSpot
                {
                    Name = "Emperor Qinshihuang's Mausoleum Site Museum",
                    Description = "The Terracotta Army is a collection of terracottas.",
                    OpenTime = "8:00 - 18:00",
                    Location = "Lintong District, Xi'an",
                    ImageFile = "spot_bingmayong.jpg",
                    NearbyFood = new List<string> { "Biängbiäng Noodles", "Roujiamo" }
                },
                new TouristSpot
                {
                    Name = "Dayan Pagoda",
                    Description = "A famous Buddhist pagoda in Xi'an.",
                    OpenTime = "8:00 - 17:30",
                    Location = "Yanta District, Xi'an",
                    ImageFile = "spot_dayanta.jpg",
                    NearbyFood = new List<string> { "Liangpi", "Yangroupaomo" }
                },
                new TouristSpot
                {
                    Name = "The Xi'an Circumvallation",
                    Description = "The most complete ancient city wall in China.",
                    OpenTime = "8:00 - 22:00",
                    Location = "Downtown Xi'an",
                    ImageFile = "spot_zhonglou.jpg",
                    NearbyFood = new List<string> { "Defachang Dumplings", "Muslim Street BBQ" }
                }
            };
        }

        public List<string> GetAllFoods()
        {
            return new List<string> { "Biängbiäng Noodles", "Roujiamo", "Liangpi", "Yangroupaomo", "Defachang Dumplings" };
        }

        public List<string> GetAllFacilities()
        {
            return new List<string>
            {
                "Public Toilet - 50m - Open",
                "Charging Station - 200m - Available",
                "Accessible Passage - 100m - Open",
                "Ticket Vending Machine - 300m - Working",
                "Water Fountain - 150m - Open"
            };
        }
    }
}