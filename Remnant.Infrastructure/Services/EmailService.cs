using System.Net;
using System.Net.Mail;

namespace Remnant.Core.Services
{
  public static class EmailService
  {
    public static string Host { private get; set; }
    public static string User { private get;  set; }
    public static string Password { private get;  set; }

    private static string Decode(string source, string key)
    {
      return CryptographyService.TripleDESDecode(source, key);
    }
 
    public static void SendEmail(MailMessage message, string key)
    {
      var smtp = new SmtpClient(Decode(Host, key)) {Credentials = new NetworkCredential(Decode(User, key), Decode(Password, key))};
      smtp.Send(message);
    }
  }
}
