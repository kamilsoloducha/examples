namespace NoSql.Api
{
    public class AddUserRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
    }
}