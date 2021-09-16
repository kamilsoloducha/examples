using System;

namespace EF.Application.Users.GetById
{
    public class GetByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}