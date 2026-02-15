using VTS.DAL.Entities.Core;

namespace VTS.DAL.Entities;

public class Tenant : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Slug { get; set; }
    public Guid APIKey { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; }
}
