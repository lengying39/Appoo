using SQLite;

namespace Appoo.Models;

[SQLite.Table("Users")]
public class User
{
    [SQLite.PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }

    [SQLite.MaxLength(50), SQLite.Unique]
    public string? Username { get; set; }   // 改为可空

    [SQLite.MaxLength(100)]
    public string? Password { get; set; }   // 改为可空

    [SQLite.MaxLength(50)]
    public string? Nickname { get; set; }

    public string? FavoriteSpotNamesSerialized { get; set; }

    [SQLite.Ignore]
    public List<string> FavoriteSpotNames
    {
        get
        {
            if (string.IsNullOrEmpty(FavoriteSpotNamesSerialized))
                return new List<string>();
            return FavoriteSpotNamesSerialized.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        set
        {
            FavoriteSpotNamesSerialized = value != null ? string.Join(",", value) : "";
        }
    }
}