using System;
namespace ChoreRecorderApi.Model
{
    public class UserProfile
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public int Age { get; set; }

        public string ProfilePicture { get; set; }
    }
}