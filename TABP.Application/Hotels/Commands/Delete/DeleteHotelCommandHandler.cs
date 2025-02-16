﻿using MediatR;
using TABP.Application.Exceptions;
using TABP.Application.Exceptions.Messages;
using TABP.Domain.Enums;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Hotels.Commands.Delete;
public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomClassRepository _roomClassRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IImageRepository _imageRepository;

    public DeleteHotelCommandHandler(
        IHotelRepository hotelRepository,
        IRoomClassRepository roomClassRepository,
        IUnitOfWork unitOfWork,
        IImageRepository imageRepository)
    {
        _hotelRepository = hotelRepository;
        _roomClassRepository = roomClassRepository;
        _unitOfWork = unitOfWork;
        _imageRepository = imageRepository;
    }
    public async Task<Unit> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await _hotelRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(HotelExceptionMessages.NotFound);

        if (await _roomClassRepository.ExistsAsync(rc => rc.HotelId == request.Id))
        {
            throw new EntityInUseException(HotelExceptionMessages.EntityInUse);
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _imageRepository.DeleteByIdAsync(request.Id, ImageType.Thumbnail);
            await _imageRepository.DeleteByIdAsync(request.Id, ImageType.Gallery);

            _hotelRepository.DeleteAsync(hotel);

            await _unitOfWork.CommitAsync();

            return Unit.Value;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
