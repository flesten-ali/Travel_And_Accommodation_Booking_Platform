﻿namespace TABP.Presentation.DTOs.City;
public class CreateCityRequest
{
    public string Name { get; set; }
    public string? PostalCode { get; set; }
    public string? Address { get; set; }
    public string Country { get; set; }
    public string PostOffice { get; set; }
}