using L4D2PlayStats.Core.AntiCheat.Responses;
using Refit;

namespace L4D2PlayStats.Core.AntiCheat.Services;

public interface IAntiCheatService
{
    [Get("/v1/client/version")]
    Task<ClientVersionResponse?> GetLatestVersionAsync(CancellationToken cancellationToken);
}