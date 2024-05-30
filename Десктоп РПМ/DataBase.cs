using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

public class Book
{
    [Key]
    public int BookId { get; set; }

    [Required]
    public string Title { get; set; }

    public string ImageUrl { get; set; }

    [Required]
    public string Author { get; set; }

    [Required]
    public string Year { get; set; }

    [Required]
    public string Genre { get; set; }

    public string Description { get; set; }

    public string Text { get; set; }
}

public class User
{
    [Key]
    public string Email { get; set; }

    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }  

}

public class ReadingHistory
{
    [Key, Column(Order = 0)]
    public int BookId { get; set; }
    [ForeignKey("BookId")]
    public virtual Book Book { get; set; }

    [Key, Column(Order = 1)]
    public string Email { get; set; }
    [ForeignKey("Email")]
    public virtual User User { get; set; }


    public int LastReadPage { get; set; }
}

public class FavoriteBooks
{

    [Key, Column(Order = 0)]
    public int BookId { get; set; }
    [ForeignKey("BookId")]
    public virtual Book Book { get; set; }

    [Key, Column(Order = 1)]
    public string Email { get; set; }
    [ForeignKey("Email")]
    public virtual User User { get; set; }

}