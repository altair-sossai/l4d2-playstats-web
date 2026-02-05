using L4D2PlayStats.Sdk.Punishments.Commands;
using L4D2PlayStats.Sdk.Punishments.Results;
using Refit;

namespace L4D2PlayStats.Sdk.Punishments;

public interface IPunishmentsService
{
    [Post("/api/punishments")]
    Task<PunishmentResult> PostAsync([Body] PunishmentCommand command, CancellationToken cancellationToken);

    [Delete("/api/punishments/{communityId}")]
    Task DeleteAsync(string communityId, CancellationToken cancellationToken);
}