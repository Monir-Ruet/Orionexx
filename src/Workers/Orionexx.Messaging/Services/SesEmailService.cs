using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Orionexx.Messaging.Configurations;

namespace Orionexx.Messaging.Services;

public interface ISesEmailService
{
    Task<SendEmailResponse?> SendEmailAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default);
}

public class SesEmailService(
    IAppConfiguration appConfiguration,
    IAmazonSimpleEmailService sesClient) : ISesEmailService
{
    public async Task<SendEmailResponse?> SendEmailAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        var sendRequest = new SendEmailRequest
        {
            Source = appConfiguration.AwsSesSettings.FromEmail,
            Destination = new Destination
            {
                ToAddresses = [toEmail]
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Html = new Content
                    {
                        Charset = "UTF-8",
                        Data = htmlBody
                    }
                }
            }
        };

        return await sesClient.SendEmailAsync(sendRequest, cancellationToken);
    }
}
