using SQLite;

namespace Appoo.Models;

[SQLite.Table("UserReviews")]
public class UserReview
{
    [SQLite.PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }

    [SQLite.MaxLength(100)]
    public string SpotName { get; set; }   // 景点英文名或中文名

    [SQLite.MaxLength(100)]
    public string Username { get; set; }   // 评论者用户名

    [SQLite.MaxLength(500)]
    public string Comment { get; set; }    // 文字评论

    public string ImagePath { get; set; }  // 可选图片本地路径

    public DateTime DatePosted { get; set; } = DateTime.Now;
}