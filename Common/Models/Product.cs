using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models;

[Table("products")]
public class Product
{
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("stock")]
    public int Amount { get; set; }
    /*
    [Column("price")]
    public string Price { get; set; }*/
}