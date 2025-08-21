using CnC.Application.DTOs.Email;
using CnC.Application.Shared.Responses;

namespace CnC.Application.Abstracts.Services;

public interface IEmailQueueService
{
    Task<EmailQueueResponse> PublishAsync(EmailMessageDto email);
}
