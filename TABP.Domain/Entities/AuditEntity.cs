﻿
namespace TABP.Domain.Entities;
public abstract class AuditEntity<Tkey> : EntityBase<Tkey>, IAuditEntity<Tkey>
{
    public virtual DateTime CreatedDate { get; set; }
    public virtual DateTime? UpdatedDate { get; set; }
}
