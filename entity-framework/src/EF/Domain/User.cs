using System;

namespace EF.Domain
{
    public class User
    {

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Password { get; private set; }
        public bool IsActive { get; private set; }

        private User() { }

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