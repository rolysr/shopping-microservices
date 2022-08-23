namespace Shopping.Catalog.Service.Dtos;

public record ItemDto
{
    public Guid Id { get; set;}
    public string ?Name { get; set;}
    public string ?Description { get; set;}
    public decimal Price { get; set;}
    public DateTimeOffset CreatedDate { get; set;}

}