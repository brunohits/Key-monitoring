namespace Key_monitoring.Interfaces;

public interface IEmail
{
    Task<bool> SendEmail(Guid idWhere, Guid idSomeone);
}