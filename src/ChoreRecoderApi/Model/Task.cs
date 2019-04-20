using System;
namespace ChoreRecorderApi.Model
{
    public class Task
    {
        public string Id { get; set; }
        
        public string Key { get; set; }

        public string Name { get; set; }

        public int Points { get; set; }

        public DateTime CreateDate {get;set;}

        public DateTime DueDate {get;set;}

        public string OwnerId {get;set;}
       
        public bool Done { get; set; }
    }
}