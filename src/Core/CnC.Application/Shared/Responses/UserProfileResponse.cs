namespace CnC.Application.Shared.Responsesl;

public class UserProfileResponse
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? ProfilePicture { get; set; }
}
