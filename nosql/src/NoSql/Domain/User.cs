using System;
using MongoDB.Bson.Serialization.Attributes;

namespace NoSql.Domain
{
    public class User
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }
}