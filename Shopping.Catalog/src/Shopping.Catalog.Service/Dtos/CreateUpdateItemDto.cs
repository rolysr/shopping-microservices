using System.ComponentModel.DataAnnotations;

namespace Shopping.Catalog.Service.Dtos;

public record CreateUpdateItemDto
{
    [Required]
    public string ?Name { get; set;}
    public string ?Description { get; set;}
    [Range(0, 5000000)]
    public decimal Price { get; set;}
}