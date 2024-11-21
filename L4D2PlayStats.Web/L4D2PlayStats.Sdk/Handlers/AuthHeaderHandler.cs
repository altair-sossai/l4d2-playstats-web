namespace L4D2PlayStats.Sdk.Handlers;

public class AuthHeaderHandler(string serverId, string secret) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.TryAddWithoutValidation("Authorization", $"{serverId}:{secret}");

        return base.SendAsync(request, cancellationToken);
    }
}