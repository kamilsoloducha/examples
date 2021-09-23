using System;
using MongoDB.Bson.Serialization.Attributes;

namespace NoSql.Domain
{
    public class User
    {
        [BsonId]
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
        public Address Address { get; set; }

        [BsonIgnore]
        public PersonalData PersonalData { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }

    public class PersonalData
    {
        [BsonId]
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}