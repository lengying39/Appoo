using System.Collections.Generic;

namespace Assignment
{
    public static class FavoriteManager
    {
        private static List<string> _favorites = new List<string>();

        public static void AddFavorite(string title)
        {
            if (!_favorites.Contains(title))
                _favorites.Add(title);
        }

        public static List<string> GetFavorites()
        {
            return new List<string>(_favorites);
        }
    }
}