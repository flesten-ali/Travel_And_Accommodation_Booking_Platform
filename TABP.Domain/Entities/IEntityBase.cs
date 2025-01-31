namespace TABP.Domain.Entities;
public interface IEntityBase<TKey>
{
    TKey Id { get; set; }
}
