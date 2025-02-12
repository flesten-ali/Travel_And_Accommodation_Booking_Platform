
namespace TABP.Domain.Entities;
public abstract class AuditEntity<Tkey> : EntityBase<Tkey>
{
    public virtual DateTime CreatedDate { get; set; }
    public virtual DateTime? UpdatedDate { get; set; }
}