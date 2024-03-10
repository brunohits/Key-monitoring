using System.Net;
using System.Net.Mail;
using Key_monitoring.Enum;
using Key_monitoring.Interfaces;
using Key_monitoring.Models;
using Keymonitoring.Migrations;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace Key_monitoring.Servises;

public class EmailService : IEmail
{
    private readonly ApplicationDbContext _dbContext;

    public EmailService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SendEmail(Guid idWhere, Guid idSomeone, int numberRoom)
    {
        var userEmailWhere = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == idWhere);
        var userEmailSomeone = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == idSomeone);
        var checkNumberRoom = await _dbContext.KeyModels.FirstOrDefaultAsync(x => x.CabinetNumber == numberRoom);
        if (checkNumberRoom.OwnerId != idSomeone)
        {
            throw new BadHttpRequestException("Вы не владеете данным ключом");
        }

        if (idWhere == idSomeone)
        {
            throw new BadHttpRequestException("Ключ нельзя передать самому себе");   
        }
        if (userEmailWhere == null)
        {
            throw new KeyNotFoundException("Данный пользователь не найден");
        }

        if (checkNumberRoom == null)
        {
            throw new KeyNotFoundException("Данный кабинет не найден");
        }

        int _min = 1000;
        int _max = 9999;
        Random _rdm = new Random();

        int number = _rdm.Next(_min, _max);
        MailAddress from = new MailAddress("keymonitoring2024@gmail.com", "Admin");
        MailAddress toAddress = new MailAddress($"{userEmailWhere.Email}", $"{userEmailWhere.FullName}");

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
            Id = Guid.NewGuid(),
            IdToAdress = userEmailSomeone.Id,
            IdFromAdress = userEmailWhere.Id,
            LifeOfCode = DateTime.UtcNow,
            Code = number,
            NumberRoom = numberRoom
        };
        _dbContext.Add(addDbNumbre);
       await _dbContext.SaveChangesAsync();
    }

    public async Task SendCode(int number, Guid id)
    {
        int _min = 1000;
        int _max = 9999;
        if (_min > number || _max < number)
        {
            throw new ArgumentException($"{number} - должен быть меньше 9999 и больше 1000");
        }

        var checkCode = await _dbContext.CodeForEmails.FirstOrDefaultAsync(x => x.Code == number && x.IdToAdress == id);
        if (checkCode == null)
        {
            throw new KeyNotFoundException("Данный код не найден");
        }

        DateTime time = DateTime.UtcNow.AddMinutes(5);
        if (checkCode.LifeOfCode > DateTime.UtcNow.AddMinutes(5))
        {
            throw new ArgumentException("Данный код устарел");
        }

        var searchRoom = await _dbContext.KeyModels.FirstOrDefaultAsync(x => x.CabinetNumber == checkCode.NumberRoom);

        if (searchRoom == null)
        {
            throw new KeyNotFoundException("Данный кабинет не найден");
        }
        else
        {
            var own = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == checkCode.IdFromAdress);
            if (own == null)
            {
                var exception = new Exception();
                exception.Data.Add(StatusCodes.Status404NotFound.ToString(), "Ahtung! User not found");
                throw exception;
            }

            var pairs = await _dbContext.Schedule.Where(x => x.PairStart <= DateTime.UtcNow).ToListAsync();

            if (pairs == null || pairs.Count == 0)
            {
                throw new ArgumentException("Расписание такого нет");
            }

            pairs = pairs.OrderByDescending(x => x.PairStart).ToList();

            var applic = await _dbContext.Applications.FirstOrDefaultAsync(
                x => x.KeyId == searchRoom.Id &&
                x.UserId == searchRoom.OwnerId &&
                x.ScheduleId == pairs[0].Id &&
                x.Status == ApplicationStatusEnum.Approved);

            if (applic == null)
            {
                throw new ArgumentException("Заявки такой нет");
            }

            applic.Status = ApplicationStatusEnum.KeyGiven;
            _dbContext.Applications.Update(applic);
            _dbContext.SaveChanges();

            searchRoom.Owner = own;
            searchRoom.OwnerId = checkCode.IdFromAdress;
            searchRoom.Status = KeyStatusEnum.OnHands;
            await _dbContext.SaveChangesAsync();
            _dbContext.CodeForEmails.Remove(checkCode);
            _dbContext.SaveChanges();
        }
    }
}