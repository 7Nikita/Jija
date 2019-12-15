namespace Jija.Services
{
    public interface IMailing
    {
        void Send(string to, string subject, string body);
    }
}