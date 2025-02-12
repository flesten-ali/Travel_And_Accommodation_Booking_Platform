namespace TABP.Domain.Entities;
public interface IAuditEntity
{
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
}

public interface IAuditEntity<Tkey> : IAuditEntity
{
}