namespace TABP.Domain.Entities;
public interface IAuditEntity<Tkey>  
{
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
}