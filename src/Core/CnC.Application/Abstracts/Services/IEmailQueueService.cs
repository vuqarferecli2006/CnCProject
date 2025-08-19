using CnC.Application.DTOs;
using CnC.Application.Shared.Responses;

namespace CnC.Application.Abstracts.Services;

public interface IEmailQueueService
{
    Task<EmailQueueResponse> PublishAsync(EmailMessageDto email);
}
