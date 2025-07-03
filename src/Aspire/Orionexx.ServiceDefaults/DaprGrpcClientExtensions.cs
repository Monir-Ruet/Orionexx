using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Orionexx.ServiceDefaults;

public static class DaprGrpcClientExtensions
{
    public static IHttpClientBuilder AddDaprGrpcClient<TClient>(
        this IHostApplicationBuilder builder,
        string appId)
        where TClient : class
    {
        return builder.Services.AddGrpcClient<TClient>((provider, options) =>
            {
                var daprPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "50001";
                options.Address = new Uri($"http://localhost:{daprPort}");
            })
            .AddInterceptor(() => new DaprAppIdInterceptor(appId));
    }
}

public class DaprAppIdInterceptor(string appId) : Interceptor
{
    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        where TRequest : class
        where TResponse : class
    {
        var headers = context.Options.Headers ?? new Metadata();
        headers.Add("dapr-app-id", appId);

        var newOptions = context.Options.WithHeaders(headers);
        var newContext = new ClientInterceptorContext<TRequest, TResponse>(
            context.Method,
            context.Host,
            newOptions);

        return base.AsyncUnaryCall(request, newContext, continuation);
    }
}
