using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChoreRecorderApi.Helpers;
using ChoreRecorderApi.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ChoreRecorderApi.Services
{

    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(string id);
        Task<User> Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(string id);
    }
    public class UserService : IUserService
    {
        private MongoClient _client = null;
        private IMongoDatabase _db = null;

        private IOptions<AppSettings> _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            this._client = new MongoClient(appSettings.Value.MongoConnectionString);
            this._db = this._client.GetDatabase(appSettings.Value.MongoDatabase);
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var userCollection = _db.GetCollection<User>("Users");
            var users = userCollection.Find((u) => u.Username == username);

            // check if username exists
            if (!users.Any())
            {
                return null;
            }
            var user = users.FirstOrDefault();
            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public async Task<User> Create(User user, string password)
        {

            var userCollection = _db.GetCollection<User>("Users");
            user.Id = Guid.NewGuid().ToString();

            var foundUser = await userCollection.Find(u => u.Username == user.Username).AnyAsync();

            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                throw new AppException("Firstname is required");
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                throw new AppException("Lastname is required");
            }

            if (foundUser)
            {
                throw new AppException("Username \"" + user.Username + "\" is already taken");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            userCollection.InsertOne(user);

            return user;
        }

        public void Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public User GetById(string id)
        {
            var userCollection = _db.GetCollection<User>("Users");

            return userCollection.Find(u => u.Id == id).SingleOrDefault();
        }

        public void Update(User user, string password = null)
        {
            throw new System.NotImplementedException();
        }


        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }

}