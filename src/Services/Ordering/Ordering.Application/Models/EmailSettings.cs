namespace Ordering.Application.Models
{
    public class EmailSettings
    {
        public string SMTPServer { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string EmailUsername { get; set; }
        public string EmailPassword { get; set; }
        
       
    }
}
