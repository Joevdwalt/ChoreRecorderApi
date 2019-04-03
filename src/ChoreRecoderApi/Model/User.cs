
namespace ChoreRecorderApi.Model
{
    public class User
    {
        public string Id {get;set;}
        public string Username { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }

}