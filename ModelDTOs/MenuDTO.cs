namespace ServerPupusas.ModelDTOs;

public class MenuDTO
{
    public int MenuId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Category { get; set; }

    public decimal Price { get; set; }

    public bool? Availability { get; set; }
}