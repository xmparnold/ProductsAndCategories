#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductsAndCategories.Models;

public class Product 
{
    [Key]
    public int ProductId { get; set; }

    [Required(ErrorMessage ="is required")]
    [MinLength(2, ErrorMessage ="must be at least 2 characters")]
    [MaxLength(45, ErrorMessage ="must be 45 characters or less")]
    public string Name { get; set; }

    [Required]
    public double Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Association> Categories { get; set; } = new List<Association>();
}