namespace ScreeningAutomation.API.Options
{
    public class EmailSenderOptions
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public EmailCredentials Credentials { get; set; }
    }
}
