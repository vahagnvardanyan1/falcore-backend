namespace VTS.BLL.DTOs;

public class TenantDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Guid APIKey { get; set; }
}
