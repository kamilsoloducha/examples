using System;
using MediatR;

namespace EF.Application.Users.GetById
{
    public class GetByIdQuery : IRequest<GetByIdResponse>
    {
        public Guid Id { get; set; }
    }
}