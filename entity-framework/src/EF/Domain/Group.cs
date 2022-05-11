using System;

namespace EF.Domain
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Property { get; set; }
    }
}