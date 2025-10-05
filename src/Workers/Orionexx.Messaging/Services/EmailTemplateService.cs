using RazorLight;

namespace Orionexx.Messaging.Services;

public interface IEmailTemplateService
{
    Task<string> ConvertTemplateToHtml(string templateName, object? metaData);
    string GetEmailTemplateNameFromType(string emailType);
    string GetEmailSubjectFromType(string emailType);
}

public class EmailTemplateService : IEmailTemplateService
{
    private readonly RazorLightEngine _engine = new RazorLightEngineBuilder()
        .UseFileSystemProject(Path.Combine(Directory.GetCurrentDirectory(), "Templates"))
        .UseMemoryCachingProvider()
        .Build();

    private async Task<string> RenderAsync<TModel>(string templateKeyOrPath, TModel model)
    {
        return await _engine.CompileRenderAsync(templateKeyOrPath, model);
    }

    public async Task<string> ConvertTemplateToHtml(string templateName, object? metaData)
    {
        var renderedTemplate = await RenderAsync(templateName, metaData);
        return renderedTemplate;
    }

    public string GetEmailTemplateNameFromType(string eventName) => $"{eventName}.cshtml";

    public string GetEmailSubjectFromType(string eventName)
    {
        var templateName = eventName switch
        {
            "AccountCreated" => "Confirm Your Account",
            _ => throw new ArgumentOutOfRangeException(nameof(eventName), eventName, null)
        };
        return templateName;
    }
}
