namespace RxGroup.Extensions;

public static class RequestExtension
{
    private static readonly List<string> Patterns = new() {"json", "text/plain"};

    public static bool IsRequestBodyTextOrJson (this HttpRequest request)
    {
        try
        {
            if (!request.Headers.TryGetValue("Content-Type", out var contentTypeValues)) return false;

            var contentType = contentTypeValues.FirstOrDefault();

            return !string.IsNullOrEmpty(contentType) && Patterns.Any(x => contentType.Contains(x));
        }
        catch (Exception)
        {
            return false;
        }
    }
}