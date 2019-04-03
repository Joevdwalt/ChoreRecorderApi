namespace ChoreRecorderApi.Model
{
    public class Task
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }

        public string OwnerId {get;set;}
        public bool Done { get; set; }
    }
}