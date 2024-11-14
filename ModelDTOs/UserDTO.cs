namespace ServerPupusas.ModelDTOs;

public class UserCreateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}
