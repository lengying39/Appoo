namespace Appoo.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // 明文仅用于演示
        public string Nickname { get; set; }
        public string Avatar { get; set; }
        public List<string> FavoriteSpotNames { get; set; } = new List<string>();
    }
}