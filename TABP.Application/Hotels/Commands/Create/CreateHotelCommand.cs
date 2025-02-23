using MediatR;
using TABP.Application.Hotels.Common;
namespace TABP.Application.Hotels.Commands.Create;

public sealed record CreateHotelCommand(
    string Name,
    string? Description, 
    int Rate, 
    double LongitudeCoordinates,
    double LatitudeCoordinates,
    Guid CityId,
    Guid OwnerId) : IRequest<HotelResponse>;
