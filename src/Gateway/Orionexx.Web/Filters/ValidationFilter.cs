using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Orionexx.Web.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        var model = context.GetArgument<T>(0);

        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var problemDetails = new ValidationProblemDetails
                {
                    Errors = new Dictionary<string, string[]>()
                    {
                        { "ValidationErrors", validationResult.Errors.Select(e => e.ErrorMessage).ToArray() }
                    },
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation failed",
                    Detail = "One or more validation errors occurred.",
                };
                return Results.BadRequest(problemDetails);
            }
        }

        return await next(context);
    }
}
