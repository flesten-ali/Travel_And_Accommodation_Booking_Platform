﻿using AutoMapper;
using MediatR;
using TABP.Application.Reviews.Common;
using TABP.Domain.Constants.ExceptionMessages;
using TABP.Domain.Entities;
using TABP.Domain.Exceptions;
using TABP.Domain.Interfaces.Persistence;
using TABP.Domain.Interfaces.Persistence.Repositories;

namespace TABP.Application.Reviews.Commands.Create;

/// <summary>
/// Handles the command to create a review for a hotel.
/// </summary>
public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResponse>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IHotelRepository hotelRepository,
        IUserRepository userRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _reviewRepository = reviewRepository;
        _hotelRepository = hotelRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Handles the request to create a review for a hotel.
    /// </summary>
    /// <param name="request">The command containing the data for the review to be created.</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
    /// <returns>
    /// A task representing the asynchronous operation, returning the review details mapped to the response DTO.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown if the specified hotel or user does not exist.
    /// </exception>
    public async Task<ReviewResponse> Handle(CreateReviewCommand request, CancellationToken cancellationToken = default)
    {
        if (!await _hotelRepository.ExistsAsync(h => h.Id == request.HotelId, cancellationToken))
        {
            throw new NotFoundException(HotelExceptionMessages.NotFound);
        }

        if (!await _userRepository.ExistsAsync(u => u.Id == request.UserId, cancellationToken))
        {
            throw new NotFoundException(UserExceptionMessages.NotFound);
        }
        var review = _mapper.Map<Review>(request);

        await _reviewRepository.CreateAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ReviewResponse>(review);
    }
}
