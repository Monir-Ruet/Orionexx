using Microsoft.AspNetCore.Mvc;
using Orionexx.Core.Shared.Constants;
using Orionexx.Core.Shared.Events.Account;
using Orionexx.Messaging.Logging;
using Orionexx.Messaging.Services;

namespace Orionexx.Messaging.EventsHandler;

public static class AccountEventHandler
{
    public static IEndpointRouteBuilder HandleAccountEvents(this IEndpointRouteBuilder app)
    {
        app.MapPost("/AccountCreated", async (
            [FromBody] AccountCreated eventData,
            IServiceProvider serviceProvider) =>
        {
            var logger = serviceProvider.GetRequiredService<ILogger<AccountCreated>>();
            var messagingService = serviceProvider.GetRequiredService<IMessagingService>();
            try
            {
                await messagingService.SendEmailAsync(eventData.Email, TopicConstants.AccountCreated, eventData);
                return Results.Ok();
            }
            catch (Exception ex)
            {
                logger.AccountCreattionFailure(ex);
                return Results.InternalServerError();
            }
        }).WithTopic(TopicConstants.PubSub, TopicConstants.AccountCreated);

        return app;
    }
}