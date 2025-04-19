namespace TABP.Domain.Common;
public abstract class AuditEntity<Tkey> : EntityBase<Tkey>, IAuditEntity<Tkey>
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
