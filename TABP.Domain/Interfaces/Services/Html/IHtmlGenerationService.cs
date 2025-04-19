namespace TABP.Domain.Interfaces.Services.Html;
public interface IHtmlGenerationService<T> where T : class
{
    string GenerateHtml(T document);
}

