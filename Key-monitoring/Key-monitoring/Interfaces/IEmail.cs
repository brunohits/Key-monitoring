namespace Key_monitoring.Interfaces;

public interface IEmail
{
    Task<bool> SendEmail(Guid idWhere, Guid idSomeone, int numberRoom);
    Task SendCode(int number, Guid id);
}