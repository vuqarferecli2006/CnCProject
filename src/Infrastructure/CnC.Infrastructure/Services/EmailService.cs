using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CnC.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSetting _emailSetting;

    public EmailService(IOptions<EmailSetting> emailSetting)
    {
        _emailSetting = emailSetting.Value;
    }

    public async Task SendEmailAsync(IEnumerable<string> toEmail, string subject, string body)
    {
        using var smtp = new SmtpClient(_emailSetting.SmtpServer, _emailSetting.SmtpPort)
        {
            Credentials = new NetworkCredential(_emailSetting.SenderEmail, _emailSetting.Password),
            EnableSsl = true
        };

        using var message = new MailMessage
        {
            From = new MailAddress(_emailSetting.SenderEmail, _emailSetting.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        
        foreach(var email in toEmail.Distinct())
        {
            message.To.Add(email);
        }
        
        await smtp.SendMailAsync(message);
    
    }
}
