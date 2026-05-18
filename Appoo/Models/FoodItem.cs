using SQLite;

namespace Appoo.Models;

[SQLite.Table("FoodItems")]
public class FoodItem
{
    [SQLite.PrimaryKey, SQLite.AutoIncrement]
    public int Id { get; set; }
    public string SpotName { get; set; }   // 关联景点英文名
    public string Name { get; set; }       // 餐厅名
    public string Description { get; set; }
    public string Price { get; set; }
    public string Dish { get; set; }
    public string ImageFileName { get; set; }
}