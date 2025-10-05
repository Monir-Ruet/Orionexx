using Microsoft.AspNetCore.Mvc;
using Orionexx.Core.Shared.Constants;
using Orionexx.Messaging.Events;

namespace Orionexx.Messaging.EventsHandler;

public static class AccountEventHandler
{
    public static IEndpointRouteBuilder HandleAccountEvents(this IEndpointRouteBuilder app)
    {
        app.MapPost("/AccountCreated", ([FromBody] AccountCreated data) =>
        {
            Console.WriteLine($"AccountCreated event received: {data.Email}");
            return Results.Ok();
        }).WithTopic(TopicConstants.PubSub, TopicConstants.AccountCreated);

        return app;
    }
}