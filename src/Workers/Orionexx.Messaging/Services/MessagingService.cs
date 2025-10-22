using System.Net;
using Orionexx.Core.Shared.Abstractions;

namespace Orionexx.Messaging.Services;

public interface IMessagingService
{
    Task<Result> SendEmailAsync<T>(string email, string eventName, T payload, CancellationToken cancellationToken = default);
}

public class MessagingService(
    ILogger<MessagingService> logger,
    IEmailTemplateService emailTemplateService,
    ISesEmailService sesEmailService) : IMessagingService
{
    public async Task<Result> SendEmailAsync<T>(string email, string eventName, T payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var emailTemplateName = emailTemplateService.GetEmailTemplateNameFromType(eventName);
            var emailSubject = emailTemplateService.GetEmailSubjectFromType(eventName);

            var emailBody = await emailTemplateService.ConvertTemplateToHtml(emailTemplateName, payload);
            var sendEmailResponse = await sesEmailService.SendEmailAsync(email, emailSubject, emailBody, cancellationToken);
            return sendEmailResponse?.HttpStatusCode == HttpStatusCode.OK ? Result.Success() : Result.Failure();
        }
        catch (Exception ex)
        {
            logger.LogError("Error sending email ${email}: ${ex}", email, ex);
            return Result.Failure();
        }
    }
}
