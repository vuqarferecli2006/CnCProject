﻿namespace CnC.Application.Abstracts.Services;

public interface IEmailService
{
    Task SendEmailAsync(IEnumerable<string> toEmail, string subject, string body);
}
