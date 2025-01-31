using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TABP.Domain.Entities;
public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual TKey Id { get; set; }
}
