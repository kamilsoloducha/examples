using System;
using System.Collections.Generic;

namespace EF.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Group> Groups { get; set; }

        public User() { }

        public static User Create(string name, string password)
        {
            var user = new User();
            user.Id = Guid.NewGuid();
            user.IsActive = true;

            user.Name = name;
            user.Password = password;

            return user;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}