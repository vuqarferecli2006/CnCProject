namespace CnC.Application.Shared.Responses;

public class RoleWithPermissionsResponse
{
    public string RoleId { get; set; } = null!;
    public string RoleName { get; set; } = null!;
    public List<string> Permissions { get; set; } = new();
}

