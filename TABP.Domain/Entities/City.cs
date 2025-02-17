﻿using TABP.Domain.Common;

namespace TABP.Domain.Entities;

public class City : AuditEntity<Guid>
{
    public string Name { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
    public ICollection<Hotel> Hotels { get; set; } = [];
    public Image Thumbnail { get; set; }
}
