using System.Net;
using System.Net.Mail;
using Key_monitoring.Interfaces;
using Key_monitoring.Models;
using Keymonitoring.Migrations;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace Key_monitoring.Servises;

public class EmailService : IEmail
{
    private readonly ApplicationDbContext _dbContext;

    public EmailService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SendEmail(Guid idWhere, Guid idSomeone)
    {
        try
        {
            var userEmailWhere = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == idWhere);
            var userEmailSomeone = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == idSomeone);
            /*SmtpClient smtpClient = new SmtpClient("smtp.mail.ru");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.EnableSsl = true;

            System.Net.NetworkCredential basicInfo = new System.Net.NetworkCredential("testkeymonitoring1@mail.ru", "atMxV78ebPRQVZVpttGa");
            smtpClient.Credentials = basicInfo;
            MailAddress from = new MailAddress("testkeymonitoring1@mail.ru", "Admin");
            MailAddress toAddress = new MailAddress("maxxx321429@mail.ru", "Максим");
            MailMessage MyMail = new System.Net.Mail.MailMessage(from, toAddress);

            MailAddress replyTo = new MailAddress("testkeymonitoring1@mail.ru");
            MyMail.ReplyToList.Add(replyTo);
            MyMail.Subject = "Test Message";
            MyMail.SubjectEncoding = System.Text.Encoding.UTF8;

            MyMail.Body = "<b>Hi</b>";
            MyMail.BodyEncoding = System.Text.Encoding.UTF8;

            MyMail.IsBodyHtml = true;
            smtpClient.Send(MyMail);*/

            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();

            int number = _rdm.Next(_min, _max);
            MailAddress from = new MailAddress("keymonitoring2024@gmail.com", "Admin");
            MailAddress toAddress = new MailAddress($"{userEmailSomeone.Email}", $"{userEmailSomeone.FullName}");

            MailMessage mailMessage = new MailMessage(from, toAddress);
            mailMessage.Subject = "Ключ для авторизации";
            mailMessage.Body = $"{number}";


            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(from.Address, "gawz ykxd xgqz syzs");
            await smtpClient.SendMailAsync(mailMessage);

            var addDbNumbre = new CodeForEmailModel()
            {
                Id = new Guid(),
                IdToAdress = userEmailSomeone.Id,
                IdFromAdress = userEmailWhere.Id,
                LifeOfCode = DateTime.UtcNow,
                Code = number
            };
            _dbContext.Add(addDbNumbre);
            _dbContext.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending email: {ex.Message}");
            return false;
        }
    }
}