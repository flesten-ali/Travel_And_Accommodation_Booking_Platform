using System.Text.Json.Serialization;
namespace TABP.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum SortOrder
{
    Ascending,
    Descending,
}
