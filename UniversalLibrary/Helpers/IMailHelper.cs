namespace UniversalLibrary.Helpers
{
    public interface IMailHelper
    {
        Response SendEmail(string to, string subject, string body);
        Response SendEmailToAdmin(string to, string subject, string body);
    }
}
