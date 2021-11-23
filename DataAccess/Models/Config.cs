namespace DataAccess.Models
{
    public class Config
    {
        public string Input { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int Threads { get; set; } = 1;
        public bool Notify { get; set; }
        public bool LogToUI { get; set; }
    }
}