﻿using MediatR;

namespace TABP.Application.Owners.Commands.Delete;
public class DeleteOwnerCommand : IRequest
{
    public Guid Id { get; set; }
}
