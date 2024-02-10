namespace Winku.DatabaseFolder
{
    public class Email
    {
        public int Id { get; set; }
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public DateTime SendTime { get; set; }
    }
}
