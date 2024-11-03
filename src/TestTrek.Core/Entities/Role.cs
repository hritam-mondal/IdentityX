namespace TestTrek.Core.Entities;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<UserRole> UserRoles { get; set; } = [];
}