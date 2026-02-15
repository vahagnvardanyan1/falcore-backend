namespace VTS.DAL.Entities.Core;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreatedDateUtc { get; set; }
    public DateTime ModifiedDateUtc { get; set; }
}
