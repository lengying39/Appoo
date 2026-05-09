namespace Appoo.Models;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Nickname { get; set; }
    public List<string> FavoriteSpotNames { get; set; } = new();
}